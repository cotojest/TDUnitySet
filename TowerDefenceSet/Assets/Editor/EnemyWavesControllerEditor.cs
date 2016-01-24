using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TDSet {
	[CustomEditor(typeof(EnemyWavesController))]
	public class EnemyWavesControllerEditor : Editor {
		EnemyWavesController myTarget;
		PathEditor pe;

		public override void OnInspectorGUI ()
		{
			pe = FindObjectOfType<PathEditor> ();
			myTarget = (EnemyWavesController)target;
			if (pe == null) {
				EditorGUILayout.HelpBox ("No PathEditor found on scene. Create one.", MessageType.Error);
				if (GUILayout.Button ("Add PathEditor")) {
					myTarget.gameObject.AddComponent<PathEditor> ();
				}
			} else {
				DrawDefaultInspector ();
				foreach (EnemyWave ew in myTarget.waves) {
					foreach (EnemySet es in ew.enemies) {
						if (es.pathId < 0) {
							es.pathId = 0;
						} else if (pe.paths != null && es.pathId > pe.paths.Count) {
							es.pathId = pe.paths.Count - 1;
						}
					}
				}
			}

		}

	}
}
