using Godot;
using System;

public partial class Lamanite : CharacterBody2D
{
	[Export]
	private float moveSpeed = 60f;

	[Export]
	private int attackDamage = 1;

	private Node2D target = null;
	private bool isCollidingWithTarget = false;
	private bool isReadyToAttack = true;
	private Timer cooldownTimer = null;

	[Signal]
	public delegate void AttackEventHandler(int damageDealt);

    public override void _Ready()
    {
        target = GetNode<CharacterBody2D>("%City");
		cooldownTimer = GetNode<Timer>("./CooldownTimer");
    }

    public override void _Process(double delta)
    {
		// Move
        Vector2 moveDirection = (target.Position - Position).Normalized() * moveSpeed * (float)delta;
		KinematicCollision2D collision = MoveAndCollide(moveDirection);
		isCollidingWithTarget = collision != null && 
							  (collision.GetCollider() as Node2D) == target;

		// Attack
		if (isCollidingWithTarget && isReadyToAttack) {
			isReadyToAttack = false;
			cooldownTimer.Start();
			EmitSignal(SignalName.Attack, attackDamage);
		}
    }

	private void OnTimerTimeout() {
		isReadyToAttack = true;
	}
}
