using Godot;
using System;

public partial class Inventory : CanvasLayer
{
	private bool _IsShopOpen = false;
	 public static Inventory Instance;

	public override void _Input(InputEvent ev)
	{
		if (ev.IsActionPressed("Inventory"))
		{
			ToggleInventory();
		}
	}

	private void ToggleInventory()
	{
		_IsShopOpen = !_IsShopOpen;
		Visible = _IsShopOpen;
		Input.MouseMode = _IsShopOpen ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
		DrawShop();
		GD.Print(_IsShopOpen ? "Inventory opened!" : "Inventory closed!");
	}
	
	public void ConnectToRune(Rune rune)
	{
		rune.Connect(Rune.SignalName.RuneClicked, new Callable(this, nameof(OnRuneClicked)));
	}
	
	private void OnRuneClicked(Rune rune)
	{
		InventoryManager.RuneClicked(rune);
	}
	
	public void DrawShop()
	{
		if (InventoryManager._ActiveRune1 != null) {
			var activeRune1Node = GetNode<ColorRect>("%ActiveRune1");
			var activeRune1 = InventoryManager._ActiveRune1;
			if (activeRune1.GetParent() != null)
				activeRune1.GetParent().RemoveChild(activeRune1);
			activeRune1Node.AddChild(activeRune1);
		}
		if (InventoryManager._ActiveRune2 != null) {
			var activeRune2Node = GetNode<ColorRect>("%ActiveRune2");
			var activeRune2 = InventoryManager._ActiveRune2;
			if (activeRune2.GetParent() != null)
				activeRune2.GetParent().RemoveChild(activeRune2);
			activeRune2Node.AddChild(activeRune2);
		}

		var panel = GetNode<Control>("Panel");

		int runeCount = InventoryManager._OwnedRunes.Count;
		int maxSlots = 15;

		int addedRunes = 0; // Count how many runes we've actually added to slots

		for (int i = 0; i < runeCount && addedRunes < maxSlots; i++)
		{
			var rune = InventoryManager._OwnedRunes[i];

			// Skip runes if they are active
			if (rune == InventoryManager._ActiveRune1 || rune == InventoryManager._ActiveRune2)
				continue;

			string slotName = $"Slot{addedRunes + 1}";

			if (panel.HasNode(slotName))
			{
				var slot = panel.GetNode<Control>(slotName);

				// Remove any existing children before adding new rune
				foreach (var child in slot.GetChildren())
					slot.RemoveChild(child);

				// Remove rune from its old parent, if any
				if (rune.GetParent() != null)
					rune.GetParent().RemoveChild(rune);

				// Add rune to the slot
				slot.AddChild(rune);

				addedRunes++;
			}
			else
			{
				GD.PrintErr($"Missing slot node: {slotName}");
			}
		}
	}


	public override void _Ready()
	{
		// Optional: start hidden
		Visible = false;
		Instance = this;
	}
}
