using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TDSet {
	[RequireComponent (typeof(NavMeshAgent))]
	public class Enemy : MonoBehaviour {
		public float hp;
		private Path path;
		private NavMeshAgent agent;
		private NavMeshPath navPath;
		private int nextPointIndex;

		void Update () {
			if (ArrivedToPoint ()) {
				nextPointIndex++;
				SetNextDestinationPoint ();
			}
		
		}

		public void Init(Path path) {
			agent = GetComponent<NavMeshAgent> ();
			navPath = new NavMeshPath ();
			this.path = path;
			transform.position = this.path.waypoints [0].position;
			nextPointIndex = 1;
			SetNextDestinationPoint ();
		}

		public void SetNextDestinationPoint() {
			Debug.Log (navPath);

			if (nextPointIndex > (path.waypoints.Count - 1)) {
				DestroyImmediate (gameObject);
			} else {
				agent.CalculatePath (path.waypoints [nextPointIndex].position, navPath);
				agent.SetPath (navPath);
			}
		}

		public bool ArrivedToPoint() {
			if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance)) {
				return true;
			}
			return false;
		}
	}
}
