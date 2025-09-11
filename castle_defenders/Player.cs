using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float Acceleration = 0.5f;
	public const float JumpVelocity = 4.5f;
	public const float MouseSensitivity = 0.25f;
	Vector2 CameraInputDirection;
	private Node3D _CameraPivot;
	private Camera3D _Camera;
	
	// https://www.youtube.com/watch?v=JlgZtOFMdfc 24.10
	public override void _UnhandledInput(InputEvent ev) 
	{
		// Add check for MouseModeCaptured
		GD.Print(Input.GetMouseMode());
		if (ev is InputEventMouseMotion mouseMotion
		 && Input.GetMouseMode() == Godot.Input.MouseModeEnum.Captured)
		{
			CameraInputDirection = mouseMotion.Relative * MouseSensitivity;
		}
	}
		
	public override void _Ready()
	{
		_CameraPivot = GetNode<Node3D>("%CameraPivot");
		_Camera = GetNode<Camera3D>("%Camera3D");
	}
	
	public override void _Input(InputEvent ev) 
	{
		if (ev.IsActionPressed("left_click")) {
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 rotation = _CameraPivot.Rotation;
		rotation.X += (float)(CameraInputDirection.Y * delta);
		rotation.X = Mathf.Clamp(rotation.X, -Mathf.Pi / 6.0f, Mathf.Pi / 3.0f);
		rotation.Y -= (float)(CameraInputDirection.X * delta);
		_CameraPivot.Rotation = rotation;
		CameraInputDirection = Vector2.Zero;
		
		// Character movement
		Vector3 velocity = Velocity;

		// Apply gravity
		if (!IsOnFloor())
			velocity += GetGravity() * (float)delta;

		// Jump
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Camera-relative movement
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 forward = _Camera.GlobalTransform.Basis.Z;
		Vector3 right = _Camera.GlobalTransform.Basis.X;

		if (inputDir != Vector2.Zero)
		{
			Vector3 moveDirection = forward * inputDir.Y + right * inputDir.X;
			moveDirection.Y = 0.0f;
			moveDirection = moveDirection.Normalized();
 			//velocity = Mathf.MoveToward(moveDirection * Speed, Acceleration*delta);
			velocity.X = moveDirection.X * Speed;
			velocity.Z = moveDirection.Z * Speed;
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
