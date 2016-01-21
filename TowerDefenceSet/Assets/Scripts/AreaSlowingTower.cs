using UnityEngine;
using System.Collections;
using TDSet;
using System.Collections.Generic;
using System.Linq;

namespace TDSet {
	public class AreaSlowingTower : Tower {
		[Range(0f,1f)]
		public float slowRate;
		public float slowDuration;
		public float cooldown;

		void Start() {
		}

		protected override void StartTowerBehaviourCouroutine() {
			StartCoroutine (Slowing ());
		}


		IEnumerator Slowing() {
			while (true) {
				yield return new WaitForSeconds (cooldown);
				Collider[] collidersInRange = Physics.OverlapSphere (transform.position, range);
				List<Enemy> enemiesInRange = collidersInRange.Select (coll => coll.gameObject.GetComponentInParent<Enemy>())
					.Where (enemy => enemy != null).ToList();
				foreach (Enemy e in enemiesInRange) {
					e.Slow (slowDuration, slowRate);
				}
			}
		}
	}
}
