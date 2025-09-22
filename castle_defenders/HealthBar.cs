using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{
	[Export] private Player _Player;
	
	public override void _Ready()
	{
		_Player.Connect(Player.SignalName.HealthChanged, new Callable(this, nameof(Update)));
		Update();
	}
	
	private void Update()
	{
		Value = (_Player._Health * 100) / _Player._MaxHealth;
	}
}
