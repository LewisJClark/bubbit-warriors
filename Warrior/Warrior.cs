using Godot;
using System;

public partial class Warrior : Node3D
{	
	[Export] public Team Team { get; private set; } = Team.Friendly;
	[Export] public float AttackRange { get; private set; } = 2;
	[Export] public float MoveSpeed { get; private set; } = 3;

	private Vector3 _targetPos;
	private Node3D _model;

	public override void _Ready()
	{
		_model = GetNode<Node3D>("Model");
		_targetPos = Team == Team.Friendly ? Game.EnemyBase.Position : Game.FriendlyBase.Position;
	}

	public override void _Process(double delta)
	{
		var dist = Position.DistanceTo(_targetPos);
		if (dist < AttackRange) {
			// Destroy the fish for now.
			QueueFree();
		}
		else {
			Position = new Vector3(Position.X + MoveSpeed * (float)delta, Position.Y, Position.Z);
		}
	}
}
