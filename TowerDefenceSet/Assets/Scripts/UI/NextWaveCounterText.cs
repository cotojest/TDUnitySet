using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TDSet {
	public class NextWaveCounterText : MonoBehaviour {
		private Text counterValue;
	
		void Start () {
			counterValue = GetComponent<Text> ();
			if (GetComponent<Animator> () != null) {
				EnemyWavesController.instance.onSpawningEndedChange += TriggerAnimation;
			}

		}

		void Update () {
			if (EnemyWavesController.instance.state == EnemyWavesController.State.WaitingForNextWave) {
				counterValue.text = EnemyWavesController.instance.GetSecondsToNextWave ().ToString ();
			}
		}

		void TriggerAnimation(bool showText) {
			GetComponent<Animator> ().SetBool ("Show", showText);
		}
	}
}
