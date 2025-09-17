using Godot;
using System;

public partial class BallistaArrow : Projectile
{
	public override void _Ready()
	{
		_Speed = 30;
		base._Ready();

	}
}
