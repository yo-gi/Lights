using UnityEngine;
using System.Collections.Generic;

public class Speech {
    public string dialog;
    public float duration;
}

public class Dialog : MonoBehaviour {

    public string textObjectName = "Text";

    private Speech currentMessage = null;
    private float messageStartTime = 0f;

    private TextMesh textObject;
    private Queue<Speech> queue = new Queue<Speech>();

    // Use this to queue messages to the dialog component.
    public void Queue(Speech speech) {
        this.queue.Enqueue(speech);
    }

    public void Awake() {
        this.textObject = gameObject.transform
                            .Find(this.textObjectName)
                            .GetComponent<TextMesh>();
    }

    public void Update() {
        if (currentMessage == null) {
            this.ShowNextDialog();
        }
        else {
            if (Time.time > this.messageStartTime + this.currentMessage.duration) {
                this.ShowNextDialog();
            }
            else {
                this.ClearDialog();
            }
        }
    }

    private void ShowNextDialog() {
        if (this.queue.Count > 0) {
            this.currentMessage = this.queue.Dequeue();
            this.textObject.text = this.currentMessage.dialog;
            this.messageStartTime = Time.time;
        }
    }

    private void ClearDialog() {
        this.textObject.text = "";
    }
}
