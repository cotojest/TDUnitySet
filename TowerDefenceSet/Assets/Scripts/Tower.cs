using UnityEngine;
using System.Collections;
using TDSet;

namespace TDSet {
	public class Tower : MonoBehaviour {
		[HideInInspector]
		public int typeID;
		[HideInInspector]
		public GameObject buildingIndicator;

		public string typeName;
		public string description;
		public Sprite icon;
		public float buildTime;
		public float range;
		public uint cost;
		[Range(0f,1f)]
		public float sellValueReturnRate;
		public Tower upgradedTower;

		public delegate void BuildStart(Tower tower);
		public static event BuildStart onBuildStart;

		public delegate void BuildEnd(Tower tower);
		public static event BuildEnd onBuildEnd;

		protected int previousLevelsCost = 0;

		void Start () {
		}
		
		void Update () {
		
		}

		public void Build() {
			StartCoroutine (Building());
		}

		public Tower Upgrade() {
			Tower newTower = (Tower)GameObject.Instantiate (upgradedTower, 
				transform.position, transform.rotation);
			newTower.previousLevelsCost = (int)cost + previousLevelsCost;
			newTower.Build ();
			Destroy (gameObject);
			return newTower;
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
			if (buildingIndicator != null) {
				Destroy (buildingIndicator);
			}
			StartTowerBehaviourCouroutine();
			Debug.Log ("build end");
		}

		protected virtual void StartTowerBehaviourCouroutine() {
		}

		public virtual int GetSellValue() {
			return (int)((cost + previousLevelsCost) * sellValueReturnRate);
		}
	}
}
