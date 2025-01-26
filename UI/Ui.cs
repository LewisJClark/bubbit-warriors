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
		if (Input.IsActionJustPressed("SpawnWarrior")) 
			OnUnitButtonPressed?.Invoke(0);
		if (Input.IsActionJustPressed("SpawnPufferfish"))
			OnUnitButtonPressed?.Invoke(1);
    }

    public void SetCurrencyAmount(Team team, int amount) {
		if (team == Team.Friendly)
			_currencyLabel.Text = $"Bubbits: {amount}";
	}
}
