using Godot;
using System;

public partial class GameModifier : Node
{
	private static int levelCount = 1;
	private static string levelLink= $"res://levels/Level{levelCount}.tscn";
	
	public static void changeLevel(Node caller, int num)
	{
		levelCount += num;

		if (levelCount > 4)
		{
			caller.GetTree().ChangeSceneToFile("res://GameCompleted.tscn");
		}
	}
	
	public static string getLevel(){
		levelLink= $"res://levels/Level{levelCount}.tscn";
		GD.Print(levelLink);
		GD.Print("levelcount:");
		GD.Print(levelCount);
		return levelLink;
	}
}
