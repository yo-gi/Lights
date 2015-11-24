using UnityEngine;

public class Walk : MonoBehaviour
{
	public static Walk S;

	public float runSpeed;
	public float jumpVelocity;

	public bool doubleJumpEnabled = false;
	public bool doubleJump = false;

    public int surfaceMask;

    Rigidbody2D r;

	public void Awake()
	{
		S = this;

		r = GetComponent<Rigidbody2D>();
        surfaceMask = 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water");
    }

	void Update() {
		var grounded = IsGrounded();

		if (doubleJumpEnabled && grounded) {
			doubleJump = true;
		}

		HandleHorizontalMovement();
		HandleJumping(grounded);
	}

	public void ToggleDoubleJump(bool enabled) {
		doubleJumpEnabled = enabled;
	}

	private void HandleHorizontalMovement() {
        Vector2 vel = new Vector2 (0, r.velocity.y);
		if (Input.GetKey (Key.Left)) {
			vel.x += -1f * runSpeed;
		}
		if (Input.GetKey (Key.Right)) {
            vel.x += runSpeed;
		}
        r.velocity = vel;
	}

	private void HandleJumping(bool grounded) {
		if (Input.GetKeyDown (Key.Jump)) {
			if (grounded == false && doubleJump) {
				r.velocity = new Vector2 (r.velocity.x, jumpVelocity);
				doubleJump = false;
			}
			else if (grounded == true) {
				r.velocity = new Vector2 (r.velocity.x, jumpVelocity);
			}
		}
	}

	private bool IsGrounded() {
		// Note the distance is *slightly* longer than the triangle's height.
		var distance = 0.47f;

		foreach (var hit in Physics2D.RaycastAll(transform.position, Vector2.down, distance, surfaceMask)) {
			if (hit.collider != null) {
				return true;
			}
		}

		return false;
	}
}