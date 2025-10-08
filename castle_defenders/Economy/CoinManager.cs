using Godot;
using System;

public partial class CoinManager : Node
{
	// Make the _TotalCoins field static
	public static int _TotalCoins = 0;
	public static int _CoinsBeforeStart = 0;
	[Signal]
	public delegate void CoinsChangedEventHandler();
	
	public static CoinManager Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public static void AddCoin()
	{
		_TotalCoins += 1;
		Instance?.EmitSignal(nameof(CoinsChanged));
	}
	
	public static void ResetCoinCounterAfterDeath()
	{
		_TotalCoins = _CoinsBeforeStart;
	}
	
	public static void NextLevelStarted()
	{
		_CoinsBeforeStart = _TotalCoins;
	}

	public static int GetCoinCount()
	{
		return _TotalCoins;
	}
	
	public static bool RemoveCoins(int withdraw)
	{
		if (_TotalCoins >= withdraw) {
			_TotalCoins -= withdraw;
			Instance?.EmitSignal(nameof(CoinsChanged));
			return true;	
		}
		return false;
	}
}
