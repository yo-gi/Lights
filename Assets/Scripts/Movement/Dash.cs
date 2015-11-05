using UnityEngine;

public class Dash : MonoBehaviour {

	private float maxDashDistance = 10f;
	private int rechargeInSeconds = 2;

	private float lastDash = -9999f;

	// Update is called once per frame
	void Update () {
		if (this.CanDash() && Input.GetKeyDown(KeyCode.Space)) {
			this.lastDash = Time.time;

			this.gameObject.transform.position += this.GetDashVector();
		}
		else if (!CanDash()) {
			Debug.Log ("Can't Dash yet");
		}
	}

	private bool CanDash() {
		return Time.time > this.lastDash + this.rechargeInSeconds;
	}

	private Vector3 GetDashVector() {
		var dashDirection = this.GetDashDirection();
		var dashDistance = this.GetDashDistance(dashDirection);

		return dashDirection * dashDistance;
	}

	private Vector3 GetDashDirection() {
		var direction = Vector3.zero;

		if (Input.GetKey(KeyCode.W)) direction.y += 1;
		if (Input.GetKey(KeyCode.S)) direction.y -= 1;
		if (Input.GetKey(KeyCode.A)) direction.x -= 1;
		if (Input.GetKey(KeyCode.D)) direction.x += 1;

		return direction;
	}

	// TODO: Test & refactor this!
	private float GetDashDistance(Vector3 direction) {
		// We don't want the dash to take the player into a wall. Trim the distance down until we
		// reach a point that isn't in a wall.
		var distance = this.maxDashDistance;

		var origin = this.gameObject.transform.position;
		var mask = 1 << 13; // TODO: Find a better way for this
		RaycastHit hitInfo;

		while (Physics.Raycast(origin, direction, out hitInfo, distance, mask)) {
			distance = hitInfo.distance;
		}

		return distance;
	}
}
