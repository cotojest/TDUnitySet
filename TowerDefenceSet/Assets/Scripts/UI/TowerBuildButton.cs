using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TDSet;

namespace TDSet {
	
[RequireComponent (typeof(Button))]
[RequireComponent (typeof(Image))]
	public class TowerBuildButton : MonoBehaviour {

		private Image image;
		private PointAndClickUI uiController;

		void Awake() {
			image = GetComponent<Image> ();
			uiController = GetComponentInParent<PointAndClickUI> ();
		}

		void Start () {
		
		}

		public void Init(Sprite img) {
			image.sprite = img;
			//button.onClick.AddListener(() => uiController.TowerButtonClicked(towerID));
		}

		

	}
}
