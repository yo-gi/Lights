using UnityEngine;

public class LoicOpenDoorHorizontalCutscene : Cutscene {

   	public TorchGroup requiredGroup;
    public bool right = false;

    private Vector3 initialDoorPos;
    private Vector3 finalDoorPos;
    private Vector3 doorLookPos;

    protected override void InitializeCutscene() {
        initialDoorPos = transform.position;

        var finalDoorPosOffset = transform.localScale.x;

        if (right == false) finalDoorPosOffset *= -1;

        finalDoorPos = initialDoorPos;
        finalDoorPos.x += finalDoorPosOffset;

        Events.Register<OnTorchGroupLitEvent>(e => {
            if (e.group != requiredGroup) return;

            StartCutscene();
        });
    }

    protected override void DefineCutscene() {
        Do(duration: 0.1f, action: DoNothing);
        Do(duration: 1f, action: LookAtDoor);
        Do(duration: 2f, action: MoveDoor);
        Do(duration: 0.5f, action: DoNothing);
        Do(duration: 0.1f, action: LookAtPlayer);
    }

    private void DoNothing() {}

    private void LookAtDoor() {
        LockCamera(initialDoorPos, 15f);
    }

    private void MoveDoor() {
        LockCamera(initialDoorPos, 15f);
        Move(gameObject, initialDoorPos, finalDoorPos);
    }

    private void LookAtPlayer() {
        LockCamera(Player.S.transform.position);
    }
}
