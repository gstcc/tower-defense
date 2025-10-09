using Godot;
using System;

public partial class FailedLevel : Node2D
{
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
	
	public void _OnButtonPressed() {
		GetTree().ChangeSceneToFile(GameModifier.getLevel());
	}
}
