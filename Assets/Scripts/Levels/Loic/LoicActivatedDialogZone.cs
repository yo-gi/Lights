using UnityEngine;
using System.Collections;

public class LoicActivatedDialogZone : DialogZone {

    public TorchGroup requiredGroup;
    private bool activated = false;

    public override void Awake() {
        base.Awake();

        Events.Register<OnTorchGroupLitEvent>(e => {
            if (e.group != this.requiredGroup) return;

            this.activated = true;
        });
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        if (this.activated) {
            base.OnTriggerEnter2D(other);
        }
    }
}
