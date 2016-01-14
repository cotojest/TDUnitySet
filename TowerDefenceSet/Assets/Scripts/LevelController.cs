using UnityEngine;
using System.Collections;
using TDSet;
using UnityEngine.Events;

namespace TDSet {
	public class LevelController : MonoBehaviour {
		[SerializeField]
		private uint startingPlayerLifePoints;
		[SerializeField]
		private uint startingResources;

		public uint resources {
			get {
				return _resources;
			}

			private set {
				_resources = value;
				onResourcesChange.Invoke ();
			}
		}
		private uint _resources;

		public int playerLifePoints {
			get {
				return _playerLifePoints;
			}

			private set {
				_playerLifePoints = value;
				onPlayerLifePointsChange.Invoke ();
			}
		}
		private int _playerLifePoints;

		public UnityEvent onResourcesChange;
		public UnityEvent onPlayerLifePointsChange;

		public static LevelController instance {
			get {
				if (_instance == null) {
					_instance = FindObjectOfType<LevelController> ();
					Debug.Log (_instance);
				}

				return _instance;

			}
		}
		private static LevelController _instance;

		void Start() {
			resources = startingResources;
			playerLifePoints = (int)startingPlayerLifePoints;
		}

		public uint GetResources() {
			return resources;
		}

		public void AddResources(uint value) {
			resources += value;
		}

		public bool SubtractResources(uint value) {
			if (ResourcesAreSufficient (value)) {
				resources -= value;
				return true;
			} else {
				return false;
			}
		}
			
		public bool ResourcesAreSufficient (uint price) {
			if (price > resources) {
				return false;
			}
			return true;
		}

		public int GetLifePoints() {
			return playerLifePoints;
		}

		public void DamagePlayer(int damage) {
			playerLifePoints -= damage;
			if (playerLifePoints < 0) {
				playerLifePoints = 0;
			}
		}
			
	}
}