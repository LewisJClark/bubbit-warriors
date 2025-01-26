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
	public delegate void BaseDestroyedHandler();
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
			if (Team == Team.Enemy && Game.IsLocalMultiplayer) 
				Team = Team.EnemyPlayer;
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
			newZ += Input.GetAxis("ui_up", "ui_down") * 3f * (float)delta;
			newZ = Math.Clamp(newZ, -2.4f, 2.4f);
			_spawnPoint.Position = new Vector3(_spawnPoint.Position.X, _spawnPoint.Position.Y, newZ);
		}
		else if (Team == Team.EnemyPlayer) {
			float newZ = _spawnPoint.Position.Z;
			newZ -= Input.GetAxis("ui_up_p2", "ui_down_p2") * 3f * (float)delta;
			newZ = Math.Clamp(newZ, -2.4f, 2.4f);
			_spawnPoint.Position = new Vector3(_spawnPoint.Position.X, _spawnPoint.Position.Y, newZ);
		}
    }

    public void Damage(uint damage)
	{
		if (Currency >= damage) 
		{
			Currency -= (int)damage;
			UI.SetCurrencyAmount(Team, Currency);
		}
		else 
		{
			Health -= (int)damage - Currency;
			Currency = 0;

			if (Health <= 0) 
				OnDestroyed?.Invoke();
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
			case 1:
				if (Currency < UnitCosts[unitType])
					return;
				Currency -= UnitCosts[unitType];
				var pufferfish = UnitScenes[unitType].Instantiate<Pufferfish>();
				pufferfish.Team = Team;
				OnUnitSpawned?.Invoke(pufferfish);
				pufferfish.GlobalPosition = _spawnPoint.GlobalPosition;
				break;
			default:
				throw new NotImplementedException("Spawn not implemented for unit");
		}
		UI.SetCurrencyAmount(Team, Currency);
	}
}
