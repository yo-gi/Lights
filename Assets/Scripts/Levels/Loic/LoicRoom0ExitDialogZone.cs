using UnityEngine;
using System.Collections;

public class LoicRoom0ExitDialogZone : DialogZone {

    private bool activated = false;

    public void Awake() {
        base.Awake();

        Events.Register<OnTorchGroupLitEvent>(e => {
            if (e.group != TorchGroup.LoicLevel0) return;

            this.activated = true;
        });
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (this.activated) {
            base.OnTriggerEnter2D(other);
        }
    }
}
