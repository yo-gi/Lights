using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	public float lerpTime;
	public float lerpSpeed;

	public Vector3 destination;
	public Vector3 start;
	public float length;
	public float startTime;

	// Use this for initialization
	void Start () {
		start = Player.S.gameObject.transform.position;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		length = Vector3.Distance(transform.position, destination);
		float distanceCovered = (Time.time - startTime) * lerpSpeed;
		float fracCovered = distanceCovered/length;
		transform.position = Vector3.Lerp(transform.position, destination, fracCovered);
		if (transform.position == destination) {
			activatePlatforms();
			Destroy(this.gameObject);
		}
	}

	void activatePlatforms() {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, (1 << 8));
		foreach (Collider2D c in colliders) {
			if (c.gameObject.tag == (Player.S.color)) {
				c.gameObject.GetComponent<Platform>().activate();
				print (c.gameObject.name);
			}
		}
	}
}
