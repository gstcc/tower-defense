using Godot;
using System;

public partial class BallistaArrow : Projectile
{
	public override void _Ready()
	{
		_Speed = 20;
		_Direction = GlobalTransform.Basis.X;
		GD.Print(_Direction);
		_Damage = 100;
		base._Ready();
	}
	
	public override bool _WillCollide() {
		RayCast3D ray = GetNode<RayCast3D>("RayCast3D");
		ray.ForceRaycastUpdate();
		if (ray.IsColliding()) {
			var collider = ray.GetCollider();
			if (collider is BaseMob mob)
			{
				if (!_HitMobs.Contains(mob)) {
					mob.Hurt(_Damage);
				 	_HitMobs.Add(mob);
				}
				return false;
			} else if (collider is Player player) {
				GD.Print("Collided with player");
				return false;
			}
			ray.QueueFree();
			GD.Print(collider);
			return true;
		}
		return false;
	}
}
