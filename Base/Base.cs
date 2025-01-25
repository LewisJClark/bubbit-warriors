using Godot;
using System;

public enum Team 
{
	Friendly,
	Enemy
}

public partial class Base : Node3D
{
	public delegate void BaseDestroyedHandler(Base destroyed);
	public BaseDestroyedHandler OnDestroyed;

	[Export]
	public Team Team = Team.Friendly;

	public int Health { get; private set; } = 10;
	public int Currency { get; private set; } = 10;

	public override void _Ready()
	{
		if (Team == Team.Friendly)
			Game.FriendlyBase = this;
		else 
			Game.EnemyBase = this;
	}

	public override void _Process(double delta)
	{

	}

	public void Attack(int damage)
	{
		if (Currency >= damage) 
		{
			Currency -= damage;
		}
		else 
		{
			damage -= Currency;
			Currency = 0;
			Health -= damage;
		}
	}
}
