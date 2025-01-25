using Godot;
using System;

public partial class Camera : Camera3D
{
	[Export]
	private float _moveSpeed = 32.0f;

	public override void _Process(double delta)
	{
		float newX = Position.X;
		if (Input.IsActionPressed("MoveCameraLeft"))
			newX -= _moveSpeed * (float)delta;
		if (Input.IsActionPressed("MoveCameraRight"))
			newX += _moveSpeed * (float)delta;
		Position = new Vector3(newX, Position.Y, Position.Z);
	}
}
