using UnityEngine;
using System.Collections;

public class BossProjectile : MonoBehaviour {

	public float lifetime;

	public float attackDamage;
	public float radius;

	public Vector3 targetPos;
	public Vector3 speed;
	public float dampTime;

	// Use this for initialization
	void Start () {
		lifetime = Time.time + 5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > lifetime) {
			DestroyImmediate(this.gameObject);
			return;
		}
		gameObject.transform.position =  Vector3.SmoothDamp(transform.position, targetPos, ref speed, dampTime);
		if (Vector2.Distance (Player.S.transform.position, transform.position) < radius) {
			Navi.S.naviLight.radius -= attackDamage;
		}
	}
}
