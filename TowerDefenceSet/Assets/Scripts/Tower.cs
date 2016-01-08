using UnityEngine;
using System.Collections;
using TDSet;

namespace TDSet {
	public class Tower : MonoBehaviour {
		[HideInInspector]
		public int typeID;

		public string typeName;
		public string description;
		public Sprite icon;
		public float buildTime;
		public float range;

		public float cost;
		public Tower upgradedTower;

		public delegate void BuildStart(Tower tower);
		public static event BuildStart onBuildStart;

		public delegate void BuildEnd(Tower tower);
		public static event BuildEnd onBuildEnd;

		void Start () {
		}
		
		void Update () {
		
		}

		public void Build() {
			StartCoroutine (Building());
		}

		IEnumerator Building() {
			if (onBuildStart != null) {
				onBuildStart (this);
			}
			Debug.Log ("build start");
			yield return new WaitForSeconds (buildTime);
			if (onBuildEnd != null) {
				onBuildEnd (this);
			}
			Debug.Log ("build end");
		}
	}
}
