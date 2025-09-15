using Godot;
using System;

public abstract partial class BaseMob : CharacterBody3D
{
	public float Speed = 5.0f;
	public int Health = 20;
	public int Damage = 10;
	protected float _SeeRange = 10f;
	public const float AttackRange = 1.0f;
	public NavigationAgent3D _NavAgent;
	public Node3D chest;
	protected CharacterBody3D _Player;
	public AnimationTree _AnimTree;

	public override void _Ready()
	{
		//_NavAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		chest = GetNode<StaticBody3D>("/root/Main/Chest");
		_Player = GetNode<CharacterBody3D>("/root/Main/Player");
		if (chest == null)
		{
			GD.PrintErr("Chest node not found at /root/Main/Chest");
			return;
		}
		MakePath();
	}

	public virtual void MakePath()
	{
		if (!IsPlayerSeeable()) {
			_NavAgent.TargetPosition = chest.GlobalPosition;
		} else {
			_NavAgent.TargetPosition = _Player.GlobalPosition;
		}
	}
	
	protected virtual bool IsPlayerSeeable() {
		return GlobalPosition.DistanceTo(_Player.GlobalPosition) < _SeeRange;
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
		Vector3 nextPos = _NavAgent.GetNextPathPosition();

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
