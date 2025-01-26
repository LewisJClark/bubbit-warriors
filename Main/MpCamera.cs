using Godot;
using System;

public partial class MpCamera : Camera3D
{
	private Tween _moveTween;

	public override void _Ready()
	{
		_moveTween = GetTree().CreateTween();
		_moveTween.SetLoops();
		_moveTween.TweenProperty(this, "position:x", -1.2, 2.0f);
		_moveTween.TweenProperty(this, "position:x", 1.2, 2.0f);
	}
}
