using Godot;
using System;

public partial class EnemyPlayerController : Node3D
{
	[Export] public Base ControlledBase = null;

	private bool _onCooldown = false;
	private Timer _spawnCooldownTimer;

    public override void _Ready()
    {
		_spawnCooldownTimer = GetNode<Timer>("SpawnCooldownTimer");
		_spawnCooldownTimer.Timeout += () => _onCooldown = false;
    }

    public override void _Process(double delta)
	{
		if (ControlledBase == null) 
			return;
		// Just spam out the cheapest unit whenever we have enough currency.
		if (!_onCooldown && ControlledBase.Currency >= ControlledBase.UnitCosts[0]) {
			ControlledBase.SpawnUnit(0);
			_spawnCooldownTimer.Start();
			_onCooldown = true;
		}
	}
}
