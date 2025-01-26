using Godot;
using System;
using System.Collections.Generic;

// Base class for all unit types.
// Contains stats and node references that should be common amongst all.

public partial class Unit : CharacterBody3D
{
	[Export] protected PackedScene fish_blue;
	[Export] protected PackedScene fish_red;
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

	// targeting
	protected float _targetBaseX;
	protected Vector3 _targetBasePosition;

		// Target Detection
	private Area3D _detectionArea;
	private List<Unit> _localEnemyUnits;

	public Unit(uint maxHealth) {
		this.maxHealth = maxHealth;
		this.health = maxHealth;

		_collisionUnits = new List<Unit>();
		_localEnemyUnits = new List<Unit>();
	}

	public override void _Ready()
	{
		_model = GetNode<Node3D>("Model");
		_hitbox = GetNode<Area3D>("Model/Hitbox");

		_collisionArea = GetNode<Area3D>("CollisionArea");
		_collisionArea.AreaEntered += _OnCollisionAreaEnter;
		_collisionArea.AreaExited += _OnCollisionAreaExit;

		_targetLine = GetNode<TargetLine>("DebugTargetLine");

		
		_detectionArea = GetNode<Area3D>("DetectionArea");
		_detectionArea.AreaEntered += _OnDetectionAreaEntered;
		_detectionArea.AreaExited += _OnDetectionAreaExited;

		switch (Team) {
			case Team.Friendly:
				_targetBaseX = Game.EnemyBase.Position.X;
				_targetBasePosition = Game.EnemyBase.Position;
				_unitModel = fish_blue.Instantiate<Node3D>();
				AddChild(_unitModel);
				break;
			case Team.Enemy:
				_targetBaseX = Game.FriendlyBase.Position.X;
				_targetBasePosition = Game.FriendlyBase.Position;
				_unitModel = fish_red.Instantiate<Node3D>();
				AddChild(_unitModel);
				break;
			default:
				throw new NotImplementedException("Unit team is unrecognised");
		}
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
		// if (Game.ShowTargets) {
		// 	_targetLine.setStart(Position);
		// 	_targetLine.SetEnd(target);
		// }

		Vector3 directionVector = Position.DirectionTo(target);

		Vector3 separationVector = new Vector3();
		Vector3 finalVelocity;
		// _collisionUnits.RemoveAll((unit) => IsInstanceValid(unit));
		if (_collisionUnits.Count > 0) {
			foreach (Unit unit in _collisionUnits) {
				Vector3 displacement = Position - unit.Position;
				float distance = displacement.Length();
				if (distance == 0) continue;
				separationVector += displacement.Normalized() / distance;
			}
			// separationVector = separationVector.LimitLength();
			
			finalVelocity = (directionVector + separationVector * spreadFactor).Normalized();

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
		MoveAndCollide(finalVelocity* MoveSpeed * (float)delta);
		// Position += finalVelocity* MoveSpeed * (float)delta;
		// Velocity = velocity;
		// MoveAndSlide();

		// Reset Location back in bounds
		if (Position.Z >  GAME_BOUNDARY) Position = new Vector3(Position.X, Position.Y,  GAME_BOUNDARY);
		if (Position.Z < -GAME_BOUNDARY) Position = new Vector3(Position.X, Position.Y, -GAME_BOUNDARY);
	}

	protected Unit _FindUnitToAttack() {
		_localEnemyUnits.RemoveAll((unit) => !IsInstanceValid(unit));
		if (_localEnemyUnits.Count == 0) return null;
		Unit target = _localEnemyUnits[0];
		float distance = Position.DistanceTo(target.Position);
		foreach (Unit unit in _localEnemyUnits) {
			float newDistance = Position.DistanceTo(unit.Position);
			if (newDistance < distance) {
				distance = newDistance;
				target = unit;
			}
		}
		return target;
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

	private void _OnDetectionAreaEntered(Area3D other) 
	{
		// GD.Print("Unit entered");
        if (other.Owner is not Unit unit || unit.Team == Team)
            return;
		
		_localEnemyUnits.Add(unit);
		// unit.TreeExiting += () => _localEnemyUnits.Remove(unit);
		// GD.Print("Unit added");
    }

	private void _OnDetectionAreaExited(Area3D other)	{
		if (other.Owner is not Unit unit)
            return;
		_localEnemyUnits.Remove(unit);
		// GD.Print("Unit removed");
	}

}
