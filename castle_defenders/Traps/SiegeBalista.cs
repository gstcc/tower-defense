using Godot;
using System;
using System.Threading.Tasks;

public partial class SiegeBalista : Node3D
{
	public InteractionArea _InteractionArea; 
	public Projectile _Projectile;
	[Export]
	public InteractionManager _interactionManager;

	public override void _Ready()
	{
		_InteractionArea = GetNode<InteractionArea>("%InteractionArea");
		_InteractionArea._interactionManager = _interactionManager;
		_Projectile = GetNode<Projectile>("%BallistaArrow");
		_InteractionArea.interact = OnInteract;
	}
	
	public async Task OnInteract() {
		GD.Print("Interacting");
		_Projectile.Fire();
	}
}
