using Godot;
using System;

public partial class GameManager : Node
{
	// Array containing all of the loaded enemy scenes
	public static readonly PackedScene[] ENEMY_SCENES = {
		ResourceLoader.Load<PackedScene>("res://Prefabs/Lamanite.tscn"),
	};

	private PackedScene towerResource = ResourceLoader.Load<PackedScene>("res://Prefabs/Tower.tscn");

	private const int maxCityHealth = 20;
	private int cityHealth = maxCityHealth;
	private int goldCount = 90;
	private int waveCount = 0;
	private const int maxWaveCount = 5;
	private bool canSendWave = true;

	private Label healthLabel = null;
	private Label goldLabel = null;
	private Label waveLabel = null;
	private PanelContainer shopContainer = null;
	private Button startWaveButton = null;
	private EnemySpawner enemySpawner = null;
	private Path2D enemyPath = null;

    public override void _Ready() {
		healthLabel = GetNode<Label>("%HealthLabel");
		goldLabel = GetNode<Label>("%GoldLabel");
		waveLabel = GetNode<Label>("%WaveLabel");
		shopContainer = GetNode<PanelContainer>("%ShopContainer");
		startWaveButton = GetNode<Button>("%StartWaveButton");
		enemySpawner = GetNode<EnemySpawner>("%EnemySpawner");
		enemyPath = GetNode<Path2D>("%EnemyPath");

		RefreshHealthLabel();
		RefreshGoldLabel();
		RefreshWaveLabel();
    }

    public override void _Process(double delta) {
		// Listen for open shop inputs
        if (Input.IsActionJustPressed("toggle_shop_ui")) {
			if (shopContainer.Visible) {
				shopContainer.Hide();
			} else {
				shopContainer.Show();
			}
		}

		// Check for level completion
		if (waveCount == maxWaveCount && 
			enemyPath.GetChildCount() == 0 && 
			enemySpawner.GetNumEnemiesLeftInWave() == 0) {
			GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");
		}
    }

    public void UpdateHealth(int changeInHealth) {
		int newHealth = cityHealth + changeInHealth;
		cityHealth = Math.Clamp(newHealth, 0, maxCityHealth);
		RefreshHealthLabel();

		if (cityHealth == 0) {
			TriggerGameOver();
		}
	}

	private void RefreshHealthLabel() {
		healthLabel.Text = $"Health: {cityHealth}/{maxCityHealth}";
	}

	public void UpdateGold(int changeInGold) {
		goldCount += changeInGold;
		RefreshGoldLabel();
	}

	private void RefreshGoldLabel() {
		goldLabel.Text = $"Gold: {goldCount}";
	}

	public void SendWave() {
		waveCount++;
		RefreshWaveLabel();
		canSendWave = false;
		int[] enemyQueue = new int[0];
		switch (waveCount) {
			case 1:
				enemyQueue = new[] {0, 0, 0, 0, 0};
				break;
			case 2:
				enemyQueue = new[] {0, 0, 0, 0, 0, 
									0, 0};
				break;
			case 3:
				enemyQueue = new[] {0, 0, 0, 0, 0, 
									0, 0, 0, 0, 0};
				break;
			case 4:
				enemyQueue = new[] {0, 0, 0, 0, 0, 
									0, 0, 0, 0, 0,
									0, 0, 0, 0, 0};
				break;
			case 5:
				enemyQueue = new[] {0, 0, 0, 0, 0, 
									0, 0, 0, 0, 0,
									0, 0, 0, 0, 0,
									0, 0, 0, 0, 0};
				break;
			default:
				break;
		}
		enemySpawner.StartWave(enemyQueue);
	}

	public void SetCanSendWave(bool canSendWave) {
		this.canSendWave = canSendWave;
	}

	public int GetWaveCount() {
		return waveCount;
	}

	public int GetMaxWaveCount() {
		return maxWaveCount;
	}

	public void ShowStartWaveButton() {
		startWaveButton.Show();
	}

	private void RefreshWaveLabel() {
		waveLabel.Text = $"Wave: {waveCount}/{maxWaveCount}";
	}

	private void TriggerGameOver() {
		GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");
	}

	public static bool IsAreaLamaniteChild(Area2D area) {
		return area.GetParent().GetType() == typeof(Lamanite);
	}

	private void OnStartWaveButtonPressed() {
		startWaveButton.Hide();
		SendWave();
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
