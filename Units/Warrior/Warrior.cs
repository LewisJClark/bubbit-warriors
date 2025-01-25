using Godot;
using System;

public partial class Warrior : Unit
{	

	private Timer _cooldownTimer;
	private float _targetBaseX;
	private Vector3 _targetBasePosition;
	private Vector3 _velocity;

	private Area3D _detectionArea;

	public override void _Ready()
	{
		base._Ready();

		_targetBaseX = Team == Team.Friendly ? Game.EnemyBase.Position.X : Game.FriendlyBase.Position.X;
		_targetBasePosition = Team == Team.Friendly ? Game.EnemyBase.Position : Game.FriendlyBase.Position;

		_detectionArea = GetNode<Area3D>("DetectionArea");
		_detectionArea.AreaEntered += OnAreaEntered;

		_cooldownTimer = GetNode<Timer>("CooldownTimer");
	}

	public override void _PhysicsProcess(double delta)
	{	
		LookAt(new Vector3(_targetBaseX, Position.Y, Position.Z));

		var distToBase = _targetBaseX - Position.X;
		if (Math.Abs(distToBase) < AttackRange)
			AttackBase();
		else if (Target != null)
			AttackTarget();
		else {
			Vector3 velocity = Position.DirectionTo(_targetBasePosition) * MoveSpeed * (float)delta;
			Position += velocity;
		}
	}

    private void AttackBase() 
	{

	}

	private void AttackTarget()
	{

	}

	private void OnAreaEntered(Area3D other) 
	{
        if (other.Owner is not Unit unit || unit.Team == Team)
            return;
		if (Target == null) {
			Target = unit;
		}
		if (unit.Target == null) {
			unit.Target = this;
		}
    }

}
