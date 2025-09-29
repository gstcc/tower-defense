using Godot;
using System.Collections.Generic;

public partial class ShopMenu : CanvasLayer
{
	// Reference to the CanvasLayer that holds the shop UI

	// This will be used to hide or show the shop UI
	private bool isShopOpen = false;
	[Export] public Label _Coins;

	public override void _Ready()
	{
		// Make sure the CanvasLayer starts as invisible
		this.Visible = false;
		_Coins.Text = $"Coins: {CoinManager._TotalCoins}";
		GD.Print("Shop Menu Initialized. Shop initially hidden.");
	}

	// This is the method that should be called when the player interacts with the shop
	public void OnPlayerInteractsWithShop()
	{
		GD.Print("Player pressed E to interact with the shop.");
		UpdateCoinTotal();
		// Toggle the shop's visibility when the player presses "E"
		if (isShopOpen)
		{
			CloseShop();
		}
		else
		{
			ShowShop();
		}
	}
	
	public void UpdateCoinTotal()
	{
		_Coins.Text = $"Coins: {CoinManager._TotalCoins}";
	}

	// Show the shop's UI and make it visible
	private void ShowShop()
	{
		this.Visible = true;  // Make the CanvasLayer (shop UI) visible
		isShopOpen = true;

		GD.Print("Shop opened!");
		// Add any additional logic for updating the shop UI, e.g., displaying items
	}

	// Hide the shop's UI
	private void CloseShop()
	{
		this.Visible = false; // Make the CanvasLayer (shop UI) invisible
		isShopOpen = false;

		GD.Print("Shop closed!");
	}

	// Handle item purchasing (implementation pending)
	public void BuyItem(int itemIndex)
	{
		// Purchase logic here
		GD.Print($"Buying item with index {itemIndex}");
		UpdateCoinTotal();
	}
}
