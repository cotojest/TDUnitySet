using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TDSet;
using System.Linq;

namespace TDSet {
	public class TowerBuildingSpot : MonoBehaviour {
		public List<int> restrictedTowersIDs;
		[HideInInspector]
		public bool isFree = true;


		void OnDrawGizmosSelected() {
			if (BuildController.instance != null && BuildController.instance.towerTypes != null) {
				List<Tower> myTowers = new List<Tower> (BuildController.instance.towerTypes);
				foreach (int i in restrictedTowersIDs) {
					myTowers.RemoveAt (i);
				}
				if (myTowers.Count > 0) {
					float maxRange = myTowers.Max (t => t.range);
					Gizmos.color = Color.green;
					Gizmos.DrawWireSphere (transform.position, maxRange);
					float minRange = myTowers.Min (t => t.range);
					Gizmos.color = Color.red;
					Gizmos.DrawWireSphere (transform.position, minRange);
				}
			}
		}
	}
}
