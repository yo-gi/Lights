using UnityEngine;

public class DialogZone : MonoBehaviour {

	public static DialogZone currentZone = null;

	public string dialog;
	public bool showOnce;
	public float minSeconds;

	void Awake() {
		this.enabled = false;
		this.dialog = this.dialog.Replace("\\n", "\n");
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject == Player.S.gameObject) {
			Navi.S.dialog.Queue(new Speech {
				dialog = this.dialog,
				duration = this.minSeconds
			});

			if (this.showOnce) {
				Destroy(this.gameObject);
			}
		}
	}
}
