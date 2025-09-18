using Godot;
using System;

public partial class MiniMapCamera : Camera3D
{
	[Export] private Node3D _Target;
	private Vector3 _Offset;
	
	public override void _Ready()
	{
		if (_Target == null) {
			GD.PrintErr("No target for minimap");
		}
		_Offset = GlobalPosition - _Target.GlobalPosition;
	}
	
	public override void _Process(double delta) 
	{
		GlobalPosition = _Target.GlobalPosition + _Offset;
	}
}
