using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

public partial class MantisShrimp : Unit
{	

	private static uint MAX_HEALTH = 20;
	private static uint BASE_DAMAGE = 4;
	private static double BASE_ATTACK_COOLDOWN = 1.0;
	private static float BASE_ATTACK_DISTANCE = 1.0f;

	private Timer _cooldownTimer;
	private Vector3 _velocity;


	private bool _canAttack = true;

	public MantisShrimp() : base(MAX_HEALTH) {
		
	}

	public override void _Ready()
	{
		base._Ready();

		switch (Team) {
			case Team.Friendly:
				break;
			case Team.Enemy:
			case Team.EnemyPlayer:
				break;
			default:
				throw new NotImplementedException("Unit team is unrecognised");
		}

		_cooldownTimer = GetNode<Timer>("CooldownTimer");
		_cooldownTimer.Timeout += () => _canAttack = true;
	}

	public override void _PhysicsProcess(double delta)
	{	
		var distToBase = _targetBaseX - Position.X;
		Unit target = _FindUnitToAttack();

		if (target != null) {
			// LookAt(target.Position);
			if (Position.DistanceTo(target.Position) < BASE_ATTACK_DISTANCE)
				AttackTarget(target);
			else
				MoveTowards(target.Position, delta, 0.3f);
		} else if (Math.Abs(distToBase) < AttackRange)
			AttackBase();
		else {
			Vector3 baseTarget = new Vector3(_targetBaseX, Position.Y, Position.Z);
			// LookAt(baseTarget);
			MoveTowards(baseTarget, delta, 0.005f);
		}
	}

    private void AttackBase() 
	{
		switch (Team) {
			case Team.Friendly:
				Game.EnemyBase.Damage(BASE_DAMAGE);
				break;
			case Team.Enemy:
			case Team.EnemyPlayer:
				Game.FriendlyBase.Damage(BASE_DAMAGE);
				break;
			default:
				throw new NotImplementedException("Unit team is unrecognised");
		}
	}

	private void AttackTarget(Unit target)
	{
		if (!_canAttack)
			return;
		
		if (target == null) {
			return;
		}
		LookAt(target.Position);

		bool killed = target.Damage(BASE_DAMAGE);
		
		_canAttack = false;
		_cooldownTimer.Start(BASE_ATTACK_COOLDOWN);
	}


}
