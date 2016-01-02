using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TDSet;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TDSet {
	public class PointAndClickUI : MonoBehaviour {
		public GameObject towerButtonsParent;
		public TowerBuildButton towerBuildButtonPrefab;

		void Start () {
			if (BuildController.instance == null) {
				Debug.LogError ("BuildController is missing! Please add object with BuildController script to scene");
			}
		}
		
		void Update () {
			if (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject()) {
				Tower tower = BuildController.instance.SelectTowerOnScreenPosition(Input.mousePosition)	;
				if (tower != null) {
					HideSpotMenu ();
					ShowTowerMenu (tower);
				} else {
					TowerBuildingSpot buildingSpot = BuildController.instance.SelectBuildingSpotOnScreenPosition(Input.mousePosition);
					if (buildingSpot != null) {
						HideTowerMenu ();
						ShowSpotMenu (buildingSpot);
					} else {
						HideTowerMenu ();
						HideSpotMenu ();
					}
				}

			}
		}

		public void TowerButtonClicked(int towerID) {
			BuildController.instance.SetTowerPreview(towerID);
		}

		private void ShowTowerMenu(Tower tower) {
			Debug.Log ("Show Tower:" + tower);
		}

		private void ShowSpotMenu(TowerBuildingSpot spot) {
			CreateTowerButtons (spot);
			Debug.Log ("Show Spot:" + spot);

		}

		private void HideTowerMenu() {
			Debug.Log ("Hide Tower");
		}

		private void HideSpotMenu() {
			Debug.Log ("Hide Spot");
		}

		private void CreateTowerButtons(TowerBuildingSpot spot) {
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in towerButtonsParent.transform) children.Add(child.gameObject);
			children.ForEach(child => Destroy(child));
			List<Tower> towerTypesForSpot = new List<Tower> (BuildController.instance.towerTypes);
			foreach(int id in spot.restrictedTowersIDs) {
				towerTypesForSpot.RemoveAt (id);
			}
			foreach (Tower t in towerTypesForSpot) {
				TowerBuildButton button = (TowerBuildButton)GameObject.Instantiate (towerBuildButtonPrefab);
				button.transform.SetParent (towerButtonsParent.transform);
				button.Init (t.icon);
				button.GetComponent<Button>().onClick.AddListener(() => TowerButtonClicked(t.typeID));
			}
		}

	}
}
