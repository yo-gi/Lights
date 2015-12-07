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
    public bool showOnce;

    // Use this to update your cutscene. 
    protected abstract void DefineCutscene();


    // ----------------------------------------------------------------

    protected void Do(float duration, Action action) {
        var groupStartTime = this.previousGroupEndTime;
        var groupEndTime = groupStartTime + duration;

        this.previousGroupEndTime = groupEndTime;

        // Is the current time within this groups time frame?
        if (this.currentTime < groupStartTime || this.currentTime > groupEndTime) return;

        // Is this the first time the group is executing?
        if (this.currentGroupStart < groupStartTime) {
            this.currentGroupStart = groupStartTime;
            this.currentGroupDuration = duration;

            this.groupNaviStartPos = Navi.S.transform.position;
            this.groupPlayerStartPos = Player.S.transform.position;

            Navi.S.dialog.textObject.text = "";
        }

        action();
    }

    protected void LockCamera(Vector3 position, float scale = 8f) {
        this.locksCam = true;

        MainCam.S.LockCamera(position, scale);
    }

    protected void NaviSay(string dialog) {
        Navi.S.dialog.textObject.text = dialog;
    }

    protected void Move(GameObject gameObject, Vector3 initial, Vector3 end) {
        float t = (this.currentTime - this.currentGroupStart) / this.currentGroupDuration;

        gameObject.transform.position = Vector3.Lerp(initial, end, t);
    }

    protected void NaviGo(Vector3 position) {
        this.Move(Navi.S.gameObject, this.groupNaviStartPos, position);
    }

    protected void PlayerGo(Vector3 position) {
        this.Move(Player.S.gameObject, this.groupPlayerStartPos, position);
    }

    // ----------------------------------------------------------------

    private float previousGroupEndTime;

    private float currentGroupStart = -1f;
    private float currentGroupDuration;
    private Vector3 groupNaviStartPos;
    private Vector3 groupPlayerStartPos;

    private float currentTime;
    private float startTime;

    private bool locksCam = false;

    void Awake() {
        if (this.GetComponent<CircleCollider2D>() == null
            && this.GetComponent<BoxCollider2D>() == null) {
            Debug.Log("Cutscene " + this.name + " does not have a collider!");
        }

        this.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject != Player.S.gameObject) return;

        Debug.Log(this.name);

        this.enabled = true;
        this.startTime = Time.time;

        Events.Broadcast(new OnPauseEvent { paused = true });
        Events.Broadcast(new OnCutsceneStartEvent { id = this.id });
    }

    void Update() {
        this.previousGroupEndTime = 0;
        this.currentTime = Time.time - this.startTime;

        this.DefineCutscene();

        if (this.currentTime > this.previousGroupEndTime) {
            this.enabled = false;

            // Release the camera lock if we have one.
            if (this.locksCam) {
                MainCam.S.ReleaseCameraLock();
            }

            // Clear Navi's dialog.
            Navi.S.dialog.textObject.text = "";

            Events.Broadcast(new OnCutsceneEndEvent { id = this.id });
            Events.Broadcast(new OnPauseEvent { paused = false });

            if (this.showOnce) {
                Destroy(this.gameObject);
            }
        }
    }
}
