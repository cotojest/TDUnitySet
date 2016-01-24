using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageForUser : MonoBehaviour {
	public static MessageForUser instance { get; private set;}
	private Animator anim;
	private Text text;
	void Awake() {
		instance = this;
		anim = GetComponent<Animator> ();
		text = GetComponentInChildren<Text> ();
	}
		
	public void Show(string message) {
		text.text = message;
		anim.SetTrigger ("Show");
	}
}
