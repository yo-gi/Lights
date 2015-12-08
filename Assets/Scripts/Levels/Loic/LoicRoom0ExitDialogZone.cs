using UnityEngine;
using System.Collections;

public class LoicRoom0ExitDialogZone : MonoBehaviour {

	private bool activated = false;

    private DialogZone zone = new DialogZone {
        dialog = "Well done! I wonder what's in the next room.",
        showOnce = false,
        minSeconds = 2f,
    };

    void Awake() {
    	Events.Register<OnTorchGroupLitEvent>(e => {
    		if (e.group != TorchGroup.LoicLevel0) return;

    		this.activated = true;
    	});
    }

    void OnTriggerEnter2D(Collider2D other) {
    	if (this.activated) {
	        this.zone.OnTriggerEnter2D(other);

	        Destroy(this.gameObject);
    	}
    }
}
