using Godot;
using System;

public partial class PlayerModifier : Node
{
	private static float _healthModifier = 1.0f;
	private static float _damageModifier = 1.0f;
	private static float _magicProtectionModifier = 1.0f;
	private static float _arrowProtectionModifier = 1.0f;
	private static float _speedModifier = 1.0f;

	// Property with getter and setter for each modifier
	public static float HealthModifier
	{
		get => _healthModifier;
		set => _healthModifier = value;
	}

	public static float DamageModifier
	{
		get => _damageModifier;
		set => _damageModifier = value;
	}

	public static float MagicProtectionModifier
	{
		get => _magicProtectionModifier;
		set => _magicProtectionModifier = value;
	}

	public static float ArrowProtectionModifier
	{
		get => _arrowProtectionModifier;
		set => _arrowProtectionModifier = value;
	}

	public static float SpeedModifier
	{
		get => _speedModifier;
		set => _speedModifier = value;
	}
}
