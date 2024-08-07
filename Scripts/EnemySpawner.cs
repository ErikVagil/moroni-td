using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	private int[] enemyIDQueue = null;
	private int queueIndex = 0;

	private Timer spawnerTimer = null;
	private Path2D enemyPath = null;
	private GameManager gameManager = null;

	public override void _Ready() {
		spawnerTimer = GetNode<Timer>("./SpawnerTimer");
		enemyPath = GetNode<Path2D>("%EnemyPath");
		gameManager = GetNode<GameManager>("%GameManager");
	}

    public override void _Process(double delta) {
        if ((enemyIDQueue == null || queueIndex >= enemyIDQueue.Length) &&
			enemyPath.GetChildCount() == 0) {
			gameManager.SetCanSendWave(true);
			if (gameManager.GetWaveCount() < gameManager.GetMaxWaveCount()) {
				gameManager.ShowStartWaveButton();
			}
		}
    }

    public void StartWave(int[] enemyIDQueue) {
		this.enemyIDQueue = enemyIDQueue;
		queueIndex = 0;
		spawnerTimer.Start();
	}

	public void SpawnEnemy() {
		// Using the enemy ID, find the scene to instantiate
		int nextEnemyID = enemyIDQueue[queueIndex];
		PackedScene enemyToSpawn = GameManager.ENEMY_SCENES[nextEnemyID];
		queueIndex++;
		
		// Instantiate the enemy and put it on the path
		var enemyInstance = enemyToSpawn.Instantiate();
		enemyPath.AddChild(enemyInstance);
	}

	private void OnSpawnerTimerTimeout() {
		if (queueIndex < enemyIDQueue.Length) {
			SpawnEnemy();
			spawnerTimer.Start();
		}
	}

	public int GetNumEnemiesLeftInWave() {
		return enemyIDQueue.Length - queueIndex;
	}
}
