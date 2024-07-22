using Godot;
using System;

public partial class GameManager : Node2D
{
	// Array containing all of the loaded enemy scenes
	public static readonly Godot.Collections.Array<PackedScene> ENEMY_SCENES = new Godot.Collections.Array<PackedScene> {
		ResourceLoader.Load<PackedScene>("res://Prefabs/Lamanite.tscn"),
	};

	private const int maxCityHealth = 50;
	private int cityHealth = 50;

	public void updateHealth(int changeInHealth) {
		int newHealth = cityHealth + changeInHealth;
		if (newHealth <= maxCityHealth && newHealth >= 0) {
			cityHealth = newHealth;
		}

		GD.Print($"City health: {cityHealth}");
	}
}
