using Godot;
using System;
using System.Collections.Generic;

public partial class Tower : Area2D
{
	private bool isInBuildMode = true;
	private bool isColliding = false;
	private readonly HashSet<Area2D> collidingAreas = new();
	private Color highlightGood = new(1f, 1f, 1f, 0.75f);
	private Color highlightBad = new(1f, 0.5f, 0.5f, 0.75f);
	private Color defaultMod = new(1f, 1f, 1f, 1f);

    public override void _Process(double delta) {
		if (isInBuildMode) {
			// Highlight if in build mode
			if (isColliding) Modulate = highlightBad;
			else Modulate = highlightGood;

			// Follow the cursor
			Position = GetGlobalMousePosition();

			if (Input.IsActionJustPressed("build_accept") && !isColliding) {
				isInBuildMode = false;
			}
		} else {
			Modulate = defaultMod;
		}
	}

	private void OnAreaEntered(Area2D area) {
		collidingAreas.Add(area);
		isColliding = true;
	}

	private void OnAreaExited(Area2D area) {
		collidingAreas.Remove(area);
		if (collidingAreas.Count == 0) {
			isColliding = false;
		}
	}
}
