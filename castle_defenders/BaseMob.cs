using Godot;
using System;

public abstract partial class BaseMob : CharacterBody3D
{
	protected float _Speed = 5.0f;
	protected int _Health = 20;
	protected int _Damage = 2;
	protected float _SeeRange = 10f;
	protected float _AttackRange = 1.0f;
	protected NavigationAgent3D _NavAgent;
	protected Node3D chest;
	protected Player _Player;
	protected AnimationTree _AnimTree;
	protected bool hasAttacked = false;
	[Signal]
	public delegate void AttackedEventHandler(Node target);

	public override void _Ready()
	{
		//_NavAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		chest = GetNode<StaticBody3D>("/root/Main/Chest");
		_Player = GetNode<Player>("/root/Main/Player");
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
	
	public bool TargetInRange() {
		return GlobalPosition.DistanceTo(_Player.GlobalPosition) < _AttackRange;
	}
	
	public async void Attack(Player player)
	{
		await ToSignal(GetTree().CreateTimer(0.4), "timeout");
		//player.TakeDamage(10);
		if (TargetInRange()) {
			EmitSignal(SignalName.Attacked, _Damage);
		}
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
		_Health -= damage;
		if (_Health <= 0) {
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
		velocity.X = direction.X * _Speed;
		velocity.Z = direction.Z * _Speed;

		Velocity = velocity;

		MoveAndSlide();
	}
}
