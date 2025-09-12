using Godot;
using System;

public abstract partial class BaseMob : CharacterBody3D
{
	public int Speed = 5;
	public int Health = 20;
	public int Damage = 10;
	NavigationAgent3D navAgent;
	StaticBody3D chest;

	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		chest = GetNode<StaticBody3D>("/root/Main/Chest");
		if (chest == null)
		{
			GD.PrintErr("Chest node not found at /root/Main/Chest");
			return;
		}
		MakePath();
	}

	public void MakePath()
	{
		navAgent.TargetPosition = chest.GlobalPosition;
	}
	
	private void Die()
	{
		GD.Print($"{Name} died!");
		//Should killed mobs be removed to save performance?
		// Maybe save dead bodies for a while and then despawn?
		//QueueFree();
	}
	
	public virtual void Hurt(int damage)
	{
		GD.Print("Hurt");
		Health -= damage;
		if (Health <= 0) {
			GD.Print("Dead");
			Die();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (chest == null)
			return;

		// Update target only if the chest moved (optional)
		// MakePath();

		// Get the next position on the path
		Vector3 nextPos = navAgent.GetNextPathPosition();

		// Direction to next position
		Vector3 direction = (nextPos - GlobalPosition).Normalized();

		// Calculate velocity with speed
		Vector3 velocity = Velocity;

		// Apply gravity
		if (!IsOnFloor())
			velocity += GetGravity() * (float)delta;

		// Move toward next position on the path, keep Y velocity for gravity
		velocity.X = direction.X * Speed;
		velocity.Z = direction.Z * Speed;

		Velocity = velocity;

		MoveAndSlide();
	}
}
