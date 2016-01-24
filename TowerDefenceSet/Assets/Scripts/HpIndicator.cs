using UnityEngine;
using System.Collections;

namespace TDSet {
	public class HpIndicator : MonoBehaviour {
		private Material material;
		private Enemy parentEnemy;

		void Start() {
			parentEnemy = GetComponentInParent<Enemy> ();
			material = GetComponentInChildren<Renderer> ().material;
		}

		void Update () {
			float newGreen = parentEnemy.normalizedHp;
			float newRed = 1 - newGreen;
			material.color = new Color (newRed, newGreen, 0f);
		}
	}
}
