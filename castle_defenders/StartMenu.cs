using Godot;
using System;

public partial class StartMenu : Node2D
{
	public void _OnButtonPressed() {
		GD.Print("button pressed");
		GetTree().ChangeSceneToFile("res://main.tscn");
	}
}
