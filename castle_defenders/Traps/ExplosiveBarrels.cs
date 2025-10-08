using Godot;
using System;
using System.Threading.Tasks;

public partial class ExplosiveBarrels : Node3D
{
	public InteractionArea _InteractionArea; 
	public Explosion _Explosion;
	public DamageArea _DamageArea;
	private const int _Damage = 100;
	private Sprite3D _Sprite;
	[Export]
	public InteractionManager _interactionManager;

	public override void _Ready()
	{
		_InteractionArea = GetNode<InteractionArea>("%InteractionArea");
		_InteractionArea._interactionManager = _interactionManager;
		_Explosion = GetNode<Explosion>("%Explosion");
		_DamageArea = GetNode<DamageArea>("%DamageArea");
		_Sprite = GetNode<Sprite3D>("Sprite3D");
		_InteractionArea.interact = OnInteract;
	}
	
	public void RemoveBarrels()
	{
		for (int i = 2; i <= 6; i++)
		{
			string barrelName = "barrel" + i;
			Node barrel = GetNodeOrNull(barrelName);
			if (barrel != null)
			{
				barrel.QueueFree();
			}
		}
	}
	
	public async Task OnInteract() {
		// 3 Seconds before barrels explode so player can run away
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		_Explosion.Explode();
		RemoveBarrels();
		_Sprite.QueueFree();
		// The ExplosiveBarrels are using Player and Mobs layer
		// So all overlapping bodies should be able to take damage
		_DamageArea.CheckOverlappingBodies(_Damage);
	}
}
