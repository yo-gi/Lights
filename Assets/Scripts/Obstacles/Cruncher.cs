using UnityEngine;
using System.Collections;

public enum CruncherState {
	Crunching,
	Returning,
	Waiting
}

public class Cruncher : MonoBehaviour {


	public Vector2 crunchAcceleration;
	public float returnVelocity;
	public float triggerDistance;
	
	static int collideMask = 1 << LayerMask.NameToLayer("Player");

	Vector3 start;
	CruncherState state = CruncherState.Waiting;
	Rigidbody2D r;


	// Use this for initialization
	void Start () {
		start = transform.position;
		r = GetComponent<Rigidbody2D>();
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Navi.S.GetComponent<Collider2D>());
		if (crunchAcceleration.x == 0) {
			r.constraints |= RigidbodyConstraints2D.FreezePositionX;
		} else if (crunchAcceleration.y == 0) {
			r.constraints |= RigidbodyConstraints2D.FreezePositionY;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch(state) {
		case CruncherState.Waiting:
			if (Vector3.Distance(Player.S.transform.position, transform.position) < triggerDistance) {
				state = CruncherState.Crunching;
			}
			break;
		case CruncherState.Crunching:
			r.velocity += crunchAcceleration;
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

	void OnCollisionEnter2D(Collision2D c)
	{
		if (c.gameObject == Player.S.gameObject)
		{
			Events.Broadcast(new OnDeathEvent());
			state = CruncherState.Returning;
		} else {
			state = CruncherState.Returning;
		}
	}
	
	void OnCollisionExit2D(Collision2D c)
	{
	}
}
