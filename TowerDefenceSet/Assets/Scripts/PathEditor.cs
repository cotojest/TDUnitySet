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

}

[System.Serializable]
public class Path {
	public List<Transform> waypoints;
}
