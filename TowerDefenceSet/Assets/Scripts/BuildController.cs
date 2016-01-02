using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TDSet;

namespace TDSet {
	public class BuildController : MonoBehaviour {
		public List<Tower> towerTypes;
		private Tower towerPreview;

		private TowerBuildingSpot selectedSpot;

		public static BuildController instance { get; private set;}
		void Awake() {
			instance = this;
		}

		void Start () {
			int id = 0;
			foreach (Tower t in towerTypes) {
				t.typeID = id;
				id++;
			}
		}
		
		void Update () {
		
		}

		public Tower SelectTowerOnScreenPosition(Vector3 position) {
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			if( Physics.Raycast(ray, out hit) )
			{
				Tower selectedTower = hit.transform.gameObject.GetComponent<Tower>();
				return selectedTower;
			} else {
				return null;
			}
		}

		public TowerBuildingSpot SelectBuildingSpotOnScreenPosition(Vector3 position) {
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Debug.Log (hit.transform.gameObject);
				selectedSpot = hit.transform.gameObject.GetComponent<TowerBuildingSpot> ();
				DestroyTowerPreview ();
				return selectedSpot;
			} else {
				return null;
			}
		}

		public void SetTowerPreview(int towerID) {
			DestroyTowerPreview ();
			towerPreview = (Tower)GameObject.Instantiate (towerTypes [towerID]);
			Debug.Log (selectedSpot.gameObject);
			Debug.Log (towerPreview.gameObject);
			towerPreview.gameObject.transform.position = selectedSpot.gameObject.transform.position;
			towerPreview.gameObject.transform.localScale = selectedSpot.gameObject.transform.localScale;
		}

		private void DestroyTowerPreview() {
			if (towerPreview != null) {
				DestroyImmediate (towerPreview.gameObject);
			}
		}

	}
}

