using Godot;
using System;

public partial class PlayerModifier : Node
{
	private static float _healthModifier = 1.0f;
	private static float _damageModifier = 1.0f;
	private static float _magicProtectionModifier = 1.0f;
	private static float _arrowProtectionModifier = 1.0f;
	private static float _speedModifier = 1.0f;

	// Signal (instance signal, not static)
	[Signal]
	public delegate void ModifiersChangedEventHandler();

	// Singleton instance (set this on _Ready)
	public static PlayerModifier Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	// Properties with notification on change
	public static float HealthModifier
	{
		get => _healthModifier;
		set
		{
			if (Math.Abs(_healthModifier - value) > 0.0001f)
			{
				_healthModifier = value;
				Instance?.EmitSignal(nameof(ModifiersChanged));
			}
		}
	}

	public static float DamageModifier
	{
		get => _damageModifier;
		set
		{
			if (Math.Abs(_damageModifier - value) > 0.0001f)
			{
				_damageModifier = value;
				Instance?.EmitSignal(nameof(ModifiersChanged));
			}
		}
	}

	public static float MagicProtectionModifier
	{
		get => _magicProtectionModifier;
		set
		{
			if (Math.Abs(_magicProtectionModifier - value) > 0.0001f)
			{
				_magicProtectionModifier = value;
				Instance?.EmitSignal(nameof(ModifiersChanged));
			}
		}
	}

	public static float ArrowProtectionModifier
	{
		get => _arrowProtectionModifier;
		set
		{
			if (Math.Abs(_arrowProtectionModifier - value) > 0.0001f)
			{
				_arrowProtectionModifier = value;
				Instance?.EmitSignal(nameof(ModifiersChanged));
			}
		}
	}

	public static float SpeedModifier
	{
		get => _speedModifier;
		set
		{
			if (Math.Abs(_speedModifier - value) > 0.0001f)
			{
				_speedModifier = value;
				Instance?.EmitSignal(nameof(ModifiersChanged));
			}
		}
	}
}
