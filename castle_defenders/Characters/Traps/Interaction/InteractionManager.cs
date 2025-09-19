using Godot;
using System;
using System.Collections.Generic; // Needed for List<>

public partial class InteractionManager : Node3D
{
	protected Player _Player;
	protected Label3D _Label;
	protected const string _BaseText = "[E] to";
	protected bool _CanInteract = true;
	protected List<InteractionArea> _ActiveAreas = new();
	private bool _OneTimeInteract = false;
	
	public void SetOneTimeInteract(bool v) {
		_OneTimeInteract = v;
	}

	public override void _Ready()
	{
		_Player = GetNode<Player>("/root/Main/Player");
		_Label = GetNode<Label3D>("%Label3D");
	}
	
	public async override void _Input(InputEvent ev) 
	{
		if (ev.IsActionPressed("interact") && _ActiveAreas.Count > 0 && _CanInteract) {
			_CanInteract=false;
			_Label.Visible = false;
			await _ActiveAreas[0].interact();
			//Some things can only be interacted with once, e.g. explose barrels.
			_CanInteract= (!_OneTimeInteract);
		}
	}	
	
	public override void _Process(double delta)
	{
		if (_ActiveAreas.Count > 0 && _CanInteract)
		{
			// Sort by distance to player
			_ActiveAreas.Sort((a, b) =>
			{
				float distA = _Player.GlobalPosition.DistanceTo(a.GlobalPosition);
				float distB = _Player.GlobalPosition.DistanceTo(b.GlobalPosition);
				return distA.CompareTo(distB);
			});

			_Label.Visible = true;
			InteractionArea closest = _ActiveAreas[0];
			_Label.Text = $"{_BaseText} {closest._ActionName}";
			_Label.GlobalPosition = closest.GlobalPosition + new Vector3(0, 2, 0);
		}
		else
		{
			_Label.Visible = false;
		}
	}

	public void RegisterArea(InteractionArea area)
	{
		if (!_ActiveAreas.Contains(area))
			_ActiveAreas.Add(area);
	}

	public void UnregisterArea(InteractionArea area)
	{
		_ActiveAreas.Remove(area);
	}
}
