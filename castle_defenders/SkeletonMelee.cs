using Godot;
using System;

public partial class SkeletonMelee : BaseMob
{
	private SkeletonMinion _Skin;

	// Variables for gravity
	private Vector3 velocity;	
	private const float RotationSpeed = 12.0f; 
	private AnimationNodeStateMachinePlayback  _StateMachine;
	
	public override void _Ready()
	{
		// Set specific values for this enemy type
		_Speed = 2;
		_Health = 100;
		_Damage = 10;

		_Skin = GetNode<SkeletonMinion>("%SkeletonMinion");
		_NavAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
		_AnimTree = GetNode<AnimationTree>("SkeletonMinion/AnimationTree");
		_StateMachine = (AnimationNodeStateMachinePlayback)_AnimTree.Get("parameters/playback");
		base._Ready();

		GD.Print("SkeletonMelee ready.");
	}
// https://www.youtube.com/watch?v=kBzV7vgdQfU
	public override void _PhysicsProcess(double delta)
	{
		string state = _StateMachine.GetCurrentNode();
		switch (state) {
			case "Idle":
				_AnimTree.Set("parameters/conditions/Run", true);
				break;
			case "Run":
				// Compute desired direction from navigation
				MakePath();
				Vector3 nextPathPosition = _NavAgent.GetNextPathPosition();
				Vector3 toTarget = nextPathPosition - GlobalPosition;
				velocity.X = toTarget.X;
				velocity.Z = toTarget.Z;
				if (!IsOnFloor()) // Check if the mob is in the air
				{
					velocity.Y += GetGravity().Y * (float)delta;
				}
				else
				{
					velocity.Y = 0;
				}
				Velocity = velocity.Normalized() * _Speed;
				float targetAngle = (-Vector3.Forward).SignedAngleTo(Velocity, Vector3.Up);
				Vector3 skinRotation = _Skin.GlobalRotation;
				skinRotation.Y = Mathf.LerpAngle(skinRotation.Y, targetAngle, (float)(RotationSpeed*delta));
				_Skin.GlobalRotation = skinRotation;
				hasAttacked = false;
				_AnimTree.Set("parameters/conditions/Attack", TargetInRange());
				MoveAndSlide(); 
				break;
			case "Attack":
				if (!hasAttacked)
				{
					Attack(_Player);
					velocity.X = 0;
					velocity.Y = 0;
					hasAttacked = true;
					_AnimTree.Set("parameters/conditions/Attack", TargetInRange());
				}
				break;
			case "Death":
				break;
			case "Hit":
				_AnimTree.Set("parameters/conditions/Hit", false);
				break;
			default:
				break;
		}
	}
}
