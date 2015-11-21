using UnityEngine;

public class Button : MonoBehaviour {

    public static readonly KeyCode ButtonKey = KeyCode.Space;

	public int id;

	void Awake() {
		enabled = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Player") return;

		enabled = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag != "Player") return;

		enabled = false;
	}

	void Update() {
		if (Input.GetKeyDown(ButtonKey)) {
            Events.Broadcast(new ButtonEvent {
                id = id,
                level = MainCam.currentLevel
			});
		}
	}
}
