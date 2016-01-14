using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TDSet;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TDSet {
	public class PointAndClickUI : MonoBehaviour {
		public ToggleGroup towerToggleGroup;
		public TowerBuildToggle towerBuildButtonPrefab;
		public Animator towerMenuAnimator;
		public Animator spotMenuAnimator;
		public Button buildButton;
		public Button sellButton;
		public Button upgradeButton;
		public Button nextWaveButton;
		public Text resourcesValueText;
		public Text lifePointsValueText;

		void Awake() {
			LevelController.instance.onResourcesChange.AddListener (() => ShowResourcesValue(LevelController.instance.GetResources()));
			LevelController.instance.onPlayerLifePointsChange.AddListener (() => ShowLifePointsValue(LevelController.instance.GetLifePoints()));
		}

		void Start () {
			if (BuildController.instance == null) {
				Debug.LogError ("BuildController is missing! Please add object with BuildController script to scene");
			}
			buildButton.onClick.AddListener (() => BuildController.instance.BuildPreviewedTower ());
			buildButton.onClick.AddListener (() => HideSpotMenu ());
			sellButton.onClick.AddListener (() => BuildController.instance.SellSelectedTower ());
			sellButton.onClick.AddListener (() => HideTowerMenu ());
			nextWaveButton.onClick.AddListener (() => EnemyWavesController.instance.RunNextWave ());
			EnemyWavesController.instance.onSpawningEndedChange += ChangeNextWaveButtonInteractable;
			Debug.Log (LevelController.instance);

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

		public void TowerToggleClicked(int id, bool value) {
			if (value) {
				BuildController.instance.SetTowerPreview (id);
				buildButton.interactable = true;
			}
		}

		private void ShowTowerMenu(Tower tower) {
			if (towerMenuAnimator != null) {
				towerMenuAnimator.SetTrigger ("Show");
			}
		}

		private void ShowSpotMenu(TowerBuildingSpot spot) {
			CreateTowerButtons (spot);
			if (spotMenuAnimator != null) {
				spotMenuAnimator.SetTrigger ("Show");
			}
		}

		private void HideTowerMenu() {
			if (towerMenuAnimator != null) {
				towerMenuAnimator.SetTrigger ("Hide");
			}
		}

		private void HideSpotMenu() {
			if (spotMenuAnimator != null) {
				spotMenuAnimator.SetTrigger ("Hide");
			}
			buildButton.interactable = false;
		}

		private void CreateTowerButtons(TowerBuildingSpot spot) {
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in towerToggleGroup.gameObject.transform) children.Add(child.gameObject);
			children.ForEach(child => Destroy(child));
			List<Tower> towerTypesForSpot = new List<Tower> (BuildController.instance.towerTypes);
			foreach(int id in spot.restrictedTowersIDs) {
				towerTypesForSpot.RemoveAt (id);
			}
			foreach (Tower t in towerTypesForSpot) {
				TowerBuildToggle toggle = (TowerBuildToggle)GameObject.Instantiate (towerBuildButtonPrefab);
				toggle.transform.SetParent (towerToggleGroup.gameObject.transform);
				toggle.Init (t.icon, t.typeID);
				toggle.GetComponent<Toggle> ().group = towerToggleGroup;
				toggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => TowerToggleClicked(toggle.typeID, value));
			}
		}

		private void ShowResourcesValue(uint value) {
			resourcesValueText.text = value.ToString ();
		}

		private void ShowLifePointsValue(int value) {
			lifePointsValueText.text = value.ToString ();
		}

		private void ChangeNextWaveButtonInteractable(bool value) {
			nextWaveButton.interactable = value;
		}
			
	}
}
