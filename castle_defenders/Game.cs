using Godot;
using System;
using System.Collections.Generic; // Needed for List<>
using System.Threading.Tasks;
using System.Linq;



enum GameState: ushort
{
	LOST = 0,
	IN_PROGRESS = 1,
	WON = 2
}

public partial class Game : Node
{
	[Export] private Node3D _Enemies;
	[Export] protected Chest _Chest;
	[Export] protected Player _Player;
	[Export] public Label _Coins;
	private GameState _State = GameState.IN_PROGRESS;
	private List<BaseMob> _AliveEnemies = new();
	[Export] public Node3D SpawnPoint1;
	protected PackedScene _axeScene = (PackedScene)GD.Load("res://Characters/SkeletonAxe.tscn");
	protected PackedScene _meleeScene = (PackedScene)GD.Load("res://Characters/SkeletonMelee.tscn");
	protected PackedScene _chestMobScene = (PackedScene)GD.Load("res://Characters/SkeletonChestRunner.tscn");
	protected List<int> _SpawnAmount;
	[Export] public int TotalWaves = 3;
	[Export] public float TimeBetweenWaves = 3.0f;
	private int _NrOfMobs;
	private HashSet<BaseMob> _ConnectedMobs = new();
		
	
	public override void _Ready()
	{
		//SpawnMobs();
		SpawnMobsInWaves();
		_NrOfMobs = _SpawnAmount.Sum();
		_Player.Connect(Player.SignalName.Died, new Callable(this, nameof(OnPlayerDied)));	
		_Chest.Connect(Chest.SignalName.Died, new Callable(this, nameof(OnChestDied)));	
		CoinManager.Instance.Connect(nameof(CoinManager.CoinsChanged), new Callable(this, nameof(UpdateCoinTotal)));
	}
	
	protected void ConnectMobs()
	{
		foreach (Node child in _Enemies.GetChildren())
		{
			// Since each mob takes time to despawn we can't 
			// check if _Enemies has no children
			if (child is BaseMob mob && !_ConnectedMobs.Contains(mob))
			{
				_AliveEnemies.Add(mob);
				_ConnectedMobs.Add(mob);
				mob.Connect(BaseMob.SignalName.Died, new Callable(this, nameof(OnMobDied)));
			}
		}
	}
	
	protected virtual void SpawnMobs()
	{
		// Spawn Axe Mobs
		for (int i = 0; i < _SpawnAmount[0]; i++)
		{
			SpawnMob(_axeScene, SpawnPoint1);
		}
		
	}
	
	protected virtual async Task SpawnMobsInWaves()
	{
		
	}
	
	private void UpdateCoinTotal()
	{
		_Coins.Text = $"{CoinManager._TotalCoins}";
	}
	
	protected void SpawnMob(PackedScene mobScene, Node3D spawnPoint)
	{
	var mobInstance = mobScene.Instantiate();
	if (mobInstance is BaseMob mob)
	{
		mob._Player = _Player;
		mob.chest = _Chest;
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
		_State = GameState.LOST;
		gameFailed();
	}
	
	private void OnChestDied()
	{
		_State = GameState.LOST;
		gameFailed();
	}
	
	private void OnMobDied(BaseMob mob)
	{
		GD.Print("Mob died");
		_AliveEnemies.Remove(mob);
		--_NrOfMobs;
		if (_AliveEnemies.Count <= 0 && _NrOfMobs <= 0)
		{
			_State = GameState.WON;
			GD.Print("Player won");
			gamesucceded();
		}
		//UpdateCoinTotal();
	}
	
	private void gameFailed(){
		GD.Print("level failed");
		GetTree().ChangeSceneToFile("res://FailedLevel.tscn");
	}
	
	private void gamesucceded(){
		GD.Print("level succeded");
		GetTree().ChangeSceneToFile("res://SuccededLevel.tscn");
	}
}
