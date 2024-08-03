using Godot;
using System;

public partial class GameManager : Node
{
	// Array containing all of the loaded enemy scenes
	public static readonly PackedScene[] ENEMY_SCENES = {
		ResourceLoader.Load<PackedScene>("res://Prefabs/Lamanite.tscn"),
	};

	private PackedScene towerResource = ResourceLoader.Load<PackedScene>("res://Prefabs/Tower.tscn");

	private const int maxCityHealth = 50;
	private int cityHealth = 50;
	private int goldCount = 90;

	private Label healthLabel = null;
	private Label goldLabel = null;
	private PanelContainer shopContainer = null;

    public override void _Ready() {
		healthLabel = GetNode<Label>("%HealthLabel");
		goldLabel = GetNode<Label>("%GoldLabel");
		shopContainer = GetNode<PanelContainer>("%ShopContainer");
    }

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("toggle_shop_ui")) {
			if (shopContainer.Visible) {
				shopContainer.Hide();
			} else {
				shopContainer.Show();
			}
		}
    }

    public void UpdateHealth(int changeInHealth) {
		int newHealth = cityHealth + changeInHealth;
		if (newHealth <= maxCityHealth && newHealth >= 0) {
			cityHealth = newHealth;
		}

		healthLabel.Text = $"Health: {cityHealth}/{maxCityHealth}";
	}

	public void UpdateGold(int changeInGold) {
		goldCount += changeInGold;
		goldLabel.Text = $"Gold: {goldCount}";
	}

	public static bool IsAreaLamaniteChild(Area2D area) {
		return area.GetParent().GetType() == typeof(Lamanite);
	}

	private void OnOpenShopButtonPressed() {
		shopContainer.Show();
	}

	private void OnCloseShopButtonPressed() {
		shopContainer.Hide();
	}

	private void OnBuyTowerButtonPressed() {
		if (goldCount < 50) {
			return;
		}
		UpdateGold(-50);
		shopContainer.Hide();
		var tower = towerResource.Instantiate<Tower>();
		GetParent().AddChild(tower);
	}
}
