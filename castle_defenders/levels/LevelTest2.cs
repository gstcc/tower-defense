using Godot;
using System;

public partial class LevelTest2 : Game
{
	//[Export]  public Node3D SpawnPoint2;
	
	public override void _Ready()
	{
		_SpawnAmount = new([5]);
		base._Ready();
	}
	
	protected override void SpawnMobs()
	{
		for (int i = 0; i < _SpawnAmount[0]; i++)
		{
			SpawnMob(_meleeScene, SpawnPoint1);
		}
	}
}
