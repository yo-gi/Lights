using UnityEngine;
using System.Collections;

public class DialogZone : MonoBehaviour {

	public static DialogZone currentZone = null;

	public string dialog;
	public bool showOnce;
	public float minSeconds;

	private float startTime;
	private bool shouldRemove = false;

	void Awake() {
		this.enabled = false;
		this.dialog = this.dialog.Replace("\\n", "\n");
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject == Player.S.gameObject) {
			Navi.S.Speech = this.dialog;
			this.startTime = Time.time;
			this.enabled = true;

			DialogZone.currentZone = this;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (this.enabled && other.gameObject == Player.S.gameObject) {
			this.shouldRemove = true;
		}
	}

	void Update() {
		if (DialogZone.currentZone != this) {
			this.Stop();
		}
		else if (this.shouldRemove && Time.time >= this.startTime + this.minSeconds) {
			Navi.S.Speech = "";
			DialogZone.currentZone = null;

			this.Stop();
		}
	}

	private void Stop() {
		if (this.showOnce) {
			Destroy(this.gameObject);
		}
		else {
			this.shouldRemove = false;
			this.enabled = false;
		}
	}
}
