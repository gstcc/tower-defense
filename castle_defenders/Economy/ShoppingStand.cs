using Godot;
using System;
using System.Threading.Tasks;

public partial class ShoppingStand : Node3D
{
	public InteractionArea _InteractionArea; 
	[Export]
	public InteractionManager _interactionManager;
	[Export] public ShopMenu _ShopMenu;

	public override void _Ready()
	{
		_InteractionArea = GetNode<InteractionArea>("%InteractionArea");
		_InteractionArea._interactionManager = _interactionManager;
		_InteractionArea.interact = OnInteract;
		_ShopMenu.Visible = false;
	}
	
	public async Task OnInteract() {
		_ShopMenu.OnPlayerInteractsWithShop();
	}
}
