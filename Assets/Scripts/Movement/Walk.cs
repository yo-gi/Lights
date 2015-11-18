using UnityEngine;

public class Walk : MonoBehaviour
{
	
	public float runSpeed;
	public float jumpHeight;

	public bool doubleJump = false;

	Rigidbody2D r;

	public void Awake()
	{
		r = GetComponent<Rigidbody2D>();
	}

	void Update() {
		var grounded = this.IsGrounded();

		if (grounded) {
			this.doubleJump = true;
		}

		this.HandleHorizontalMovement();
		this.HandleJumping(grounded);
	}

	private void HandleHorizontalMovement() {
		if (Input.GetKey (KeyCode.A)) {
			r.velocity = new Vector2 (-1f * runSpeed, r.velocity.y);
		}
		if (Input.GetKey (KeyCode.D)) {
			r.velocity = new Vector2 (runSpeed, r.velocity.y);
		}
		if (!Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			r.velocity = new Vector2 (0, r.velocity.y);
		}
	}

	private void HandleJumping(bool grounded) {
		if (Input.GetKeyDown (KeyCode.W)) {
			if (grounded == false && this.doubleJump) {
				r.velocity = new Vector2 (r.velocity.x, jumpHeight);
				this.doubleJump = false;
			}
			else if (grounded == true) {
				r.velocity = new Vector2 (r.velocity.x, jumpHeight);
			}
		}
	}

	private bool IsGrounded() {
		// Note the distance is *slightly* longer than the triangle's height.
		var distance = 0.47f;
		var wallMask = ~(1 << 10);

		foreach (var hit in Physics2D.RaycastAll(transform.position, Vector2.down, distance, wallMask)) {
			if (hit.collider != null) {
				return true;
			}
		}

		return false;
	}
}