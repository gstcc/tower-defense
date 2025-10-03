using Godot;
using System;

public abstract partial class Rune : Node2D
{
	[Export] public float _healthModifier = 1.0f;
	[Export] public float _damageModifier = 1.0f;
	[Export] public float _magicProtectionModifier = 1.0f;
	[Export] public float _arrowProtectionModifier = 1.0f;
	[Export] public float _speedModifier = 1.0f;
	[Export] public Button button; 
	[Signal]
	public delegate void RuneClickedEventHandler(Rune rune);
	[Export] public int _Cost;
	
	public override void _Ready()
	{
		if (button != null)
		{
			button.Pressed += OnButtonPressed;
		}
	}
	
	protected void OnButtonPressed()
	{
		EmitSignal(SignalName.RuneClicked, this);
	}
	
	
	public virtual void Apply()
	{
		PlayerModifier.HealthModifier *= _healthModifier;
		PlayerModifier.DamageModifier *= _damageModifier;
		PlayerModifier.MagicProtectionModifier *= _magicProtectionModifier;
		PlayerModifier.ArrowProtectionModifier *= _arrowProtectionModifier;
		PlayerModifier.SpeedModifier *= _speedModifier;
	}
	
	public virtual void Remove()
	{
		PlayerModifier.HealthModifier /= _healthModifier;
		PlayerModifier.DamageModifier /= _damageModifier;
		PlayerModifier.MagicProtectionModifier /= _magicProtectionModifier;
		PlayerModifier.ArrowProtectionModifier /= _arrowProtectionModifier;
		PlayerModifier.SpeedModifier /= _speedModifier;
	}
}
