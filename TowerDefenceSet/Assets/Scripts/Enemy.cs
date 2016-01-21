﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TDSet {
	[RequireComponent (typeof(NavMeshAgent))]
	public class Enemy : MonoBehaviour {
		private enum State {Normal, Slowed}
		public float hp;
		public uint damageToPlayer = 1;
		public uint minDroppedResources;
		public uint maxDroppedResources;
		private State state;
		private Path path;
		private NavMeshAgent agent;
		private NavMeshPath navPath;
		private int nextPointIndex;
		private float normalSpeed;

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
			normalSpeed = agent.speed;
			transform.position = this.path.waypoints [0].position;
			nextPointIndex = 1;
			SetNextDestinationPoint ();
			state = State.Normal;
		}

		public void AddDamage (float damage) {
			hp -= damage;
			if (hp <= 0) {
				DropResources ();
				Destroy (gameObject);
			}
		}

		public void Slow (float time, float rate) {
			if (state != State.Slowed) {
				agent.speed = normalSpeed - normalSpeed * rate;
				state = State.Slowed;
				Invoke ("ReturnFromSlowedToNormal", time);

			}
		}

		private void ReturnFromSlowedToNormal() {
			state = State.Normal;
			agent.speed = normalSpeed;
		}

		private void SetNextDestinationPoint() {
			if (nextPointIndex > (path.waypoints.Count - 1)) {
				LevelController.instance.DamagePlayer((int)damageToPlayer);
				DestroyImmediate (gameObject);
			} else {
				agent.CalculatePath (path.waypoints [nextPointIndex].position, navPath);
				agent.SetPath (navPath);
			}
		}

		private bool ArrivedToPoint() {
			if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance)) {
				return true;
			}
			return false;
		}

		private void DropResources() {
			int droppedResources = Random.Range ((int)minDroppedResources, (int)maxDroppedResources);
			LevelController.instance.AddResources ((uint)droppedResources);
		}
			
			
	}
}
