using UnityEngine;

public class Button : MonoBehaviour {

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
        if (Input.GetKeyDown(Key.Activate)) {
            Events.Broadcast(new ButtonEvent {
                id = id,
                level = MainCam.currentLevel
            });
        }
    }
}
