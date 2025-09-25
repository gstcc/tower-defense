using Godot;
using System;
using System.Threading.Tasks;


public partial class SiegeBalista : Node3D
{
	public InteractionArea _InteractionArea; 
	public Projectile _Projectile;

	public override void _Ready()
	{
		_InteractionArea = GetNode<InteractionArea>("%InteractionArea");
		_Projectile = GetNode<Projectile>("%BallistaArrow");
		_InteractionArea.interact = OnInteract;
	}
	
	public async Task OnInteract() {
		GD.Print("Interacting");
		_Projectile.Fire();
	}
}
