using Godot;
using System;

public partial class SuccededLevel : Node2D
{
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
	
	public void _OnTryAgainPressed() {
		GetTree().ChangeSceneToFile(GameModifier.getLevel());
	}
	
	public void _OnNextLevelPressed() {
		GameModifier.changeLevel(this, 1);
		GetTree().ChangeSceneToFile(GameModifier.getLevel());
	}
}
