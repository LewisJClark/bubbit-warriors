using Godot;
using System;

public partial class Ui : Control
{
	public delegate void UnitButtonPressHandler(int unitType);
	public UnitButtonPressHandler OnUnitButtonPressed;

	private Button _spawnButton;
	private Label _currencyLabel;

	public override void _Ready()
	{
		_spawnButton = GetNode<Button>("SpawnButton");
		_spawnButton.Pressed += () => OnUnitButtonPressed?.Invoke(0);

		_currencyLabel = GetNode<Label>("CurrencyLabel");
		_spawnButton.GrabFocus();
	}

	public void SetCurrencyAmount(Team team, int amount) {
		_currencyLabel.Text = $"Bubbits: {amount}";
	}
}
