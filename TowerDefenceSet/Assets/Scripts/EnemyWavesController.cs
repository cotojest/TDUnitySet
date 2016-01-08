using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace TDSet {
	public class EnemyWavesController : MonoBehaviour {
		public List<EnemyWave> waves;

		public bool spawningEnded;
		private EnemyWave thisWave;

		public void Start() {
			RunNextWave ();
		}

		public void Update() {
		}

		public void RunNextWave() {
			if (waves.Count > 0) {
				thisWave = waves [0];
				spawningEnded = false;
				thisWave.Run (OnWaveSpawnEnd);
				InvokeRepeating ("SpawnNextEnemy", thisWave.timeBetweenSpawns, thisWave.timeBetweenSpawns);
				waves.RemoveAt (0);
			}
		}

		public void OnWaveSpawnEnd() {
			Debug.Log ("in");
			CancelInvoke ("SpawnNextEnemy");
			spawningEnded = true;
			Invoke ("RunNextWave", thisWave.timeToNextWave);
		}

		public void SpawnNextEnemy() {
			thisWave.SpawnEnemy ();
		}

	}

	[System.Serializable]
	public class EnemyWave {
		public List<EnemySet> enemies;
		public SpawnOrder spawnOrder;
		public float timeBetweenSpawns;
		public float timeToNextWave;
		public List<int> enemiesQueue;
		public event Action onSpawnFinish;

		public void Run(Action callback) {
			onSpawnFinish = callback;
			enemiesQueue = new List<int> ();
			PrepareEnemiesInOrder ();
			//InvokeRepeating ("SpawnEnemy", timeBetweenSpawns, timeBetweenSpawns);
		}
			
		public void SpawnEnemy() {
			if (enemiesQueue.Count <= 0) {
				if (onSpawnFinish != null) {
					onSpawnFinish ();
				}
			} else {
				Enemy spawnedEnemy = (Enemy)GameObject.Instantiate(enemies[enemiesQueue[0]].enemyPrefab);
				Path enemyPath = PathEditor.instance.GetPath (enemies [enemiesQueue [0]].pathId);
				spawnedEnemy.Init(enemyPath);
				String enemiesQueueString = "";
				foreach (int i in enemiesQueue) {
					enemiesQueueString += i.ToString() + ", ";
				}
				Debug.Log (enemiesQueueString);
				enemiesQueue.RemoveAt(0);
			}
		}

		private void PrepareEnemiesInOrder() {
			if (spawnOrder == SpawnOrder.AsDefined || spawnOrder == SpawnOrder.Random) {
				for (int es = 0; es < enemies.Count; es++) {
					for (int i = 0; i < enemies[es].count; i++) {
						enemiesQueue.Add (es);
					}
				}
			}
			if (spawnOrder == SpawnOrder.Random) {
				for (int i = 0; i < enemiesQueue.Count * 2; i++) {
					int randomIndex1 = UnityEngine.Random.Range(0, enemiesQueue.Count);
					int randomIndex2 = UnityEngine.Random.Range(0, enemiesQueue.Count);
					if (randomIndex1 != randomIndex2) {
						int temp = enemiesQueue [randomIndex1];
						enemiesQueue [randomIndex1] = enemiesQueue [randomIndex2];
						enemiesQueue [randomIndex2] = temp;
					}
				}
			}
			if (spawnOrder == SpawnOrder.IterateOverSets) {
				List<int> setCounts = new List<int>(enemies.Select(e => e.count));
				int biggestSetCount = setCounts.Max(i => i);
				for (int i = 0; i < biggestSetCount; i++) {
					for (int es = 0; es < enemies.Count; es++) {
						if (enemies[es].count >= i) {
							enemiesQueue.Add (es);
						}
					}
				}
			}
		}


	}

	[System.Serializable]
	public class EnemySet {
		public Enemy enemyPrefab;
		public int count;
		public int pathId;
	}

	public enum SpawnOrder {
		AsDefined, IterateOverSets, Random
	}
}
