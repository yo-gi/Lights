using UnityEngine;

public class Button : MonoBehaviour {

    public int id;

    void Awake() {
        enabled = false;

        if (this.GetComponent<CircleCollider2D>() == null) {
            Debug.Log("WARNING: Button " + this.id + " has no circle collider!");
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject != Player.S.gameObject) return;

        enabled = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject != Player.S.gameObject) return;

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
