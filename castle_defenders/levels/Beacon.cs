using Godot;
using System;

public partial class Beacon : Node3D
{
	[Export] public MeshInstance3D _Mesh;
	[Export] public SpotLight3D _Light;

	private float _bounceHeight = 1.0f; // How high the mesh will bounce
	private float _bounceSpeed = 2.0f;  // Speed of the bounce
	private float _pulseSpeed = 2.0f;   // Speed of the light pulse
	private float _pulseIntensity = 5.0f; // Maximum intensity of the light pulse
	private float _sum;
	
	public override void _Process(double delta)
	{
		// Animate the mesh bouncing up and down
		float bounceOffset = Mathf.Sin(_sum) * _bounceHeight ;
		_Mesh.Position = new Vector3(_Mesh.Position.X, bounceOffset, _Mesh.Position.Z);
		_sum += (float) delta;
		//2pi;
		if (_sum >= 6.28)
		{
			_sum = 0;
		}
	}
}
