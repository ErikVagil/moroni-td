using Godot;
using System;

public partial class Lamanite : PathFollow2D
{
	[Export]
	protected float moveSpeed = 2.5f;
	protected int attackDamage = 1;
	protected int goldDrop = 5;
	protected int health = 2;

	private GameManager gameManager;

    public override void _Ready() {
		gameManager = GetNode<GameManager>("%GameManager");
    }

    public override void _Process(double delta) {
		// Move along the path
		Progress += moveSpeed;
	}

	public int GetAttackDamage() {
		return attackDamage;
	}

	public int GetGoldDrop() {
		return goldDrop;
	}

	public int GetHealth() {
		return health;
	}

	public void UpdateHealth(int changeInHealth) {
		int newHealth = health + changeInHealth;
		if (newHealth <= 0) {
			KillEnemy();
		}
		health = newHealth;
	}

	private void KillEnemy() {
		gameManager.UpdateGold(goldDrop);
		QueueFree();
	}
}
