using Godot;
using System;
using System.Collections.Generic; // Needed for List<>


enum GameState: ushort
{
	LOST = 0,
	IN_PROGRESS = 1,
	WON = 2
}

public partial class Game : Node
{
	[Export] private Node3D _Enemies;
	[Export] private Chest _Chest;
	[Export] private Player _Player;
	private GameState _State = GameState.IN_PROGRESS;
	private List<BaseMob> _AliveEnemies = new();
	private Node3D SpawnPoint1;
	private Node3D SpawnPoint2;
	private PackedScene _axeScene = (PackedScene)GD.Load("res://Characters/SkeletonAxe.tscn");
	private PackedScene _meleeScene = (PackedScene)GD.Load("res://Characters/SkeletonMelee.tscn");
	private List<int> _SpawnAmount = new([5, 5]);
		
	
	public override void _Ready()
	{
		SpawnPoint1 = GetNode<Node3D>("SpawnPoint1");
		SpawnPoint2 = GetNode<Node3D>("SpawnPoint2");
		SpawnMobs(2);
		foreach (Node child in _Enemies.GetChildren())
		{
			// Since each mob takes time to despawn we can't 
			// check if _Enemies has no children
			if (child is BaseMob mob)
			{
				_AliveEnemies.Add(mob);
				mob.Connect(BaseMob.SignalName.Died, new Callable(this, nameof(OnMobDied)));
			}
		}
		_Player.Connect(Player.SignalName.Died, new Callable(this, nameof(OnPlayerDied)));	
	}
	
	private void SpawnMobs(int spawnPointCount)
	{
		

		// Store spawn points in a list
		List<Node3D> spawnPoints = new() { SpawnPoint1, SpawnPoint2 };

		// Spawn Axe Mobs
		for (int i = 0; i < _SpawnAmount[0]; i++)
		{
			SpawnMob(_axeScene, spawnPoints[0]);
		}

		// Spawn Melee Mobs
		for (int i = 0; i < _SpawnAmount[1]; i++)
		{
			SpawnMob(_meleeScene, spawnPoints[1]);
		}
	}
	
	private void SpawnMob(PackedScene mobScene, Node3D spawnPoint)
{
	var mobInstance = mobScene.Instantiate();
	if (mobInstance is BaseMob mob)
	{
		// Generate random offset in range [-5, 5] for X and Z
		float randomOffsetX = (float)(GD.Randf() * 10.0 - 5.0);
		float randomOffsetZ = (float)(GD.Randf() * 10.0 - 5.0);

		// Get spawn point's global transform and modify translation
		Transform3D spawnTransform = spawnPoint.GlobalTransform;

		// Apply random offset to X and Z of the translation
		Vector3 newPosition = spawnTransform.Origin;
		newPosition.X += randomOffsetX;
		newPosition.Z += randomOffsetZ;

		// Set the new position back to the transform
		spawnTransform.Origin = newPosition;

		// Assign the new global transform to the mob
		mob.GlobalTransform = spawnTransform;

		// Reset velocity
		mob.Velocity = Vector3.Zero;

		_Enemies.AddChild(mob);
	}
}


		
	private void OnPlayerDied()
	{
		GD.Print("Player died AAAAAAAAAAAAAA");
		_State = GameState.LOST;
	}
	
	private void OnMobDied(BaseMob mob)
	{
		GD.Print("Mob died");
		_AliveEnemies.Remove(mob);
		if (_AliveEnemies.Count <= 0)
		{
			_State = GameState.WON;
			GD.Print("Player won");
		}
	}
}
