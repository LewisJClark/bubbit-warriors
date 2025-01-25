using Godot;
using System;

public partial class WarriorSpawner : Node3D
{

    [Export]
	public Sprite3D ground;

	[Export]
	string WarriorStructure;

	float laneWidth;

	PackedScene warriorPrefab;

	Random rand;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rand = new Random();
		// ground = GetNode<Sprite3D>("Main/Ground");
		GD.Print("Got Ground: ", ground);
		laneWidth = ground.PixelSize * ground.Scale.Y * ground.Texture.GetSize().Y * 0.5f;
		GD.Print("Lane Width: ", laneWidth);
		float laneEdge = ground.PixelSize * ground.Scale.X * ground.Texture.GetSize().X * 0.5f;
		GD.Print("Lane Edge: ", laneEdge);
		// Position = new Vector3(laneEdge, 0, 0);

		warriorPrefab = GD.Load<PackedScene>(WarriorStructure);
		Node warrior = warriorPrefab.Instantiate();
		AddChild(warrior);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CreateWarriorAtJoystick();
	}

	private void CreateWarriorAtJoystick() {
		// Fake Joystick
		float angle = 0.0f;
		if (Input.IsPhysicalKeyPressed(Key.Key1))
			angle = 0.1f;
		else if (Input.IsPhysicalKeyPressed(Key.Key2))
			angle = 0.2f;
		else if (Input.IsPhysicalKeyPressed(Key.Key3))
			angle = 0.3f;
		else if (Input.IsPhysicalKeyPressed(Key.Key4))
			angle = 0.4f;
		else if (Input.IsPhysicalKeyPressed(Key.Key5))
			angle = 0.5f;
		else if (Input.IsPhysicalKeyPressed(Key.Key6))
			angle = 0.6f;
		else if (Input.IsPhysicalKeyPressed(Key.Key7))
			angle = 0.7f;
		else if (Input.IsPhysicalKeyPressed(Key.Key8))
			angle = 0.8f;
		else if (Input.IsPhysicalKeyPressed(Key.Key9))
			angle = 0.9f;
		else if (Input.IsPhysicalKeyPressed(Key.Key0))
			angle = 1.0f;
		
		// ideally get dot product of angle t0 joystick and use that
		// angle = cos(dot(Joystick_Vec2,  Vec2(0.0f, 1.0f)) / 180.0f

		CreateWarrior((int)((angle - 0.5) * 2 * laneWidth));
	}

	private void CreateWarrior(int position) {
		Node warrior = warriorPrefab.Instantiate();
		
		if (warrior is Node3D) {
			((Node3D)warrior).Position = new Vector3(this.Position.X, 0, position);
		}

		AddChild(warrior);
	}
}
