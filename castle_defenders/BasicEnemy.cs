using Godot;
using System;

public partial class BasicEnemy : BaseMob
{
	public override void _Ready()
	{
		// Call base setup
		base._Ready();

		// Set specific values for this enemy type
		Speed = 3;
		Health = 100;
		Damage = 10;

		GD.Print("BasicEnemy ready.");
	}	
}
