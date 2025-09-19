using Godot;
using System;
using Godot.Collections;


public partial class Explosion : Node3D
{
	private GpuParticles3D  _Debris;
	private GpuParticles3D  _Smoke;
	private GpuParticles3D _Fire;
	private AudioStreamPlayer3D _ExplosionSound;

	public override void _Ready()
	{
		_Debris = GetNode<GpuParticles3D>("Debris");
		_Smoke = GetNode<GpuParticles3D>("Smoke");
		_Fire = GetNode<GpuParticles3D>("Fire");
		_ExplosionSound = GetNode<AudioStreamPlayer3D>("ExplosionSound");
	}

	public async void Explode()
	{
		_Debris.Emitting = true;
		_Smoke.Emitting = true;
		_Fire.Emitting = true;
		_ExplosionSound.Play();

		await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);
		QueueFree();
	}
}
