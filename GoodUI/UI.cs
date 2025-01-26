using Godot;
using System;

public partial class UI : Node
{
	[Export] private Button _friendlyCharacter1;
	[Export] private Button _friendlyCharacter2;
	[Export] private Button _friendlyCharacter3;

	[Export] private Button _enemyCharacter1;
	[Export] private Button _enemyCharacter2;
	[Export] private Button _enemyCharacter3;

	[Export] private Label _friendlyCurrency;
	[Export] private Label _enemyCurrency;

	[Export] private ProgressBar _healthBar;

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (Input.IsActionJustPressed("SpawnUnit1P1")) { Game.FriendlyBase.SpawnUnit(0); }
		if (Input.IsActionJustPressed("SpawnUnit2P1")) { Game.FriendlyBase.SpawnUnit(1); }
		if (Input.IsActionJustPressed("SpawnUnit3P1")) { Game.FriendlyBase.SpawnUnit(2); }

		if (Game.IsLocalMultiplayer) {
			if (Input.IsActionJustPressed("SpawnUnit1P2")) { Game.EnemyBase.SpawnUnit(0); }
			if (Input.IsActionJustPressed("SpawnUnit2P2")) { Game.EnemyBase.SpawnUnit(1); }
			if (Input.IsActionJustPressed("SpawnUnit3P2")) { Game.EnemyBase.SpawnUnit(2); }
		}

		_healthBar.Value = Game.FriendlyBase.Health;	 
    }

    public void SetCurrencyAmount(Team team, int amount) { 
		if (team == Team.Friendly)
			_friendlyCurrency.Text = amount.ToString();
		else
			_enemyCurrency.Text = amount.ToString();
	}
}
