using UnityEngine;
using System.Collections.Generic;

public class Speech {
    public string dialog;
    public float duration;
}

public class Dialog : MonoBehaviour {

    public const string TextObjectName = "Text";
    public const float MinDuration = 2f;

    private TextMesh textObject;

    private Queue<Speech> queue = new Queue<Speech>();

    public Speech current = null;
    public float endTime;

    // Use this to queue messages to the dialog component.
    public void Queue(Speech speech) {
        if (speech.duration < Dialog.MinDuration) {
            speech.duration = Dialog.MinDuration;
        }

        this.queue.Enqueue(speech);
    }

    public void Awake() {
        this.textObject = gameObject.transform
                            .Find(Dialog.TextObjectName)
                            .GetComponent<TextMesh>();

        Events.Register<OnPauseEvent>(this.OnPause);
    }

    public void Update() {
        if (this.current != null && Time.time > this.endTime) {
            this.current = null;

            this.textObject.text = "";
        }

        if (this.current == null && this.queue.Count > 0) {
            this.current = this.queue.Dequeue();
            this.endTime = this.current.duration + Time.time;

            this.textObject.text = this.current.dialog;
        }
    }

    void OnPause(OnPauseEvent e) {
        // Pausing the game will push back the end time for the dialog. We'll
        // store how much time is left in the dialog in the endTime field,
        // and then restore the value when the game is unpaused.
        if (e.paused) {
            this.enabled  = false;
            this.endTime -= Time.time;
        }
        else {
            this.enabled  = true;
            this.endTime += Time.time;
        }
    }
}
