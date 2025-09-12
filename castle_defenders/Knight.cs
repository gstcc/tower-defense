using Godot;
using System;

public partial class Knight : Node3D
{
	private AnimationPlayer _Animation;
	
	
	public override void _Ready()
	{
		_Animation = GetNode<AnimationPlayer>("%AnimationPlayer");
	}
	
	public void Attack() {
		_Animation.Play("1H_Melee_Attack_Slice_Diagonal");
	}
	
	public void Jump()
	{
		_Animation.Play("Jump_Start");
	}
	
	public void Fall() {
		_Animation.Play("Jump_Idle");
	}

	public void Idle() {
		_Animation.Play("Idle");
	}
	
	public void Move() {
		_Animation.Play("Walking_A");
	}
}
