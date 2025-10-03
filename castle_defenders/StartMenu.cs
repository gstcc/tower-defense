using Godot;
using System;

public partial class StartMenu : Node2D
{
	public void _OnButtonPressed() {
		GetTree().ChangeSceneToFile("res://levels/Level1.tscn");
	}
	
	public void _OnButtonQuitPressed() {
		GetTree().Quit();
	}
}
