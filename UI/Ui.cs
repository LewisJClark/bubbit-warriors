using Godot;
using System;

public partial class Ui : Control
{
	public delegate void UnitButtonPressHandler(int unitType);
	public UnitButtonPressHandler OnUnitButtonPressed;

	private Button _spawnButton;
	private Button _spawnButton2;
	private Label _currencyLabel;

	public override void _Ready()
	{
		_spawnButton = GetNode<Button>("SpawnButton");
		_spawnButton.Pressed += () => OnUnitButtonPressed?.Invoke(0);

		_spawnButton2 = GetNode<Button>("SpawnButton");
		_spawnButton2.Pressed += () => OnUnitButtonPressed?.Invoke(1);

		_currencyLabel = GetNode<Label>("CurrencyLabel");
		// _spawnButton.GrabFocus();
	}

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (Input.IsActionJustPressed("ui_accept")) 
			Game.FriendlyBase.SpawnUnit(0);
			
		if (Game.IsLocalMultiplayer) {
			if (Input.IsActionJustPressed("ui_accept_p2"))
				Game.EnemyBase.SpawnUnit(0);
		}
    }

    public void SetCurrencyAmount(Team team, int amount) {
		if (team == Team.Friendly)
			_currencyLabel.Text = $"Bubbits: {amount}";
	}
}
