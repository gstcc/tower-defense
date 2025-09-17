using Godot;
using System;
using System.Threading.Tasks;


public partial class SiegeBalista : Node3D
{
	public InteractionArea _InteractionArea; 
	public Area3D _Projectile;

	public override void _Ready()
	{
		_InteractionArea = GetNode<InteractionArea>("%InteractionArea");
		_Projectile = GetNode<Area3D>("%BallistaArrow");
		_InteractionArea.interact = OnInteract;
	}
	
	public async Task OnInteract() {
		GD.Print("Interacting");
	}
}
