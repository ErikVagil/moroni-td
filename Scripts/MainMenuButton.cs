using Godot;
using System;

public partial class MainMenuButton : Button
{
	public void OnPressed() {
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}
}
