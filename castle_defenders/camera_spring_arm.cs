using Godot;
using System;

public partial class camera_spring_arm : SpringArm3D
{
	public const double mouse_sensitivity = 0.005;
	
	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	
	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventMouseMotion motion) {
			float Y = (float) (Rotation.Y - motion.Relative.X * mouse_sensitivity);
			float X = (float) (Rotation.X - motion.Relative.Y * mouse_sensitivity);
			Rotation = new Vector3(X, Y, Rotation.Z);
		}
	}
}
