using Godot;
using System;
using System.Collections.Generic;


public partial class InventoryManager : Node
{
	public static List<Rune> _OwnedRunes = new List<Rune>();
	public static Rune _ActiveRune1;
	public static Rune _ActiveRune2;
	
	public static bool AddRune(Rune rune)
	{
		if (_OwnedRunes.Contains(rune) || rune == null)
		{
			return false;
		}
		Inventory.Instance.ConnectToRune(rune);
		_OwnedRunes.Add(rune);
		return true;
	}
	
	public static bool OwnRune(Rune rune)
	{
		return _OwnedRunes.Contains(rune);
	}
	
	public static void RuneClicked(Rune rune)
	{
		GD.Print("Rune clicked");
		if (rune == _ActiveRune1)
		{
			_ActiveRune1.Remove();
			_ActiveRune1 = null;
		} else if (rune == _ActiveRune2)
		{
			_ActiveRune2.Remove();
			_ActiveRune2 = null;
		} else {
			if (_ActiveRune1 == null) {
				_ActiveRune1 = rune;
				_ActiveRune1.Apply();
			} else if (_ActiveRune2 == null) {
				_ActiveRune2 = rune;
				_ActiveRune2.Apply();
			} else {
				GD.Print("Can't have more than two runes active at the same time");
			}
		}
		Inventory.Instance.DrawShop();
	}
}
