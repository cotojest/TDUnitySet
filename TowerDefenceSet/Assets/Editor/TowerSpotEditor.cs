using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TDSet {
	[CustomEditor(typeof(TowerBuildingSpot))]
	public class TowerSpotEditor : Editor {
		TowerBuildingSpot myTarget;
		BuildController bc;

		public override void OnInspectorGUI ()
		{
			bc = FindObjectOfType<BuildController> ();
			myTarget = (TowerBuildingSpot)target;
			if (bc == null) {
				EditorGUILayout.HelpBox ("No BuildController found on scene. Create one.", MessageType.Error);
				if (GUILayout.Button ("Create BuildController")) {
					new GameObject ().AddComponent<BuildController> ();
				}
			} else {
				DrawDefaultInspector ();
				for (int i = 0; i < myTarget.restrictedTowersIDs.Count; i++) {
					if (myTarget.restrictedTowersIDs[i] < 0) {
						myTarget.restrictedTowersIDs[i] = 0;
					} else if (bc.towerTypes != null && myTarget.restrictedTowersIDs[i] > bc.towerTypes.Count) {
						myTarget.restrictedTowersIDs[i] = bc.towerTypes.Count - 1;
					}
				}
			}
		}
	}
}
