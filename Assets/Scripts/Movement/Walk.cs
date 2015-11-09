using UnityEngine;

public class Walk : MonoBehaviour
{
    public static Walk S;
	
	public float runSpeed;
	public float jumpHeight;

    public bool grounded = false;

	Rigidbody2D r;
	
	public void Awake ()
	{
        S = this;
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
        // Check to see if the character is on the ground
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.45f, ~(1 << 10));
        bool change = false;
        if (grounded == true) grounded = false;
        else change = true;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                grounded = true;
                break;
            }
        }
        if (grounded && change)
        {
            r.velocity = Vector2.zero;
            Events.Broadcast(new OnLanding());
        }
        // Jumping
        if (grounded && Input.GetKeyDown (KeyCode.W)) {
            Jump();
		}
	}

    public void Jump()
    {
        r.velocity = new Vector2(r.velocity.x, jumpHeight);
    }
}
