using Godot;
using System;
using System.Collections.Generic;

public partial class Tower : Area2D
{
	private readonly Color highlightGood = new(1f, 1f, 1f, 0.75f);
	private readonly Color highlightBad = new(1f, 0.5f, 0.5f, 0.75f);
	private readonly Color defaultMod = new(1f, 1f, 1f, 1f);

	private bool isInBuildMode = true;
	private bool isColliding = false;
	private readonly HashSet<Area2D> collidingAreas = new();
	private Lamanite target = null;
	private readonly HashSet<Lamanite> possibleTargets = new();

	private Sprite2D sprite = null;
	private Area2D detectionArea = null;

    public override void _Ready() {
		sprite = GetNode<Sprite2D>("Sprite2D");
		detectionArea = GetNode<Area2D>("DetectionArea");
    }

    public override void _Process(double delta) {
		if (isInBuildMode) {
			// Highlight if in build mode
			if (isColliding) sprite.Modulate = highlightBad;
			else sprite.Modulate = highlightGood;

			// Follow the cursor
			Position = GetGlobalMousePosition();

			if (Input.IsActionJustPressed("build_accept") && !isColliding) {
				isInBuildMode = false;
			}
		} else {
			sprite.Modulate = defaultMod;
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
		if (isInBuildMode) {
			return;
		}
		if (!GameManager.IsAreaLamaniteChild(area)) {
			return;
		}
		Lamanite enteredLamanite = area.GetParent<Lamanite>();
		possibleTargets.Add(enteredLamanite);
		target = FindFurthestTarget();
	}

	private void OnDetectionAreaExited(Area2D area) {
		if (isInBuildMode) {
			return;
		}
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
}
