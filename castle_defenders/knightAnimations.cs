using Godot;
using System;

public partial class knightAnimations : AnimationPlayer
{
	public override void _Process(double delta)
	{
		// Movement input flags
		bool moving = false;

		if (Input.IsActionPressed("ui_right"))
		{
			Play("Running_Strafe_Right");
			moving = true;
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			Play("Running_Strafe_Left");
			moving = true;
		}
		else if (Input.IsActionPressed("ui_up"))
		{
			Play("Walking_A");
			moving = true;
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			Play("Walking_Backwards");
			moving = true;
		}

		// If no movement key is pressed, go to idle animation
		if (!moving) {
			Play("Unarmed_Idle");
		}
	}
}
