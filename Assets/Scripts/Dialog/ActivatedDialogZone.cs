using UnityEngine;

///
/// A dialog zone that can only activated once a specific torch group has been lit.
///
public class ActivatedDialogZone : DialogZone {

    ///
    /// The torch group that must lit for this dialog zone to be activated.
    ///
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
