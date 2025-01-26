using Godot;
using System;

public partial class Main : Node3D
{
	private Base _friendlyBase;
	private Base _enemyBase;

	[Export] private PackedScene _gameOverScene;

	[Export] private AudioManager _audioManager;
	[Export] private Camera3D _singleplayerCamera;
	[Export] private Camera3D _multiplayerCamera;
	[Export] private EnemyPlayerController _aiController;

	public override void _Ready()
	{
		_friendlyBase = GetNode<Base>("FriendlyBase");
		_friendlyBase.OnUnitSpawned += UnitSpawned;
		_friendlyBase.OnDestroyed += () => ShowGameOver(2);

		_enemyBase = GetNode<Base>("EnemyBase");
		_enemyBase.OnUnitSpawned += UnitSpawned;
		_enemyBase.OnDestroyed += () => ShowGameOver(1);

		_audioManager.PlayMusicStream();
		Game.AudioManager = _audioManager;

		if (Game.IsLocalMultiplayer) {
			_aiController.QueueFree();
			_singleplayerCamera.QueueFree();
			_multiplayerCamera.Current = true;
		}
		else {
			_multiplayerCamera.QueueFree();
			_singleplayerCamera.Current = true;
		}
	}

    public override void _Process(double delta)
    {
        if (Input.IsPhysicalKeyPressed(Key.Shift))
			Game.ShowTargets = true;
		else
			Game.ShowTargets = false;
		// Game.ShowTargets = true;
    }

	public void ShowGameOver(int winner) {
		Game.Winner = winner;
		var gameOverMenu = _gameOverScene.Instantiate();
		GetTree().Root.AddChild(gameOverMenu);
		QueueFree();
	}

    private void UnitSpawned(Unit unit) {
		AddChild(unit);
	}

}
