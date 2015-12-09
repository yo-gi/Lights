using UnityEngine;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {
	
	float length;
	static int collideMask = 1 << LayerMask.NameToLayer("Player");
	static List<Vector3> checkpoints = new List<Vector3>();

	// Use this for initialization
	void Start () {
		length = GetComponent<ParticleSystem>().startSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (var hit in Physics2D.RaycastAll(transform.position, Vector2.up, length, collideMask)) {
			if (hit.collider != null) {
				checkpoints.Add(gameObject.transform.position);
				GetComponent<ParticleSystem>().startColor = Color.green;
				break;
			}
		}
	}

	public static Vector3 getClosestCheckpoint()
	{
		if (checkpoints.Count == 0)
			return new Vector3(1.5f, 2f, 0f);

		return checkpoints[checkpoints.Count - 1];
		
		// Vector3 p = Player.S.gameObject.transform.position;
		// Vector3 closest = checkpoints[0];
		// float minDist = Vector3.Distance(p, closest);
		// foreach (var v in checkpoints) {
		// 	float dist = Vector3.Distance(v, p);
		// 	if (dist < minDist) {
		// 		minDist = dist;
		// 		closest = v;
		// 	}
		// }
		// return closest;
	}
}
