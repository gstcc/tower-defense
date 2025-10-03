using Godot;
using System;

public partial class DamageArea : Area3D
{
	
	public void CheckOverlappingBodies(int damage)
	{
		var overlappingBodies = GetOverlappingBodies();

		foreach (var body in overlappingBodies)
		{
			if (body is PhysicsBody3D physicsBody)
			{
				if (physicsBody.HasMethod("Hurt"))
				{
					physicsBody.Call("Hurt", damage);
				}
			}
		}
	}
}
