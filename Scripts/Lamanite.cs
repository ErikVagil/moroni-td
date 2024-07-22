using Godot;
using System;

public partial class Lamanite : PathFollow2D
{
	[Export]
	private float moveSpeed = 2.5f;
	private int attackDamage = 1;

	public override void _Process(double delta) {
		Progress += moveSpeed;
	}

	public int getAttackDamage() {
		return attackDamage;
	}
}
