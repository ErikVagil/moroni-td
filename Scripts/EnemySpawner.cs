using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	private int[] enemyIDQueue = null;
	private int queueIndex = 0;

	private Timer spawnerTimer = null;
	private Path2D enemyPath = null;

	public override void _Ready() {
		spawnerTimer = GetNode<Timer>("./SpawnerTimer");
		enemyPath = GetNode<Path2D>("%EnemyPath");
		// Test code
		StartWave(enemyIDQueue);
	}

	public void StartWave(int[] waveEnemyQueue) {
		SetEnemyIDQueue(waveEnemyQueue);
		spawnerTimer.Start();
	}

	private void SetEnemyIDQueue(int[] enemyIDs) {
		enemyIDQueue = enemyIDs;
		queueIndex = 0;
	}

	public void SpawnEnemy() {
		// Using the enemy ID, find the scene to instantiate
		PackedScene enemyToSpawn = GameManager.enemyIDLookup[enemyIDQueue[queueIndex]];
		queueIndex++;
		GD.Print($"Queue Index: {queueIndex}");
		
		// Instantiate the enemy and put it on the path
		var enemyInstance = enemyToSpawn.Instantiate();
		enemyPath.AddChild(enemyInstance);
	}

	private void OnSpawnerTimerTimeout() {
		// Kill the timer if there's no more enemies to spawn
		if (queueIndex >= enemyIDQueue.Length) {
			return;
		}

		// If there's an enemy in the queue, spawn it and start the cooldown timer
		SpawnEnemy();
		spawnerTimer.Start();
	}
}
