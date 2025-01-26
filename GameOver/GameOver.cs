using Godot;
using System;

public partial class GameOver : Control
{
	[Export] private Label _gameOverLabel;
	[Export] private Button _mainMenuButton;

	public override void _Ready()
	{
		_gameOverLabel.Text = Game.Winner == 1 ? "Player One wins!" : "Player Two wins!";
		_mainMenuButton.Pressed += () => {
			GetTree().Quit();
		};
	}
}
