using Godot;
using System;

// Base class for all unit types.
// Contains stats and node references that should be common amongst all.

public partial class Unit : Node3D
{
	[Export] public Team Team = Team.Friendly;
	[Export] public float AttackRange { get; protected set; } = 2;
	[Export] public float MoveSpeed { get; protected set; } = 3;
	[Export] public int CurrencyAwarded { get; protected set; } = 10;

	public Unit Target = null;

	protected Node3D _model;
	protected Area3D _hitbox;

	public override void _Ready()
	{
		_model = GetNode<Node3D>("Model");
		_hitbox = GetNode<Area3D>("Model/Hitbox");
	}

}
