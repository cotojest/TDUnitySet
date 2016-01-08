using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TDSet;

namespace TDSet {
	public class TowerBuildingSpot : MonoBehaviour {
		public List<int> restrictedTowersIDs;
		[HideInInspector]
		public bool isFree = true;
	}
}
