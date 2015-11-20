using UnityEngine;
using System.Collections.Generic;

public class Teleport : MonoBehaviour, Rechargeable {

    private float teleportBackInSeconds = 2f;
	private float cooldown = 2f;

	private int maxHistorySize = 500;
	private float lastTeleport = 0;
    private float rechargeTime = 0;
	private Queue<Location> locationHistory = new Queue<Location>();

	private Object trailPrefab;
	private Object trail;

	public class Location {
		public Vector3 vector;
		public float time;
	}

    public int MaxCharges
    {
        get { return 1; }
    }

    public int Charges
    {
        get { return TeleportIsAvailable() ? 1 : 0; }
    }

    public float ChargePercentage
    {
        get { return Mathf.Min(rechargeTime/cooldown, 1f); }
    }

	void Awake() {
		this.trailPrefab = Resources.Load("Teleport Trail");

		Events.Register<OnResetEvent>(this.ResetLocationHistory);
		Events.Register<OnLevelCompleteEvent>(this.ResetLocationHistory);
	}

	void Update() {
        rechargeTime = Time.time - lastTeleport;
		if (this.TeleportIsAvailable() && Input.GetKeyDown(KeyCode.K)) {
			this.gameObject.transform.position = this.GetTeleportVector();

			this.ResetLocationHistory();
		}

		this.UpdateLocationHistory(this.gameObject.transform.position);
	}

	void OnEnable() {
		this.CreateTrailObject();
	}

	void OnDisable() {
		if (this.trail) {
			Destroy(this.trail);
		}
	}
	
	private void ResetLocationHistory() {
		this.lastTeleport = Time.time;

		Destroy(this.trail);

		this.locationHistory.Clear();
		this.CreateTrailObject();
	}

	private void CreateTrailObject() {
		GameObject trail = (GameObject) Instantiate(this.trailPrefab, this.transform.position, this.transform.rotation);

		trail.transform.parent = this.transform;
		this.trail = trail;
	}

	private bool TeleportIsAvailable() {
		return (rechargeTime >= cooldown);
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
