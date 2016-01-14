using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TDSet;

namespace TDSet {
	public class BuildController : MonoBehaviour {
		public List<Tower> towerTypes;
		public GameObject rangeIndicator;
		private Tower towerPreview;

		private TowerBuildingSpot selectedSpot;
		private Tower selectedTower;

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
			HideRangeIndicator ();
		}
		
		void Update () {
		
		}

		public Tower SelectTowerOnScreenPosition(Vector3 position) {
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			if( Physics.Raycast(ray, out hit) )
			{
				selectedTower = hit.transform.gameObject.GetComponent<Tower>();
				if (selectedTower != null) {
					ShowRangeIndicator (selectedTower);
				}
				return selectedTower;
			} else {
				return null;
			}
		}

		public TowerBuildingSpot SelectBuildingSpotOnScreenPosition(Vector3 position) {
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
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
			towerPreview.gameObject.transform.position = selectedSpot.gameObject.transform.position;
			towerPreview.gameObject.transform.localScale = selectedSpot.gameObject.transform.localScale;
			ShowRangeIndicator (towerPreview);
		}


		public void BuildPreviewedTower() {
			if (towerPreview != null) {
				if (LevelController.instance.SubtractResources(towerPreview.cost)) {
					towerPreview.Build ();
					towerPreview = null;
					HideRangeIndicator ();
				} else {
					DestroyTowerPreview ();
					Debug.Log ("not enough gold");
				}
			}
		}

		public void SellSelectedTower() {
			if (selectedTower != null) {
				LevelController.instance.AddResources (selectedTower.cost);
				DestroyImmediate (selectedTower.gameObject);
			}
			HideRangeIndicator ();
		}

		private void DestroyTowerPreview() {
			if (towerPreview != null) {
				DestroyImmediate (towerPreview.gameObject);
			}
			HideRangeIndicator ();
		}

		private void ShowRangeIndicator(Tower tower) {
			if (rangeIndicator != null) {
				rangeIndicator.transform.position = tower.gameObject.transform.position;
				rangeIndicator.transform.localScale = Vector3.one * tower.range;
				rangeIndicator.SetActive (true);
			}
		}

		private void HideRangeIndicator() {
			if (rangeIndicator != null) {
				rangeIndicator.SetActive (false);
			}
		}

	}
}

