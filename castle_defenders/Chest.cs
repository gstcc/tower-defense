using Godot;
using System;
using System.Collections.Generic;

public partial class Chest : StaticBody3D
{
	[Export] public Node3D _Enemies;
	[Signal]
	public delegate void HealthChangedEventHandler(Node target);
	[Signal]
	public delegate void DiedEventHandler(Node target);
	public int _Health = 100;
	public int _MaxHealth = 100;
	private HashSet<BaseMob> _ConnectedMobs = new();
	
	public override void _Ready()
	{
		 CallDeferred(nameof(ConnectMobs));
		
	}
	
	public void ConnectMobs()
	{
		foreach (Node child in _Enemies.GetChildren())
		{
			// Connect all mobs and player
			if (child is BaseMob mob && !_ConnectedMobs.Contains(mob))
			{
				mob.Connect(BaseMob.SignalName.AttackedChest, new Callable(this, nameof(OnEnemyAttacked)));
				_ConnectedMobs.Add(mob);
			}
		}
	}
	
	private void OnEnemyAttacked(int damage)
	{
		_Health-=damage;
		EmitSignal(SignalName.HealthChanged);
		if (_Health <= 0)
		{
			Die();
		}
	}
	
	private void Die()
	{
		EmitSignal(SignalName.Died);
	}
}
