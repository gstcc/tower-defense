using Godot;
using System;

public partial class Knight : Node3D
{
	private AnimationPlayer _Animation;
	
	
	public override void _Ready()
	{
		_Animation = GetNode<AnimationPlayer>("%AnimationPlayer");
	}
}
