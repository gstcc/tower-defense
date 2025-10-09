using Godot;
using System;

public partial class TutorialText : RichTextLabel
{
	private int _currentStep = 0;
	private Timer _tutorialTimer;
	[Export] public Level1 _Level;

	public override void _Ready()
	{
		Clear();
		_tutorialTimer = new Timer();
		AddChild(_tutorialTimer); 
		_tutorialTimer.OneShot = true;
		_tutorialTimer.Connect("timeout", new Callable(this, nameof(OnTutorialTimeout)));

		ShowMovementInstructions();
	}

	private void ShowMovementInstructions()
	{
		Clear();
		AddText("Use the WASD keys to move around:\n");
		AddText("- W: Move forward\n");
		AddText("- A: Move left\n");
		AddText("- S: Move backward\n");
		AddText("- D: Move right\n\n");

		_tutorialTimer.Start(10);
	}

	private void ShowLookAroundInstructions()
	{
		Clear();
		AddText("Use the mouse to look around:\n");
		AddText("- Move the mouse to change your viewpoint.\n");
		AddText("- The mouse controls the camera rotation.\n\n");
		_tutorialTimer.Start(10);
	}

	private void ShowBlockInstructions()
	{
		Clear();
		AddText("Right-click to block attacks:\n");
		AddText("- Right-click to raise your shield or block incoming attacks.\n\n");
		_tutorialTimer.Start(10); 
	}

	private void ShowAttackInstructions()
	{
		Clear();
		AddText("Left-click to attack:\n");
		AddText("- Left-click to perform a basic attack.\n");
	}

	// Called when the timer runs out (timeout signal)
	private void OnTutorialTimeout()
	{
		_currentStep++;

		switch (_currentStep)
		{
			case 1:
				ShowLookAroundInstructions();
				break;
			case 2:
				ShowBlockInstructions();
				break;
			case 3:
				ShowAttackInstructions();
				break;
			default:
				// All tutorial steps are complete
				GD.Print("Tutorial Complete!");
				_Level._TutorialCompleted = true;
				Clear();
				break;
		}
	}

	// Check player actions to trigger tutorial progression
	public override void _Process(double delta)
	{
		if (_currentStep == 0 && Input.IsActionJustPressed("move_up")) // WASD 'W' key
		{
			OnTutorialTimeout();
		}

		if (_currentStep == 3 && Input.IsMouseButtonPressed(MouseButton.Left)) // Left-click attack
		{
			OnTutorialTimeout();
		}

		if (_currentStep == 2 && Input.IsMouseButtonPressed(MouseButton.Right)) // Right-click block
		{
			OnTutorialTimeout();
		}
	}
}
