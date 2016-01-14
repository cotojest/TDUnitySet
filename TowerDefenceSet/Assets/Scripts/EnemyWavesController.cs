using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace TDSet {
	public class EnemyWavesController : MonoBehaviour {
		public enum State {WaitingForStart, SpawningEnemies, WaitingForNextWave, Ended}
		public State state {
			get {
				return _state;
			} 
			private set {
				if (onSpawningEndedChange != null) {
					onSpawningEndedChange (value != State.SpawningEnemies);
				}
				_state = value;
			}
		}
		private State _state;

		public List<EnemyWave> waves;

		public event Action<bool> onSpawningEndedChange;
		private EnemyWave thisWave;
		public static EnemyWavesController instance { get; private set;}
		public float timer;

		void Awake() {
			instance = this;
			state = State.WaitingForStart;
		}
		void Update() {
			if (state != State.WaitingForStart && state != State.Ended) {
				timer += Time.deltaTime;
				if (state == State.SpawningEnemies) {
					if (timer >= thisWave.timeBetweenSpawns) {
						SpawnNextEnemy ();
					}
				} else if (state == State.WaitingForNextWave) {
					if (timer >= thisWave.timeToNextWave) {
						RunNextWave ();
					}
				}
			}
		}

		public int GetSecondsToNextWave() {
			if (state == State.WaitingForNextWave) {
				return (int)(thisWave.timeToNextWave - timer);
			} else {
				return -1;
			}
		}
		public void RunNextWave() {
			if (waves.Count > 0) {
				thisWave = waves [0];
				state = State.SpawningEnemies;
				thisWave.Run (OnWaveSpawnEnd);
				waves.RemoveAt (0);
			} else {
				state = State.Ended;
			}
		}

		private void OnWaveSpawnEnd() {
			state = State.WaitingForNextWave;
			timer = 0;

		}

		private void SpawnNextEnemy() {
			timer = 0;
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
