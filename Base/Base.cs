using Godot;
using System;

public enum Team 
{
	Friendly,
	Enemy,        // AI.
	EnemyPlayer,
}

public partial class Base : Node3D
{
	public delegate void BaseDestroyedHandler(Base destroyed);
	public BaseDestroyedHandler OnDestroyed;

	public delegate void UnitSpawnedHandler(Unit unit);
	public UnitSpawnedHandler OnUnitSpawned;

	[Export] public Team Team = Team.Friendly;
	[Export] public Ui UI;
	[Export] public int CurrencyPerSecond = 10;

	// Unit scenes and costs.
	[Export] public PackedScene[] UnitScenes;
	[Export] public int[] UnitCosts;

	public int Health { get; private set; } = 10;
	public int Currency { get; private set; } = 100;

	private Timer _currencyTimer;
	private Marker3D _spawnPoint;

	public override void _Ready()
	{
		_currencyTimer = GetNode<Timer>("CurrencyTimer");
		_currencyTimer.Timeout += () => { 
			Currency += CurrencyPerSecond;
			UI.SetCurrencyAmount(Team, Currency);
		};

		if (Team == Team.Friendly) {
			UI.OnUnitButtonPressed += (int unitType) => SpawnUnit(unitType);
			Game.FriendlyBase = this;
		}
		else 
			Game.EnemyBase = this;

		_spawnPoint = GetNode<Marker3D>("SpawnPoint");
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public void Attack(int damage)
	{
		if (Currency >= damage) 
		{
			Currency -= damage;
			UI.SetCurrencyAmount(Team, Currency);
		}
		else 
		{
			damage -= Currency;
			Currency = 0;
			Health -= damage;
		}
	}

	public void SpawnUnit(int unitType)
	{
		switch (unitType) {
			case 0:
				if (Currency < UnitCosts[unitType])
					return;
				Currency -= UnitCosts[unitType];
				var warrior = UnitScenes[unitType].Instantiate<Warrior>();
				warrior.Team = Team;
				OnUnitSpawned?.Invoke(warrior);
				warrior.GlobalPosition = _spawnPoint.GlobalPosition;
				break;
		}
		UI.SetCurrencyAmount(Team, Currency);
	}
}
