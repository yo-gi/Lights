using System;
using UnityEngine;

public class OnCutsceneStartEvent {
    public int id;
}

public class OnCutsceneEndEvent {
    public int id;
}

public abstract class Cutscene : MonoBehaviour {

    public int id;

    // The length of the cutscene in seconds.
    protected abstract float CutSceneLength { get; }

    // Use this to update your cutscene. 
    protected abstract void DefineCutscene();


    // ----------------------------------------------------------------

    protected void Do(float startTime, float endTime, Action action) {
        if (startTime > this.currentTime || endTime < this.currentTime) return;

        if (this.currentGroupStart < startTime) {
            this.currentGroupStart = startTime;
            this.currentGroupEnd = endTime;
            this.currentGroupDuration = endTime - startTime;

            this.groupNaviStartPos = Navi.S.transform.position;
            this.groupPlayerStartPos = Player.S.transform.position;
        }

        action();
    }

    protected void NaviSay(string dialog) {
        Navi.S.dialog.textObject.text = dialog;
    }

    protected void NaviGo(Vector3 position) {
        float t = (this.currentTime - this.currentGroupStart) / this.currentGroupDuration;

        Navi.S.transform.position = Vector3.Lerp(this.groupNaviStartPos, position, t);
    }

    protected void PlayerGo(Vector3 position) {
        float t = (this.currentTime - this.currentGroupStart) / this.currentGroupDuration;

        Player.S.transform.position = Vector3.Lerp(this.groupPlayerStartPos, position, t);
    }

    // ----------------------------------------------------------------


    private float currentGroupStart = -1f;
    private float currentGroupEnd;
    private float currentGroupDuration;
    private Vector3 groupNaviStartPos;
    private Vector3 groupPlayerStartPos;

    private float cutsceneTime;
    private float currentTime;
    private float startTime;

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
        this.startTime = Time.time;
        this.cutsceneTime = this.CutSceneLength;

        Events.Broadcast(new OnPauseEvent { paused = true });
        Events.Broadcast(new OnCutsceneStartEvent { id = this.id });
    }

    void Update() {
        this.currentTime = Time.time - this.startTime;

        this.DefineCutscene();

        if (this.currentTime > this.cutsceneTime) {
            this.enabled = false;

            Events.Broadcast(new OnCutsceneEndEvent { id = this.id });
            Events.Broadcast(new OnPauseEvent { paused = false });
        }
    }
}
