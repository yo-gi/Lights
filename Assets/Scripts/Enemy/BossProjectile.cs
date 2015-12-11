using UnityEngine;

public class BossProjectile : MonoBehaviour {

	public float lifetime;

	public int attackDamage;
	public float radius;

	public Vector3 targetPos;
	public Vector3 speed;
	public float dampTime;

	// Use this for initialization
	void Start () {
		lifetime = Time.time + 5f;
	}
	
	//void OnTriggerEnter2D(Collider2D other) {
	//	if (other.tag == "Torch") {
	//		if(!other.GetComponent<Torch>().active) return;
	//		other.GetComponent<Torch>().takeDamage(.2f);
	//		Destroy(this.gameObject);
	//	}
	//}
	
	void OnCollisionEnter2D(Collision2D other) {
		// Destroy the projectile if it hits the player.
		if (other.gameObject == Player.S.gameObject) {
			Player.S.takeDamage(attackDamage);
			Destroy(this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (Time.time > lifetime) {
			DestroyImmediate(this.gameObject);
			return;
		}
        // Use speed instead of damp time. If the target is further away, the projectile will move too fast
		gameObject.transform.position =  Vector3.SmoothDamp(transform.position, targetPos, ref speed, dampTime);
		if (Vector2.Distance (Player.S.transform.position, transform.position) < radius) {
			Player.S.takeDamage(attackDamage);
		}
	}
}
