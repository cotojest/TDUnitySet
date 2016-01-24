using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathEditor : MonoBehaviour {
	public List<Path> paths;
	public static PathEditor instance { get; private set;}
	void Awake() {
		instance = this;
	}

	public Path GetPath(int pathId) {
		if (pathId < paths.Count) {
			return paths [pathId];
		}
		return null;
	}

	private Color[] colors = {Color.red, Color.blue, Color.green, Color.magenta, Color.white, Color.cyan, Color.yellow, Color.white};
	void OnDrawGizmos() {
		if (paths != null) {
			for (int j = 0; j < paths.Count; j++) {
				Gizmos.color = colors [j % colors.Length];
				for (int i = 0; i < paths [j].waypoints.Count; i++) {
					if (i < paths [j].waypoints.Count - 1) {
						Gizmos.DrawLine (paths [j].waypoints [i].transform.position, paths [j].waypoints [i + 1].transform.position);
						Vector3 lineVector = (paths [j].waypoints [i].transform.position - paths [j].waypoints [i + 1].transform.position).normalized;
						Quaternion rotation1 = Quaternion.Euler (0f, 30f, 0f);
						Quaternion rotation2 = Quaternion.Euler (0f, 330f, 0f);

						Gizmos.DrawLine (paths [j].waypoints [i + 1].transform.position, paths [j].waypoints [i + 1].transform.position +
							(rotation1 * lineVector));
						Gizmos.DrawLine (paths [j].waypoints [i + 1].transform.position, paths [j].waypoints [i + 1].transform.position +
							(rotation2 * lineVector));
					}
				}

			}
		}
	}

}

[System.Serializable]
public class Path {
	public List<Transform> waypoints;
}
