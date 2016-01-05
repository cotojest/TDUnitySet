using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TDSet;

namespace TDSet {
[RequireComponent (typeof(Toggle))]
[RequireComponent (typeof(Image))]
	public class TowerBuildToggle : MonoBehaviour {

		private Image image;
		public int typeID;

		void Awake() {
			image = GetComponent<Image> ();
		}
			
		public void Init(Sprite img, int typeID) {
			image.sprite = img;
			this.typeID = typeID;
		}
	}
}
