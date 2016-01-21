using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TDSet;

namespace TDSet {

	public class TowerMenuPanel : MonoBehaviour {
		public Text towerName;
		public Text towerDesc;
		public Text sellValue;
		public Text upgradeCost;
		private Button upgradeButton;

		void Start () {
			BuildController.instance.onSelectedTowerChange += HandleTowerChange; 
			upgradeButton = FindObjectOfType<PointAndClickUI> ().upgradeButton;
		}

		private void HandleTowerChange(Tower tower) {
			towerName.text = tower.typeName;
			towerDesc.text = tower.description;
			sellValue.text = tower.GetSellValue().ToString ();
			if (tower.upgradedTower != null) {
				upgradeButton.gameObject.SetActive (true);
				upgradeCost.text = tower.upgradedTower.cost.ToString ();
			} else {
				upgradeButton.gameObject.SetActive (false);
			}
		}

	}
}
