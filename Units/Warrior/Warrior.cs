using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

public partial class Warrior : Unit
{	
	private static uint MAX_HEALTH = 10;
	private static uint BASE_DAMAGE = 2;
	private static double BASE_ATTACK_COOLDOWN = 1.0;

	private Timer _cooldownTimer;
	private float _targetBaseX;
	private Vector3 _targetBasePosition;
	private Vector3 _velocity;

	// Target Detection
	private Area3D _detectionArea;
	private List<Unit> _localEnemyUnits;

	private bool _canAttack = true;

	public Warrior() : base(MAX_HEALTH){
		
	}

	public override void _Ready()
	{
		base._Ready();

		_targetBaseX = Team == Team.Friendly ? Game.EnemyBase.Position.X : Game.FriendlyBase.Position.X;
		_targetBasePosition = Team == Team.Friendly ? Game.EnemyBase.Position : Game.FriendlyBase.Position;

		_localEnemyUnits = new List<Unit>();

		_detectionArea = GetNode<Area3D>("DetectionArea");
		_detectionArea.AreaEntered += OnAreaEntered;
		_detectionArea.AreaExited += OnAreaExited;

		_cooldownTimer = GetNode<Timer>("CooldownTimer");
		_cooldownTimer.Timeout += () => _canAttack = true;
	}

	public override void _PhysicsProcess(double delta)
	{	
		var distToBase = _targetBaseX - Position.X;
		Unit target = _FindUnitToAttack();

		if (Math.Abs(distToBase) < AttackRange)
			AttackBase();
		else if (target != null) {
			LookAt(Position.DirectionTo(target.Position));
			AttackTarget(target);
		}
		else {
			LookAt(new Vector3(_targetBaseX, Position.Y, Position.Z));
			Vector3 velocity = Position.DirectionTo(_targetBasePosition) * MoveSpeed * (float)delta;
			Position += velocity;
		}
	}

	private Unit _FindUnitToAttack() {
		if (_localEnemyUnits.Count == 0) return null;
		Unit target = _localEnemyUnits[0];
		float distance = Position.DistanceTo(target.Position);
		foreach (Unit unit in _localEnemyUnits) {
			float newDistance = Position.DistanceTo(target.Position);
			if (newDistance < distance) {
				newDistance = distance;
				target = unit;
			}
		}
		return target;
	}

    private void AttackBase() 
	{

	}

	private void AttackTarget(Unit target)
	{
		if (!_canAttack)
			return;
		
		if (target == null) {
			GD.PrintErr("Cannot attack Null Target");
			return;
		}

		bool killed = target.Damage(BASE_DAMAGE);
		
		_canAttack = false;
		_cooldownTimer.Start(BASE_ATTACK_COOLDOWN);
	}

	private void OnAreaEntered(Area3D other) 
	{
		// GD.Print("Unit entered");
        if (other.Owner is not Unit unit || unit.Team == Team)
            return;
		
		_localEnemyUnits.Add(unit);
		unit.TreeExiting += () => _localEnemyUnits.Remove(unit);
		// GD.Print("Unit added");
    }

	private void OnAreaExited(Area3D other)	{
		if (other.Owner is not Unit unit)
            return;
		_localEnemyUnits.Remove(unit);
		// GD.Print("Unit removed");
	}

}
