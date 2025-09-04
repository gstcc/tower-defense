using Godot;
using System;

public abstract partial class BaseMob : CharacterBody3D
{
	public int speed = 5;
	public int health = 100;
	public int damage = 10;
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
		velocity.X = direction.X * speed;
		velocity.Z = direction.Z * speed;

		Velocity = velocity;

		MoveAndSlide();
	}
}
