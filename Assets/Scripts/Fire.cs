using UnityEngine;

public class Fire : MonoBehaviour {

	public float burntime;

    bool playerBurning = false;

	void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject == Player.S.gameObject) {
			playerBurning = true;
		}

		Burn.S.setBurning(burntime);
	}

	void OnTriggerExit2D(Collider2D c) {
		if (c.gameObject.gameObject == Player.S.gameObject) {
			playerBurning = false;
		}
	}

	void Update() {
		if (playerBurning) {
			Burn.S.setBurning(burntime);
		}
	}
}
