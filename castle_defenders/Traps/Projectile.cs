using Godot;
using System;
using System.Collections.Generic;


public abstract partial class Projectile : Area3D
{
	[Export] protected bool _IsFired = false;
	[Export] protected float _Speed = 30.0f;
	[Export] protected Vector3 _Direction;
	[Export] protected int _Damage = 5;
	private Vector3 _velocity = Vector3.Zero;
	private bool _HasCollided = false;
	protected HashSet<BaseMob> _HitMobs = new HashSet<BaseMob>();

	public override void _PhysicsProcess(double delta)
	{
		if (!_IsFired || _HasCollided)
			return;
			
		if (_WillCollide()) {
			 _velocity = Vector3.Zero;
			_HasCollided=true;
			return;
		}

		// Apply gravity to Y component
		_velocity.Y += -GetGravity() * (float)delta;

		// Maintain horizontal movement (X and Z)
		Vector3 horizontalDirection = _Direction.Normalized();
		_velocity.X = horizontalDirection.X * _Speed;
		_velocity.Z = horizontalDirection.Z * _Speed;

		// Move the projectile
		GlobalPosition += _velocity * (float)delta;
	}
	
	public virtual bool _WillCollide() {return false;}
	
	public virtual void Fire() {
		GD.Print("Fired");
		_Direction = GlobalTransform.Basis.X;
		_velocity = _Direction;
		_velocity.Y = 7.0f;
		_IsFired = true;
		_HitMobs.Clear();
	}
}
