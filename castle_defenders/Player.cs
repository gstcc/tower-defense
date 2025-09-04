using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	private SpringArm3D _springArm;
	
	public override void _Ready()
	{
		_springArm = GetNode<SpringArm3D>("SpringArm3D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Apply gravity
		if (!IsOnFloor())
			velocity += GetGravity() * (float)delta;

		// Jump
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Camera-relative movement
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (inputDir != Vector2.Zero)
		{
			inputDir = inputDir.Normalized();

			// Get camera basis from the spring arm
			Basis camBasis = _springArm.GlobalTransform.Basis;
			Vector3 camForward = camBasis.Z;
			Vector3 camRight = camBasis.X;

			// Flatten direction vectors
			camForward.Y = 0;
			camRight.Y = 0;
			camForward = camForward.Normalized();
			camRight = camRight.Normalized();

			// Combine input direction with camera vectors
			Vector3 moveDir = (camForward * inputDir.Y + camRight * inputDir.X).Normalized();

			velocity.X = moveDir.X * Speed;
			velocity.Z = moveDir.Z * Speed;
		}
		else
		{
			// Deceleration
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

}
