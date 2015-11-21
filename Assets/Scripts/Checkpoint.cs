using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	
	public static Vector3 latestCheckpoint = new Vector3(0f, 2f);
	public static int collideMask = 1 << LayerMask.NameToLayer("Player");

	public float length;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (var hit in Physics2D.RaycastAll(transform.position, Vector2.up, length, collideMask)) {
			if (hit.collider != null) {
				latestCheckpoint = gameObject.transform.position;
			}
		}
	}
}
