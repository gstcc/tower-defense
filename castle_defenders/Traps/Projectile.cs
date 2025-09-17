using Godot;
using System;

public abstract partial class Projectile : Area3D
{
	protected bool _IsFired;
	protected float _Speed;
	
	public override void _PhysicsProcess(double delta) {
		if (!_IsFired) {
			return;
		}
		
		Vector3 velocity = Velocity;
		if (!IsOnFloor()) // Check if the mob is in the air
		{
			velocity.Y += GetGravity().Y * (float)delta;
		}
	}
	
	public void _OnBodyEntered(Node3D body) {
		
	}
	
	public void _OnBodyExited(Node3D body) {
	}

}
