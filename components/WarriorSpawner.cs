using Godot;
using System;

public partial class WarriorSpawner : Node3D
{

    [Export]
	public Sprite3D ground;

	float laneWidth;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// ground = GetNode<Sprite3D>("Main/Ground");
		GD.Print("Got Ground: ", ground);
		laneWidth = ground.PixelSize * ground.Scale.Y * ground.Texture.GetSize().Y;
		GD.Print("Lane Width: ", laneWidth);
		float laneEdge = ground.PixelSize * ground.Scale.X * ground.Texture.GetSize().X * 0.5f;
		GD.Print("Lane Edge: ", laneEdge);
		// Position = new Vector3(laneEdge, 0, 0);

		// instance
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print("Hello World!");
	}
}
s