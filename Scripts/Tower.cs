using Godot;
using System;
using System.Collections.Generic;

public partial class Tower : Area2D
{
	private readonly Color highlightGood = new(1f, 1f, 1f, 0.75f);
	private readonly Color highlightBad = new(1f, 0.5f, 0.5f, 0.75f);
	private readonly Color defaultMod = new(1f, 1f, 1f, 1f);
	private readonly Color transparent = new(1f, 1f, 1f, 0.5f);

	private bool isInBuildMode = true;
	private bool isColliding = false;
	private readonly HashSet<Area2D> collidingAreas = new();
	private Lamanite target = null;
	private readonly HashSet<Lamanite> possibleTargets = new();

	private Sprite2D sprite = null;
	private Area2D detectionArea = null;
	private Sprite2D radiusSprite = null;
	private Timer cooldownTimer = null;
	private Path2D projectilePath = null;

	private PackedScene arrowResource = ResourceLoader.Load<PackedScene>("res://Prefabs/Arrow.tscn");

    public override void _Ready() {
		sprite = GetNode<Sprite2D>("./Sprite2D");
		detectionArea = GetNode<Area2D>("./DetectionArea");
		radiusSprite = GetNode<Sprite2D>("./DetectionArea/Sprite2D");
		cooldownTimer = GetNode<Timer>("./CooldownTimer");
		projectilePath = GetNode<Path2D>("./ProjectilePath");
    }

    public override void _Process(double delta) {
		if (isInBuildMode) {
			// Highlight if in build mode
			if (isColliding) {
				sprite.Modulate = highlightBad;
				radiusSprite.Modulate = highlightBad;
			} else {
				sprite.Modulate = highlightGood;
				radiusSprite.Modulate = transparent;
			}

			// Follow the cursor
			Position = GetGlobalMousePosition();

			if (Input.IsActionJustPressed("build_accept") && !isColliding) {
				isInBuildMode = false;
				sprite.Modulate = defaultMod;
				radiusSprite.Hide();
				cooldownTimer.Start();
			}
		} else {
			if (target != null) {
				// Set projectile path to go from tower to target
				Curve2D path = new();
				path.AddPoint(Vector2.Zero);
				path.AddPoint(target.Position - Position);
				projectilePath.Curve = path;
			}
		}
	}

	private void OnAreaEntered(Area2D area) {
		if (!isInBuildMode) {
			return;
		}
		if (IsDetectionArea(area)) {
			return;
		}
		collidingAreas.Add(area);
		isColliding = true;
	}

	private void OnAreaExited(Area2D area) {
		if (!isInBuildMode) {
			return;
		}
		if (IsDetectionArea(area)) {
			return;
		}
		collidingAreas.Remove(area);
		if (collidingAreas.Count == 0) {
			isColliding = false;
		}
	}

	private void OnDetectionAreaEntered(Area2D area) {
		if (!GameManager.IsAreaLamaniteChild(area)) {
			return;
		}
		Lamanite enteredLamanite = area.GetParent<Lamanite>();
		possibleTargets.Add(enteredLamanite);
		target = FindFurthestTarget();
	}

	private void OnDetectionAreaExited(Area2D area) {
		if (!GameManager.IsAreaLamaniteChild(area)) {
			return;
		}
		Lamanite exitedLamanite = area.GetParent<Lamanite>();
		possibleTargets.Remove(exitedLamanite);
		if (exitedLamanite == target) {
			target = FindFurthestTarget();
		}
	}

	private Lamanite FindFurthestTarget() {
		Lamanite newTarget = null;
		foreach (Lamanite possibleTarget in possibleTargets) {
			if (newTarget == null) {
				newTarget = possibleTarget;
				continue;
			}
			
			if (possibleTarget.Progress > newTarget.Progress) {
				newTarget = possibleTarget;
			}
		}
		return newTarget;
	}

	private bool IsDetectionArea(Area2D area) {
		return area == detectionArea;
	}

	private void AttackTarget() {
		if (target == null) {
			return;
		}
		var arrow = arrowResource.Instantiate<Arrow>();
		arrow.SetTarget(target);
		projectilePath.AddChild(arrow);
	}

	private void OnCooldownTimerTimeout() {
		AttackTarget();
		cooldownTimer.Start();
	}
}
