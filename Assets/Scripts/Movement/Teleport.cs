using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleport : MonoBehaviour {
	private int teleportBackInSeconds = 2;
	private int rechargeInSeconds = 2;
	
	private int maxHistorySize = 500;
	private float lastTeleport = 0;
	private Queue<Location> locationHistory = new Queue<Location>();

	public class Location {
		public Vector3 vector;
		public float time;
	}

	void Update() {
		if (this.TeleportIsAvailable() && Input.GetKeyDown(KeyCode.Z)) {			
			this.gameObject.transform.position = this.GetTeleportVector();

			this.locationHistory.Clear();
		}

		this.UpdateLocationHistory(this.gameObject.transform.position);
	}

	private bool TeleportIsAvailable() {
		return (Time.time > this.lastTeleport + this.rechargeInSeconds);
	}

	private Vector3 GetTeleportVector() {
		var goal = Time.time - this.teleportBackInSeconds;

		while (true) {
			var location = this.locationHistory.Dequeue();

			if (location.time >= goal) {
				return location.vector;
			}
		}
	}

	private void UpdateLocationHistory(Vector3 location) {
		this.locationHistory.Enqueue(new Location() {
			vector = this.gameObject.transform.position,
			time = Time.time,
		});

		while (this.locationHistory.Count > this.maxHistorySize) this.locationHistory.Dequeue();
	}
}
