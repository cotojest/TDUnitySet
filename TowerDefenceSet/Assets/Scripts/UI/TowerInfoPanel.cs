using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TDSet;

namespace TDSet {

	public class TowerInfoPanel : MonoBehaviour {
		public Text towerName;
		public Text towerDesc;
		public Text cost;
		private string standardTitleText;
		private string standardDescText;

		void Start () {
			BuildController.instance.onTowerPreviewChange += HandleTowerChange; 
			standardTitleText = towerName.text;
			standardDescText = towerDesc.text;
			cost.gameObject.SetActive (false);
		}

		private void HandleTowerChange(Tower tower) {
			if (tower != null) {
				towerName.text = tower.typeName;
				towerDesc.text = tower.description;
				cost.gameObject.SetActive (true);
				cost.text = tower.cost.ToString ();
			} else {
				cost.gameObject.SetActive (false);
				towerName.text = standardTitleText;
				towerDesc.text = standardDescText;
			}
		}

	}
}
