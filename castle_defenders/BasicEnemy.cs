using Godot;
using System;

public partial class BasicEnemy : BaseMob
{
	public override void _Ready()
	{
		// Call base setup
		base._Ready();

		// Set specific values for this enemy type
		speed = 3;
		health = 100;
		damage = 10;

		GD.Print("BasicEnemy ready.");
	}
}
