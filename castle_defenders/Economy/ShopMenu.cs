using Godot;
using System.Collections.Generic;



public partial class ShopMenu : CanvasLayer
{
	private bool isShopOpen = false;
	[Export] public Label _Coins;
	private List<string> Rarities = ["Basic", "Rare", "Exotic"];
	private List<string> RuneTypes = ["Health", "Damage", "Arrow"];
	private List<ColorRect> BasicNodes = new List<ColorRect>();
	private List<ColorRect> RareNodes = new List<ColorRect>();
	private List<ColorRect> ExoticNodes = new List<ColorRect>();
	private Dictionary<ColorRect, Rune> runeMap = new Dictionary<ColorRect, Rune>();
	private List<Rune> Runes = new List<Rune>();

	public override void _Ready()
	{
		// Make sure the CanvasLayer starts as invisible
		this.Visible = false;
		_Coins.Text = $"Coins: {CoinManager._TotalCoins}";
		GD.Print("Shop Menu Initialized. Shop initially hidden.");
		AddShopWindows();
		AddRunes();
	}
	
	private void AddShopWindows()
	{
		foreach (string s in Rarities) 
		{
			for (int i = 1; i<=5; i++)
			{
				ColorRect node = GetNode<ColorRect>($"Panel/{s}{i}");
				switch (s)
				{
					case "Basic":
						BasicNodes.Add(node);
						break;
					case "Rare":
						RareNodes.Add(node);
						break;
					case "Exotic":
						ExoticNodes.Add(node);
						break;
					default:
						break;
				}
			}
		}
	}
	
	private void AddRunes()
	{
		//int runeCount = Mathf.Min(BasicNodes.Count, RareNodes.Count, ExoticNodes.Count, RuneTypes.Count);
		int runeCount = 2;
		for (int i = 0; i < runeCount; i++)
		{
			string runeType = RuneTypes[i];

			AddRuneToSlot(BasicNodes[i], $"res://Modifiers/BasicRunes/Basic{runeType}Rune.tscn");
			AddRuneToSlot(RareNodes[i], $"res://Modifiers/RareRunes/Rare{runeType}Rune.tscn");
			AddRuneToSlot(ExoticNodes[i], $"res://Modifiers/ExoticRunes/Exotic{runeType}Rune.tscn");
		}
	}
	
	private void AddRuneToSlot(ColorRect slot, string path)
	{
		if (ResourceLoader.Exists(path))
		{
			PackedScene runeScene = GD.Load<PackedScene>(path);
			if (runeScene != null)
			{
				Rune runeInstance = runeScene.Instantiate<Rune>();
				Runes.Add(runeInstance);
				runeInstance.Connect(Rune.SignalName.RuneClicked, new Callable(this, nameof(RunePressed)));
				slot.AddChild(runeInstance);
			}
			else
			{
				GD.PrintErr($"Failed to instantiate scene: {path}");
			}
		}
		else
		{
			GD.Print($"Rune scene not found at path: {path} (Skipping)");
		}
	}
	
	private void RunePressed(Rune rune)
	{
		GD.Print($"Rune clicked: {rune.Name}");

		// Prevent adding duplicates
		if (InventoryManager.OwnRune(rune))
		{
			GD.Print("Player already owns this rune.");
			return;
		}

		// Remove from shop UI
		rune.GetParent()?.RemoveChild(rune);
		
		var callable = new Callable(this, nameof(RunePressed));
		if (rune.IsConnected(Rune.SignalName.RuneClicked, callable))
		{
			rune.Disconnect(Rune.SignalName.RuneClicked, callable);
			GD.Print("Disconnected RuneClicked signal from ShopMenu.");
		}

		// Add to InventoryManager
		if (InventoryManager.AddRune(rune))
		{
			GD.Print($"Rune added to inventory: {rune.Name}");
		}
		else
		{
			GD.Print("Failed to add rune to inventory.");
		}
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
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
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
