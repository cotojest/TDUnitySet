using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TDSet;
using UnityEngine.SceneManagement;
namespace TDSet {
	public class GameOverPanel : MonoBehaviour {
		public string gameWonMessage;
		public string gameLostMessage;
		public Text gameResultText;
		private Animator anim;

		void Awake() {
			anim = GetComponent<Animator> ();
		}

		void Start () {
			LevelController.instance.onLevelFinished += ShowPanel;	
		}

		public void ReloadLevel() {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		private void ShowPanel(bool gameWon) {
			if (gameWon) {
				gameResultText.text = gameWonMessage;
			} else {
				gameResultText.text = gameLostMessage;
			}
			anim.SetTrigger ("Show");
		}
	}
}
