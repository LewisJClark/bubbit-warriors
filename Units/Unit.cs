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

	// Health of a Unit
	protected uint health;
	public uint maxHealth;

	[Signal]
    public delegate void HealthDepletedEventHandler();

	// public Unit Target = null;

	protected Node3D _model;
	protected Area3D _hitbox;

	public Unit(uint maxHealth) {
		this.maxHealth = maxHealth;
		this.health = maxHealth;
	}

	public override void _Ready()
	{
		_model = GetNode<Node3D>("Model");
		_hitbox = GetNode<Area3D>("Model/Hitbox");
	}

	/** Damage this unit. If the health falls to 0 then the Unit will be queued for deletion.
	params:
		power: attack damage.
	returns:
		bool: Was Unit killed.
	*/
	public bool Damage(uint power) {
		if (health <= power) {
			GD.Print("Unit Killed");
			this.QueueFree();
			EmitSignal(SignalName.HealthDepleted);
			return true;
		}
		else {
			this.health -= power;
			return false;
		}
	}

}
