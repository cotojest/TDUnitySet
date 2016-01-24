using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using TDSet;

namespace TDSet {
	public class BuildController : MonoBehaviour {
		public List<Tower> towerTypes;
		public GameObject rangeIndicator;
		public GameObject buildingIndicator;
		private Tower towerPreview;

		private TowerBuildingSpot selectedSpot;
		private Tower selectedTower;
		public event Action<Tower> onSelectedTowerChange;
		public event Action<Tower> onTowerPreviewChange;

		public static BuildController instance {
			get {
				if (_instance == null) {
					_instance = FindObjectOfType<BuildController> ();
				}
				return _instance;
			}
		}
		private static BuildController _instance;

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
					if (onSelectedTowerChange != null) {
						onSelectedTowerChange (selectedTower);
					}	
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
			if (onTowerPreviewChange != null) {
				onTowerPreviewChange (towerPreview);
			}
			ShowRangeIndicator (towerPreview);
		}


		public void BuildPreviewedTower() {
			if (towerPreview != null) {
				if (LevelController.instance.SubtractResources(towerPreview.cost)) {
					towerPreview.Build ();
					AddBuildingIndicator (towerPreview);
					towerPreview = null;
					onTowerPreviewChange (towerPreview);
					HideRangeIndicator ();
				} else {
					DestroyTowerPreview ();
					if (MessageForUser.instance != null) {
						MessageForUser.instance.Show ("NOT ENOUGH GOLD");
					}
				}
			}
		}

		public void UpgradeSelectedTower() {
			if (selectedTower != null && selectedTower.upgradedTower != null ) {
				if (LevelController.instance.SubtractResources(selectedTower.upgradedTower.cost)) {
					selectedTower = selectedTower.Upgrade ();
					HideRangeIndicator ();
					AddBuildingIndicator (selectedTower);

				} else {
					HideRangeIndicator ();
					if (MessageForUser.instance != null) {
						MessageForUser.instance.Show ("NOT ENOUGH GOLD");
					}
				}
			}
		}

		public void SellSelectedTower() {
			if (selectedTower != null) {
				LevelController.instance.AddResources ((uint)selectedTower.GetSellValue());
				DestroyImmediate (selectedTower.gameObject);
			}
			HideRangeIndicator ();
		}

		private void DestroyTowerPreview() {
			if (towerPreview != null) {
				DestroyImmediate (towerPreview.gameObject);
				onTowerPreviewChange (towerPreview);
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

		private void AddBuildingIndicator(Tower tower) {
			if (buildingIndicator != null) {
				tower.buildingIndicator = GameObject.Instantiate (buildingIndicator);
				tower.buildingIndicator.transform.position = tower.transform.position;
				tower.buildingIndicator.transform.parent = tower.transform;
			}
		}

	}
}

