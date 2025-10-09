using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class Level1 : Game
{
	private List<int> _SpawnPerWave = new([1, 2, 5]);
	private bool _TutorialCompleted = false;
	
	
	public override void _Ready()
	{
		_SpawnAmount = new([8]);
		base._Ready();
	}
	
	protected override async Task SpawnMobsInWaves()
	{
		while (!_TutorialCompleted) {
			// Check every one second if tutorial is completed
			await ToSignal(GetTree().CreateTimer(1), "timeout");
		}
		for (int wave = 0; wave < TotalWaves; wave++)
			{
				GD.Print($"Starting Wave {wave + 1}");

				int meleePerWave = _SpawnPerWave[wave];

				for (int i = 0; i < meleePerWave; i++)
				{
					SpawnMob(_meleeScene, SpawnPoint1);
				}

				GD.Print($"Wave {wave + 1} complete. Waiting for next wave...");
				
				//Added new mobs, need to connect their signals aswell
				ConnectMobs();
				_Chest.ConnectMobs();
				if (wave < TotalWaves - 1)
				{
					await ToSignal(GetTree().CreateTimer(TimeBetweenWaves), "timeout");
				}
			}

			GD.Print("All waves spawned.");
	}
}
