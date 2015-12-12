using UnityEngine;

public enum CruncherState {
	Crunching,
	Returning,
	Waiting
}

public class Cruncher : MonoBehaviour {

    static int collideMask;

	public Vector3 crunchAcceleration;
	public float returnVelocity;
	public float triggerDistance;
	public int damage;

	Vector3 start;
    private Vector3 velocity;
	CruncherState state = CruncherState.Waiting;
	// Rigidbody2D r;
	
	// Use this for initialization
	void Start () {
		start = transform.position;
        velocity = Vector3.zero;
        collideMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Terrain");
        // r = GetComponent<Rigidbody2D>();
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Navi.S.GetComponent<Collider2D>());
		//if (crunchAcceleration.x == 0) {
		//	r.constraints |= RigidbodyConstraints2D.FreezePositionX;
		//} else if (crunchAcceleration.y == 0) {
		//	r.constraints |= RigidbodyConstraints2D.FreezePositionY;
		//}

        transform.GetComponent<Renderer>().sortingLayerName = "Default";
        transform.GetComponent<Renderer>().sortingOrder = 15;

		Events.Register<OnDeathEvent>(() => {
			transform.position = start;
            velocity = Vector3.zero;
			state = CruncherState.Waiting;
			//r.velocity = Vector2.zero;
		});
	}

	// Update is called once per frame
	void FixedUpdate () {
		switch(state) {
		case CruncherState.Waiting:
			var hit = Physics2D.Raycast(transform.position, crunchAcceleration.normalized, triggerDistance, collideMask);
			if (hit.collider != null && hit.collider.gameObject == Player.S.gameObject) {
				state = CruncherState.Crunching;
			}
			break;
		case CruncherState.Crunching:
			velocity += crunchAcceleration;
                transform.position += velocity * Time.fixedDeltaTime;
			break;
		case CruncherState.Returning:
			transform.position = Vector3.MoveTowards(transform.position, start, returnVelocity * Time.deltaTime);
			if (transform.position == start) {
				state = CruncherState.Waiting;
			}
			break;
		default:
			return;
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject == Player.S.gameObject)
		{
			Player.S.TakeDamage(damage);
            velocity = Vector3.zero;
			state = CruncherState.Returning;
		} else if (LayerMask.LayerToName(c.gameObject.layer) == "Terrain") {
            velocity = Vector3.zero;
			state = CruncherState.Returning;
		}
		MainCam.ShakeForSeconds(0.1f);
	}
}
