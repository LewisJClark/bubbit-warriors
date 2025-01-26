using Godot;
using System;

public partial class Main : Node3D
{
	private Base _friendlyBase;
	private Base _enemyBase;
	[Export]
	private AudioManager _audioManager;

	public override void _Ready()
	{
		_friendlyBase = GetNode<Base>("FriendlyBase");
		_friendlyBase.OnUnitSpawned += UnitSpawned;

		_enemyBase = GetNode<Base>("EnemyBase");
		_enemyBase.OnUnitSpawned += UnitSpawned;
		_audioManager.PlayMusicStream();
		Game.AudioManager = _audioManager;

	}

    public override void _Process(double delta)
    {
        if (Input.IsPhysicalKeyPressed(Key.Shift))
			Game.ShowTargets = true;
		else
			Game.ShowTargets = false;
		// Game.ShowTargets = true;
    }

    private void UnitSpawned(Unit unit) {
		AddChild(unit);
	}
}
