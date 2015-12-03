using UnityEngine;

public class OnCutsceneStartEvent {
    public int id;
}

public class OnCutsceneEndEvent {
    public int id;
}

public abstract class Cutscene : MonoBehaviour {

    public int id;

    void Awake() {
        if (this.GetComponent<CircleCollider2D>() == null
            && this.GetComponent<BoxCollider2D>() == null) {
            Debug.Log("Cutscene " + this.name + " does not have a collider!");
        }

        this.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject != Player.S.gameObject) return;

        this.enabled = true;

        Events.Broadcast(new OnPauseEvent { paused = true });
        Events.Broadcast(new OnCutsceneStartEvent { id = this.id });
    }

    void Update() {
        if (this.PlayCustcene() == false) {
            this.enabled = false;

            Events.Broadcast(new OnCutsceneEndEvent { id = this.id });
            Events.Broadcast(new OnPauseEvent { paused = false });
        }
    }

    protected abstract bool PlayCustcene();
}
