using Godot;
using System;

public partial class GameOver : Control
{
	[Export] private Label _gameOverLabel;
	[Export] private Button _mainMenuButton;
	[Export] private string _mainMenuScene = "res://MainMenu/MainMenu.tscn";

	public override void _Ready()
	{
		_gameOverLabel.Text = Game.Winner == 1 ? "Player One wins!" : "Player Two wins!";
		_mainMenuButton.Pressed += () => {
			GetTree().ChangeSceneToFile(_mainMenuScene);
		};
		//GetTree().ChangeSceneToFile(_mainMenuScene);
	}
}
