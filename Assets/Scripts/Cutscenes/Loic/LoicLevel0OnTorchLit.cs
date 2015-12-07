using UnityEngine;

public class LoicLevel0OnTorchLit : Cutscene {

    public GameObject door;
    
    private Vector3 initialDoorPos;
    private Vector3 finalDoorPos;
    private Vector3 doorLookPos;

    void Start() {
        initialDoorPos = door.transform.position;

        finalDoorPos = initialDoorPos;
        finalDoorPos.y += door.transform.localScale.y;
    }

    protected override void DefineCutscene() {
        Do(duration: 0.1f, action: this.DoNothing);
        Do(duration: 1f, action: this.LookAtDoor);
        Do(duration: 2f, action: this.MoveDoor);
        Do(duration: 1f, action: this.DoNothing);
        Do(duration: 2f, action: this.NaviSpeech);
    }

    private void DoNothing() {}

    private void LookAtDoor() {
        LockCamera(initialDoorPos, 15f);
    }

    private void MoveDoor() {
        LockCamera(initialDoorPos, 15f);
        Move(door, initialDoorPos, finalDoorPos);
    }

    private void NaviSpeech() {
        LockCamera(Player.S.transform.position);
        NaviSay("Looks like the door opened!");
    }
}
