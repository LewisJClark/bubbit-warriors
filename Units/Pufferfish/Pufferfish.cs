using Godot;
using System;
using System.Collections.Generic;

public partial class Pufferfish : Unit
{	
	private static uint MAX_HEALTH = 40;
	private static uint BASE_DAMAGE = 2;
	private static double BASE_ATTACK_COOLDOWN = 2.0;
	private static float BASE_ATTACK_DISTANCE = 1.0f;

	private Timer _cooldownTimer;
	private Vector3 _velocity;

	private Area3D _hitZone;
	private bool _canAttack = true;

	public Pufferfish() : base(MAX_HEALTH) {
		
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

		// Temporary until unit gets made
		_unitModel.Scale = new Vector3(1.0f, 1.0f, 1.0f);

		_cooldownTimer = GetNode<Timer>("CooldownTimer");
		_cooldownTimer.Timeout += () => _canAttack = true;

		_hitZone = GetNode<Area3D>("HitZone");
	}

	public override void _PhysicsProcess(double delta)
	{	
		var distToBase = _targetBaseX - Position.X;
		Unit target = _FindUnitToAttack();

		if (target != null) {
			// LookAt(target.Position);
			if (Position.DistanceTo(target.Position) < BASE_ATTACK_DISTANCE)
				Attack(target);
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

	private void Attack(Unit target)
	{
		if (!_canAttack)
			return;
		
		if (target == null) {
			return;
		}
		LookAt(target.Position);

		Godot.Collections.Array<Area3D> targetHitboxes = _hitZone.GetOverlappingAreas();

		List<Unit> targets = new List<Unit>();
		foreach (Area3D hitbox in targetHitboxes) {
			if (hitbox.Owner is not Unit unit || unit.Team == Team)
				continue;
			targets.Add(unit);
		}

		foreach (Unit unit in targets) {
			unit.Damage(BASE_DAMAGE);
		}

		_canAttack = false;
		_cooldownTimer.Start(BASE_ATTACK_COOLDOWN);
	}

}
