using Godot;
using System;

public partial class Player : CharacterBody3D
{
	private const float Speed = 3.0f;
	private const float Acceleration = 0.5f;
	private const float JumpVelocity = 1.5f;
	private const float MouseSensitivity = 0.25f;
	private const float RotationSpeed = 12.0f; 
	private const int Damage = 20;
	private Vector2 CameraInputDirection;
	private Vector3 _LastMovementDirection;
	private Node3D _CameraPivot;
	private Camera3D _Camera;
	private Area3D _HitBox;
	private Knight _Skin;
	private BaseMob _Enemy;
	private AnimationNodeStateMachinePlayback  _StateMachine;
	protected AnimationTree _AnimTree;
	private bool isAttacking = false;
	private float attackTimer = 0.0f;
	private const float ATTACK_DURATION = 0.5f;
	//private bool _IsBlocking = false;
	
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
		_AnimTree = GetNode<AnimationTree>("Knight/AnimationTree");
		_StateMachine = (AnimationNodeStateMachinePlayback)_AnimTree.Get("parameters/playback");
		_Enemy = GetNode<BaseMob>("/root/Main/Skeleton");
		_Enemy.Connect(BaseMob.SignalName.Attacked, new Callable(this, nameof(OnEnemyAttacked)));
	}
	
	private void OnEnemyAttacked(int damage)
	{
		//Blocking
		if (Input.IsActionPressed("right_click")) {
			GD.Print("Blocking while attackec");
		}
		GD.Print("Player received attack signal.");
		GD.Print(damage);
	}
	
	public void Hurt(int damage) {
		OnEnemyAttacked(damage);
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
		attackTimer -= (float)delta;
		
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (inputDir != Vector2.Zero) {
			_AnimTree.Set("parameters/conditions/Run", IsOnFloor());
			_AnimTree.Set("parameters/conditions/Idle", false);
			_AnimTree.Set("parameters/conditions/Block", false);
		} else {
			_AnimTree.Set("parameters/conditions/Run", false);
			// If we're falling we're not idle
			_AnimTree.Set("parameters/conditions/Idle", IsOnFloor());
			_AnimTree.Set("parameters/conditions/Block", false);
		}
		
		if (Input.IsActionPressed("right_click")) {
			_AnimTree.Set("parameters/conditions/Block", true);
			_AnimTree.Set("parameters/conditions/Run", false);
			_AnimTree.Set("parameters/conditions/Idle", false);
		}
		
		// Attack button was pressed
		if (Input.IsActionJustPressed("left_click")) {
			_AnimTree.Set("parameters/conditions/Attack", true);
			_AnimTree.Set("parameters/conditions/Idle", false);
		}
		// Jump
		if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
			_AnimTree.Set("parameters/conditions/Jump", true);
			_AnimTree.Set("parameters/conditions/Idle", false);
		}
		//Handle movement and animations
		Vector3 velocity = Velocity;
		string state = _StateMachine.GetCurrentNode();
		switch (state) {
			case "Run":
				// Apply gravity
				if (!IsOnFloor())
					velocity += GetGravity() * (float)delta;

				// Camera-relative movement
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
				break;
			case "Attack":
				if (attackTimer <= 0f) {
					isAttacking = true;
					attackTimer = ATTACK_DURATION;
					Attack();
				}
				velocity.X = 0;
				velocity.Z = 0;	
				_AnimTree.Set("parameters/conditions/Attack", attackTimer <= 0f);
				break;
			case "Hit":
				break;
			case "Block":
				velocity.X = 0;
				velocity.Z = 0;	
				break;
			case "Interact":
				break;
			case "Jump":
				if (IsOnFloor()) {
					velocity.Y = JumpVelocity;	
				}
				_AnimTree.Set("parameters/conditions/Jump", false);
				break;
			case "Fall":
				if (!IsOnFloor()) {
					velocity += GetGravity() * (float)delta;	
				} else {
					velocity = Vector3.Zero;
					if (inputDir != Vector2.Zero) {
						_AnimTree.Set("parameters/conditions/Run", true);
					} else {
						_AnimTree.Set("parameters/conditions/Run", false);
						_AnimTree.Set("parameters/conditions/Idle", true);
					}
				}
				break;
			default:
				_AnimTree.Set("parameters/conditions/Idle", true);
				break;
		}
		Velocity = velocity;
		MoveAndSlide();
		
		if (attackTimer <= 0f)
		{
			isAttacking = false;
		} else {
			return;
		}
	}

}
