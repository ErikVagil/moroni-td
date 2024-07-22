using Godot;
using System;

public partial class GameManager : Node2D
{
	public static readonly Godot.Collections.Array<PackedScene> enemyIDLookup = new Godot.Collections.Array<PackedScene> {
		ResourceLoader.Load<PackedScene>("res://Prefabs/Lamanite.tscn"),
	};
}
