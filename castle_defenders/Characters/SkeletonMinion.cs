using Godot;
using System;

public partial class SkeletonMinion : Node3D
{
	private AnimationPlayer _Animation;
	
	public override void _Ready()
	{
		_Animation = GetNode<AnimationPlayer>("%AnimationPlayer");
	}
	
	public void Idle() {
		_Animation.Play("Idle");
	}
	
	public void Move() {
		_Animation.Play("Walking_A");
	}
}
