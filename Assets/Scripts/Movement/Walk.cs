using UnityEngine;

public class Walk : MonoBehaviour
{
	
	public float runSpeed;
	public float jumpHeight;

	bool doubleJump = false;

	Rigidbody2D r;
	
	public void Awake ()
	{
		r = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		// Horizontal Movement
		if (Input.GetKey (KeyCode.A)) {
			r.velocity = new Vector2 (-1f * runSpeed, r.velocity.y);
		}
		if (Input.GetKey (KeyCode.D)) {
			r.velocity = new Vector2 (runSpeed, r.velocity.y);
		}
		if (!Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			r.velocity = new Vector2 (0, r.velocity.y);
		}

		// Jumping
		if (Input.GetKeyDown (KeyCode.W)) {
			if (doubleJump) {
				r.velocity = new Vector2 (r.velocity.x, jumpHeight);
				doubleJump = false;
				print ("false");
			}
			else {
				RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, Vector2.down, 0.45f, ~(1 << 10));
				foreach (RaycastHit2D hit in hits) {
					if (hit.collider != null) {
						r.velocity = new Vector2 (r.velocity.x, jumpHeight);
						doubleJump = true;
						break;
					}
				}
			}
		}
	}
}
