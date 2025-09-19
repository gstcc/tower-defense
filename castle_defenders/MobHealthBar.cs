using Godot;
using System;

public partial class MobHealthBar : ProgressBar
{
	[Export] private BaseMob _Mob;
	
	public override void _Ready()
	{
		_Mob.Connect(Player.SignalName.HealthChanged, new Callable(this, nameof(Update)));
		Update();
	}
	
	private void Update()
	{
		Value = (_Mob._Health * 100) / _Mob._MaxHealth;
	}
}
