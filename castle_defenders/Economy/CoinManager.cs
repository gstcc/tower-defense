using Godot;
using System;

public partial class CoinManager : Node
{
	// Make the _TotalCoins field static
	public static int _TotalCoins = 0;

	// Optionally, add a static method to manipulate coins
	public static void AddCoin()
	{
		_TotalCoins += 1;
	}

	// You can also create methods to get the coin count if needed
	public static int GetCoinCount()
	{
		return _TotalCoins;
	}
	
	public static bool RemoveCoins(int withdraw)
	{
		// We can only remove coins as long as it doesn't go into negative
		if (_TotalCoins >= withdraw) {
			_TotalCoins -= withdraw;
			return true;	
		}
		return false;
	}
}
