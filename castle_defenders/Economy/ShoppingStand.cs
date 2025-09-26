using Godot;
using System;
using System.Threading.Tasks;

public partial class ShoppingStand : Node3D
{
	public InteractionArea _InteractionArea; 
	[Export]
	public InteractionManager _interactionManager;

	public override void _Ready()
	{
		_InteractionArea = GetNode<InteractionArea>("%InteractionArea");
		_InteractionArea._interactionManager = _interactionManager;
		_InteractionArea.interact = OnInteract;
	}
	
	public async Task OnInteract() {
		GD.Print("Shopping");
	}
}
