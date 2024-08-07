using Godot;
using System;

public partial class City : Area2D
{
	private GameManager gameManager = null;
	private AnimationPlayer animationPlayer = null;

	public override void _Ready() {
		gameManager = GetNode<GameManager>("%GameManager");
		animationPlayer = GetNode<AnimationPlayer>("./AnimationPlayer");
	}

	private void OnAreaEntered(Area2D area) {
		if (!GameManager.IsAreaLamaniteChild(area)) {
			return;
		}
		Lamanite enteredEnemy = area.GetParent<Lamanite>();

		gameManager.UpdateHealth(-enteredEnemy.GetAttackDamage());

		enteredEnemy.KillEnemy();
		animationPlayer.Play(name: "attacked");
	}
}
