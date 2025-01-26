using Godot;
using System;
using System.Collections.Generic;

// Base class for all unit types.
// Contains stats and node references that should be common amongst all.

public partial class Unit : CharacterBody3D
{
	[Export] public Team Team = Team.Friendly;
	[Export] public float AttackRange { get; protected set; } = 5;
	[Export] public float MoveSpeed { get; protected set; } = 3;
	[Export] public int CurrencyAwarded { get; protected set; } = 10;
	[Export] public float GAME_BOUNDARY = 4.0f;

	// Health of a Unit
	protected uint health;
	public uint maxHealth;

	[Signal]
    public delegate void HealthDepletedEventHandler();

	// public Unit Target = null;

	protected Node3D _model;
	protected Area3D _hitbox;

	protected Node3D _unitModel;


	// Collision detection
	private Area3D _collisionArea;
	private List<Unit> _collisionUnits;
	private Vector3 lastMoveView = new Vector3();
	private TargetLine _targetLine;

	public Unit(uint maxHealth) {
		this.maxHealth = maxHealth;
		this.health = maxHealth;

		_collisionUnits = new List<Unit>();
	}

	public override void _Ready()
	{
		_model = GetNode<Node3D>("Model");
		_hitbox = GetNode<Area3D>("Model/Hitbox");

		_collisionArea = GetNode<Area3D>("CollisionArea");
		_collisionArea.AreaEntered += _OnCollisionAreaEnter;
		_collisionArea.AreaExited += _OnCollisionAreaExit;

		_targetLine = GetNode<TargetLine>("DebugTargetLine");

		
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

	protected void MoveTowards(Vector3 target, double delta, float spreadFactor) {
		if (Game.ShowTargets) {
			_targetLine.setStart(Position);
			_targetLine.SetEnd(target);
		}

		Vector3 directionVector = Position.DirectionTo(target);

		Vector3 separationVector = new Vector3();
		Vector3 finalVelocity;
		// _collisionUnits.RemoveAll((unit) => IsInstanceValid(unit));
		if (_collisionUnits.Count > 0) {
			foreach (Unit unit in _collisionUnits) {
				Vector3 displacement = Position - unit.Position;
				float distance = displacement.Length();
				if (distance == 0) continue;
				separationVector += displacement / distance;
			}
			// separationVector = separationVector.LimitLength();
			
			finalVelocity = directionVector + separationVector * spreadFactor;

		} else {
			finalVelocity = directionVector;
		}

		
		
		
		// GD.Print(directionVector, separationVector, finalVelocity);

		Vector3 newViewDirection = lastMoveView.Lerp(finalVelocity, (float)Math.Min(1.0, delta * 5.0));
		if (Game.ShowTargets){
			LookAt(target);
		} else {
			LookAt(Position + newViewDirection);
		}
		lastMoveView = newViewDirection;
		// MoveAndCollide(finalVelocity* MoveSpeed * (float)delta);
		Position += finalVelocity* MoveSpeed * (float)delta;
		// Velocity = velocity;
		// MoveAndSlide();

		// Reset Location back in bounds
		if (Position.Z >  GAME_BOUNDARY) Position = new Vector3(Position.X, Position.Y,  GAME_BOUNDARY);
		if (Position.Z < -GAME_BOUNDARY) Position = new Vector3(Position.X, Position.Y, -GAME_BOUNDARY);
	}

	private void _OnCollisionAreaEnter(Area3D other) {
		if (other.Owner is not Unit unit || unit.Team != Team)
			return;
		
		_collisionUnits.Add(unit);
		unit.TreeExiting += () => _collisionUnits.Remove(unit);
	}

	private void _OnCollisionAreaExit(Area3D other) {
		if (other.Owner is not Unit unit)
			return;
		
		_collisionUnits.Remove(unit);
	}

}
