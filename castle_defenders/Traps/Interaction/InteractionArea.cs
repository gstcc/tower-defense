using Godot;
using System;
using System.Threading.Tasks;


public partial class InteractionArea : Area3D
{
	[Export]
	public string _ActionName = "interact";
	private InteractionManager _interactionManager;

	public override void _Ready()
	{
		_interactionManager = GetNode<InteractionManager>("/root/Main/InteractionManager");
	}
	
	public Func<Task> interact;
	
	public void _OnBodyEntered(Node3D body) {
		_interactionManager.RegisterArea(this);
	}
	
	public void _OnBodyExited(Node3D body) {
		_interactionManager.UnregisterArea(this);
		
	}
	
}
