using UnityEngine;

public class Walk : MonoBehaviour
{
	
	public float runSpeed;
	public float jumpVelocity;

	public bool doubleJump = false;

    public int surfaceMask;

    public static readonly KeyCode Left = KeyCode.A;
    public static readonly KeyCode Right = KeyCode.D;
    public static readonly KeyCode Jump = KeyCode.W;
    public static readonly KeyCode Down = KeyCode.S;

    Rigidbody2D r;

	public void Awake()
	{
		r = GetComponent<Rigidbody2D>();
        surfaceMask = 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water");
    }

	void Update() {
		var grounded = IsGrounded();

		if (grounded) {
			doubleJump = true;
		}

		HandleHorizontalMovement();
		HandleJumping(grounded);
	}

	private void HandleHorizontalMovement() {
        Vector2 vel = new Vector2 (0, r.velocity.y);
		if (Input.GetKey (Left)) {
			vel.x += -1f * runSpeed;
		}
		if (Input.GetKey (Right)) {
            vel.x += runSpeed;
		}
        r.velocity = vel;
	}

	private void HandleJumping(bool grounded) {
		if (Input.GetKeyDown (Jump)) {
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