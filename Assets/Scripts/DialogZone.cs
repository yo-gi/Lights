using UnityEngine;
using System.Collections;

public class DialogZone : MonoBehaviour {

	public string dialog;
	public bool showOnce;

	private bool hasShown = false;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject == Player.S.gameObject) {
			if (showOnce && hasShown) return;

			Navi.S.Speech = this.dialog;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject == Player.S.gameObject) {
			Navi.S.Speech = "";
			this.hasShown = true;
		}
	}
}
