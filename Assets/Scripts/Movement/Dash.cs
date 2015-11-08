using UnityEngine;

public class Dash : MonoBehaviour {

	private float maxDashDistance = 10f;
	private int rechargeInSeconds = 2;

	private float lastDash = -9999f;

	// Update is called once per frame
	void Update () {
		if (this.CanDash() && Input.GetKeyDown(KeyCode.J)) {
			this.lastDash = Time.time;

			this.gameObject.transform.position += this.GetDashVector();
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

	private float GetDashDistance(Vector3 direction) {
		// Find all the walls in the dash's path.
		var start = this.gameObject.transform.position;
		var mask = (1 << LayerMask.NameToLayer("Terrain"));

		var hits = Physics2D.RaycastAll(start, direction, this.maxDashDistance, mask);

		// Return the max dash distance if there are no walls.
		if (hits.Length == 0) return this.maxDashDistance;

		// Raycast backwards to find the end point of the wall.
		var lastHit = hits[hits.Length - 1];
		var end = start + direction * this.maxDashDistance;

		var reverseHit = Physics2D.Raycast(end, -1 * direction, this.maxDashDistance, mask);
		var reverseHitPoint = reverseHit.point;

		if (Vector3.Distance(start, reverseHitPoint) < this.maxDashDistance) {
			// The reverse hitpoint is within the dash's range. Use the max dash distance for the dash.
			return this.maxDashDistance;
		}
		else {
			// The reverse hitpoint is within the dash's range. Teleport to the last wall's hit point.
			return lastHit.distance;
		}
	}
}
