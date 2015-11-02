using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float burntime;

//	Collider2D target;

	GameObject player;

	void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject.name == "Player") {
			this.player = c.gameObject;
		}

		Burn.S.setBurning(burntime);
	}

	void OnTriggerExit2D(Collider2D c) {
		if (c.gameObject.name == "Player") {
			this.player = null;
		}
	}

	void Update() {
		if (this.player) {
			Burn.S.setBurning(burntime);
		}
	}
}
