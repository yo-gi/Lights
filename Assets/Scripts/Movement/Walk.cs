using UnityEngine;

public class Walk : MonoBehaviour
{
	
	public float runSpeed;
	public float jumpHeight;

	public bool grounded = false;
	public bool doubleJump = false;

	Rigidbody2D r;

	public void Awake()
	{
		r = GetComponent<Rigidbody2D>();
	}

	void Update() {
		this.HandleHorizontalMovement();
		this.HandleJumping();
	}

	void FixedUpdate() {
		this.grounded = this.IsGrounded();

		if (this.grounded) {
			this.doubleJump = true;
		}
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

	private void HandleJumping() {
		if (Input.GetKeyDown (KeyCode.W)) {
			if (grounded == false && doubleJump) {
				r.velocity = new Vector2 (r.velocity.x, jumpHeight);
				doubleJump = false;
			}
			else if (grounded == true) {
				r.velocity = new Vector2 (r.velocity.x, jumpHeight);
			}
		}
	}

	private bool IsGrounded() {
		var wallMask = ~(1 << 10);

		foreach (var hit in Physics2D.RaycastAll(transform.position, Vector2.down, 0.45f, wallMask)) {
			if (hit.collider != null) {
				return true;
			}
		}

		return false;
	}
}