using Godot;

public partial class MainMenu : Control
{
	[Export] private Button _singleplayerButton;
	[Export] private Button _multiplayerButton;
	[Export] private Button _quitButton;

	[Export] private PackedScene _mainScene;
	[Export] private AudioManager _audioManager;

	public override void _Ready()
	{
		_audioManager.PlayMusicStream();
		_singleplayerButton.Pressed += () => {
			var main = _mainScene.Instantiate();
			GetTree().Root.AddChild(main);
			QueueFree();
		};

		_multiplayerButton.Pressed += () => {
			// Launch the game scene but without the
			// AI enemy controller. Setting the Enemy base's
			// team to Team.EnemyPlayer instead of Team.Enemy.
			Game.IsLocalMultiplayer = true;
			var main = _mainScene.Instantiate();
			GetTree().Root.AddChild(main);
			QueueFree();
		};

		_quitButton.Pressed += () => GetTree().Quit();

		_singleplayerButton.GrabFocus();
	}
}
