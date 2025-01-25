using Godot;
using System;

public partial class Main : Node3D
{
	private Base _friendlyBase;
	private Base _enemyBase;

	public override void _Ready()
	{
		_friendlyBase = GetNode<Base>("FriendlyBase");
		_friendlyBase.OnUnitSpawned += UnitSpawned;

		_enemyBase = GetNode<Base>("EnemyBase");
		_enemyBase.OnUnitSpawned += UnitSpawned;

	}

	private void UnitSpawned(Unit unit) {
		AddChild(unit);
	}
}
