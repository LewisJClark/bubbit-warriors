using Godot;
using System;

public enum Team 
{
	Friendly,
	Enemy
}

public partial class Base : Node3D
{
	public delegate void BaseDestroyedHandler(Base destroyed);
	public BaseDestroyedHandler OnDestroyed;

	public delegate void UnitSpawnedHandler(Node3D unit);
	public UnitSpawnedHandler OnUnitSpawned;

	[Export] public Team Team = Team.Friendly;
	[Export] public Ui UI;
	[Export] public int CurrencyPerSecond = 10;

	// Uunit scenes and costs.
	[Export] public PackedScene[] _unitScenes;
	[Export] private int[] _unitCosts;

	public int Health { get; private set; } = 10;
	public int Currency { get; private set; } = 100;

	private Timer _currencyTimer;

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
				if (Currency < _unitCosts[unitType])
					return;
				Currency -= _unitCosts[unitType];
				var warrior = _unitScenes[unitType].Instantiate<Warrior>();
				OnUnitSpawned?.Invoke(warrior);
				warrior.GlobalPosition = Position;
				break;
		}
		UI.SetCurrencyAmount(Team, Currency);
	}
}
