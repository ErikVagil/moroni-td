using Godot;
using System;

public partial class GameManager : Node
{
	private int cityHealth = 100;
	private Label cityHealthLabel = null;

    public override void _Ready()
    {
		cityHealthLabel = GetNode<Label>("%CityHealthLabel");
    }

    private void UpdateCityHealthUI() {
		cityHealthLabel.Text = $"Health: {cityHealth}/100";
	}

	private void OnLamaniteAttack(int damageDealt) {
		cityHealth -= damageDealt;
		UpdateCityHealthUI();
	}
}
