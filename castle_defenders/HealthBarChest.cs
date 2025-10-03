using Godot;
using System;

public partial class HealthBarChest : ProgressBar
{
	[Export] private Chest _Chest;
	
	public override void _Ready()
	{
		_Chest.Connect(Player.SignalName.HealthChanged, new Callable(this, nameof(Update)));
		Update();
	}
	
	private void Update()
	{
		Value = (_Chest._Health * 100) / _Chest._MaxHealth;
	}
}
