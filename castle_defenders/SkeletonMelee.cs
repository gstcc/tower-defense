using Godot;
using System;

public partial class SkeletonMelee : BaseMob
{
	private SkeletonMinion _Skin;
	private NavigationAgent3D _NavAgent;
	

	// Variables for gravity
	private Vector3 velocity;	
	private const float RotationSpeed = 12.0f; 

	public override void _Ready()
	{
		base._Ready();

		// Set specific values for this enemy type
		Speed = 3;
		Health = 100;
		Damage = 10;

		_Skin = GetNode<SkeletonMinion>("%SkeletonMinion");
		_NavAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");

		GD.Print("SkeletonMelee ready.");
	}

	public override void _PhysicsProcess(double delta)
	{
		// Compute desired direction from navigation
		Vector3 nextPathPosition = _NavAgent.GetNextPathPosition();
		Vector3 toTarget = nextPathPosition - GlobalPosition;

		// Apply horizontal velocity (ignoring vertical for now)
		velocity.X = toTarget.X * Speed;
		velocity.Z = toTarget.Z * Speed;

		// Apply gravity only to the y-axis (vertical component)
		if (!IsOnFloor()) // Check if the mob is in the air
		{
			velocity.Y += GetGravity().Y * (float)delta;
		}
		else
		{
			// Ensure the mob doesn't keep falling if it's on the ground
			velocity.Y = 0;
		}
		Velocity = velocity;
		MoveAndSlide(); // Vector3.Up defines the 'up' direction for gravity

		// Animation control: Check if the mob is moving
		float groundSpeed = velocity.Length();
		GD.Print(velocity);
		float targetAngle = (-Vector3.Forward).SignedAngleTo(velocity, Vector3.Up);
		Vector3 skinRotation = _Skin.GlobalRotation;
		skinRotation.Y = Mathf.LerpAngle(skinRotation.Y, targetAngle, (float)(RotationSpeed*delta));
		_Skin.GlobalRotation = skinRotation;
		
		if (groundSpeed > 0.0f)
		{
			_Skin.Move(); // Play walk animation
		}
		else
		{
			_Skin.Idle(); // Play idle animation
		}
	}
}
