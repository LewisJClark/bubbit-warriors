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

	private Node3D _friendlyModel;
	private Node3D _enemyModel;
	private Timer _currencyTimer;
	private Marker3D _spawnPoint;

	public override void _Ready()
	{
		_friendlyModel = GetNode<Node3D>("FriendlyModel");
		_enemyModel = GetNode<Node3D>("EnemyModel");
		_currencyTimer = GetNode<Timer>("CurrencyTimer");
		_currencyTimer.Timeout += () => {
			Currency += CurrencyPerSecond;
			UI.SetCurrencyAmount(Team, Currency);
		};

		if (Team == Team.Friendly) {
			_enemyModel.Visible = false;
			UI.OnUnitButtonPressed += (int unitType) => SpawnUnit(unitType);
			Game.FriendlyBase = this;
		}
		else {
			_friendlyModel.Visible = false;
			Game.EnemyBase = this;
		}

		_spawnPoint = GetNode<Marker3D>("SpawnPoint");
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (Team == Team.Friendly) {
			float newZ = _spawnPoint.Position.Z;
			newZ += Input.GetAxis("MoveSpawnerUp", "MoveSpawnerDown") * 3f * (float)delta;
			newZ = Math.Clamp(newZ, -2.4f, 2.4f);
			_spawnPoint.Position = new Vector3(_spawnPoint.Position.X, _spawnPoint.Position.Y, newZ);
		}
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
