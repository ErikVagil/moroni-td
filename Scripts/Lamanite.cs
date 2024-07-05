using Godot;
using System;

public partial class Lamanite : CharacterBody2D
{
	[Export]
	private Node2D target = null;
	[Export]
	private float moveSpeed = 60f;

    public override void _Process(double delta)
    {
        Vector2 moveDirection = (target.Position - Position).Normalized() * moveSpeed * (float)delta;
		MoveAndCollide(moveDirection);
    }
}
