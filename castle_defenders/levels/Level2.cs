using Godot;
using System;
using System.Threading.Tasks;

public partial class Level2 : Game
{
	[Export]  public Node3D SpawnPoint2;
	
	
	public override void _Ready()
	{
		_SpawnAmount = new([12, 15]);
		base._Ready();
	}
	
	protected override async Task SpawnMobsInWaves()
	{
		for (int wave = 0; wave < TotalWaves; wave++)
			{
				GD.Print($"Starting Wave {wave + 1}");

				// Divide mobs across waves (simple even split)
				int axePerWave = _SpawnAmount[0] / TotalWaves;
				int meleePerWave = _SpawnAmount[1] / TotalWaves;

				// You can randomize or fine-tune this distribution as needed.

				for (int i = 0; i < axePerWave; i++)
				{
					SpawnMob(_axeScene, SpawnPoint1);
				}

				for (int i = 0; i < meleePerWave; i++)
				{
					SpawnMob(_meleeScene, SpawnPoint2);
				}

				GD.Print($"Wave {wave + 1} complete. Waiting for next wave...");
				
				//Added new mobs, need to connect their signals aswell
				ConnectMobs();
				_Chest.ConnectMobs();
				_Player.ConnectMobs();
				if (wave < TotalWaves - 1)
				{
					await ToSignal(GetTree().CreateTimer(TimeBetweenWaves), "timeout");
				}
			}

			GD.Print("All waves spawned.");
	}
}
