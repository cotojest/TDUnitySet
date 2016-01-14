using UnityEngine;
using System.Collections;
using TDSet;
using System.Collections.Generic;
using System.Linq;

namespace TDSet {
	public class StandardAttackTower : Tower {
		public float damagePerHit;
		public float cooldown;
		public float rotationSpeed;
		public GameObject rotatingElement;

		private Enemy currentTarget;

		void Start() {
		}
		void Update () {
			if (currentTarget != null) {
				Vector3 targetDir = currentTarget.transform.position - rotatingElement.transform.position;
				targetDir.y = 0;
				float step = rotationSpeed * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards(rotatingElement.transform.forward, targetDir, step, 0.0F);
				Debug.DrawRay(rotatingElement.transform.position, newDir, Color.red);
				Quaternion lookRotation = Quaternion.LookRotation (newDir, Vector3.forward);
				rotatingElement.transform.rotation = new Quaternion( 0f, lookRotation.y, 0f, lookRotation.w);


			}
		}

		protected override void StartTowerBehaviourCouroutine() {
			StartCoroutine (Shooting ());
		}


		IEnumerator Shooting() {
			while (true) {
				yield return new WaitForSeconds (cooldown);
				Collider[] collidersInRange = Physics.OverlapSphere (transform.position, range);
				List<Enemy> enemiesInRange = collidersInRange.Select (coll => coll.gameObject.GetComponentInParent<Enemy>())
					.Where (enemy => enemy != null).ToList();
				if (enemiesInRange.Count > 0) {
					currentTarget = enemiesInRange [0];
					enemiesInRange [0].AddDamage (damagePerHit);
				}
					
			}
		}
	}
}
