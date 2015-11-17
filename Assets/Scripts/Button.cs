using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public int id;

	void Awake() {
		this.enabled = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Player") return;

		this.enabled = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag != "Player") return;

		this.enabled = false;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Events.Broadcast(new ButtonEvent {
				id = id,
				level = 0, // TODO: Use actual level.
			});
		}
	}
}
