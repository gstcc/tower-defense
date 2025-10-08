using Godot;
using System;

public partial class SkeletonChestRunner : BaseMob
{
	private Node3D _Skin;

	private Vector3 velocity;	
	private const float RotationSpeed = 12.0f; 
	private AnimationNodeStateMachinePlayback  _StateMachine;
	[Export] public NavigationAgent3D _Agent;
	
	public override void _Ready()
	{
		// Set specific values for this enemy type
		_Speed = 3;
		_Health = 30;
		_MaxHealth = 30;
		_Damage = 5;

		_Skin = GetNode<Node3D>("%Rig");
		_NavAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
		_AnimTree = GetNode<AnimationTree>("%AnimationTree");
		_StateMachine = (AnimationNodeStateMachinePlayback)_AnimTree.Get("parameters/playback");
		//_Agent.AvoidancePriotity = (float)(GD.Randf());
		_Agent.AvoidancePriority = (float)(GD.Randf() * 0.999f + 0.001f);
		GD.Print(_Agent.AvoidancePriority);
		base._Ready();
	}
	
	protected override bool CanAttack() {
		return _StateMachine.GetCurrentNode() != "Hit";
	}
	
	public virtual void MakePath()
	{
		// Can only attack chest
		_NavAgent.TargetPosition = chest.GlobalPosition;
	}
	
	public void VeloctityComputedCallback(Vector3 safeVelocity)
	{
		Velocity = safeVelocity;
	}

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
				//Vector3 toTarget = nextPathPosition - GlobalPosition;
				Vector3 toTarget = GlobalPosition.DirectionTo(nextPathPosition) * _Speed;
				velocity.X = toTarget.X;
				velocity.Z = toTarget.Z;
				if (!IsOnFloor()) // Check if the mob is in the air
				{
					velocity += GetGravity() * (float)delta;
				}
				else
				{
					velocity.Y = toTarget.Y;
				}
				//Velocity = velocity.Normalized() * _Speed;
				if (_NavAgent.AvoidanceEnabled) {
					_NavAgent.SetVelocity(velocity);	
				} else {
					VeloctityComputedCallback(velocity);
				}
				float targetAngle = (-Vector3.Forward).SignedAngleTo(Velocity, Vector3.Up);
				Vector3 skinRotation = _Skin.GlobalRotation;
				skinRotation.Y = Mathf.LerpAngle(skinRotation.Y, targetAngle, (float)(RotationSpeed*delta));
				_Skin.GlobalRotation = skinRotation;
				hasAttacked = false;
				_AnimTree.Set("parameters/conditions/Attack", TargetInRange());
				_AnimTree.Set("parameters/conditions/Hit", false);
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
