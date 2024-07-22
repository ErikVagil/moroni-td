using Godot;
using System;

public partial class Lamanite : PathFollow2D
{
	[Export]
	private float moveSpeed = 2.5f;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		Progress += moveSpeed;
	}
}
