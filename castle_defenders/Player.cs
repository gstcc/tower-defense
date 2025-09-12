using Godot;
using System;

public partial class Player : CharacterBody3D
{
	private const float Speed = 5.0f;
	private const float Acceleration = 0.5f;
	private const float JumpVelocity = 4.5f;
	private const float MouseSensitivity = 0.25f;
	private const float RotationSpeed = 12.0f; 
	private const int Damage = 20;
	private Vector2 CameraInputDirection;
	private Vector3 _LastMovementDirection;
	private Node3D _CameraPivot;
	private Camera3D _Camera;
	private Area3D _HitBox;
	private Knight _Skin;
	private bool isAttacking = false;
	private float attackTimer = 0.0f;
	private const float ATTACK_DURATION = 0.5f;
	
	// https://www.youtube.com/watch?v=JlgZtOFMdfc 24.10
	public override void _UnhandledInput(InputEvent ev) 
	{
		// Add check for MouseModeCaptured
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
		_Skin = GetNode<Knight>("%Knight");
		_HitBox = GetNode<Area3D>("%HitBox");
	}
	
	public override void _Input(InputEvent ev) 
	{
		if (ev.IsActionPressed("left_click")) {
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}
	
	public void Attack()
	{
		var enemies = _HitBox.GetOverlappingBodies();

		foreach (var body in _HitBox.GetOverlappingBodies())
		{
			if (body is BaseMob enemy)
			{
				enemy.Hurt(Damage);
			}
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
		float yVelocity = velocity.Y;
		bool isStartingJump = false;

		// Apply gravity
		if (!IsOnFloor())
			velocity += GetGravity() * (float)delta;

		// Jump
		if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
			velocity.Y = JumpVelocity;
			isStartingJump = true;	
		}

		// Camera-relative movement
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 forward = _Camera.GlobalTransform.Basis.Z;
		Vector3 right = _Camera.GlobalTransform.Basis.X;
		Vector3 moveDirection = forward * inputDir.Y + right * inputDir.X;

		if (inputDir != Vector2.Zero)
		{
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
		if (moveDirection.Length() > 0.2f) {
			_LastMovementDirection = moveDirection;
		}

		float targetAngle = (-Vector3.Forward).SignedAngleTo(_LastMovementDirection, Vector3.Up);

		Vector3 skinRotation = _Skin.GlobalRotation;
		skinRotation.Y = Mathf.LerpAngle(skinRotation.Y, targetAngle, (float)(RotationSpeed*delta));
		_Skin.GlobalRotation = skinRotation;
		float groundSpeed = Velocity.Length();
		attackTimer -= (float)delta;
		
		
		if (attackTimer <= 0f)
		{
			isAttacking = false;
		} else {
			return;
		}
		
		if (isStartingJump) {
			_Skin.Jump();
		} else if (!IsOnFloor() && velocity.Y < 0.0f) {
			_Skin.Fall();
		} else if (Input.IsActionJustPressed("left_click")) {
			_Skin.Attack();
			isAttacking = true;
			attackTimer = ATTACK_DURATION;
			Attack();
		} else if (IsOnFloor() && !isAttacking) {
			if (groundSpeed > 0.0f) {
				_Skin.Move();
			} else {
				_Skin.Idle();
			}	
		}
	}

}
