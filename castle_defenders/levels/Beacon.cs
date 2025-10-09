using Godot;
using System;

public partial class Beacon : Node3D
{
	[Export] public MeshInstance3D _Mesh;
	[Export] public SpotLight3D _Light;

	private float _bounceHeight = 0.05f; // How high the mesh will bounce
	private float _sum = 0.0f;
	
	public override void _Process(double delta)
	{
		// Animate the mesh bouncing up and down
		float bounceOffset = Mathf.Sin(_sum*2) * _bounceHeight ;
		_Mesh.Position = new Vector3(_Mesh.Position.X, _Mesh.Position.Y+bounceOffset, _Mesh.Position.Z);
		_sum += (float) delta;
		//2pi;
		if (_sum >= 3.1415)
		{
			_sum = 0;
		}
	}
}
