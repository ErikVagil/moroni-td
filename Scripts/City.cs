using Godot;
using System;

public partial class City : Area2D
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	private void OnAreaEntered(Area2D area) {
		if (area.GetParent().GetType() != typeof(Lamanite)) {
			return;
		}
		
		Lamanite enteredEnemy = area.GetParent<Lamanite>();
		GD.Print("1 damage dealt");
		enteredEnemy.QueueFree();
	}
}
