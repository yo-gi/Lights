using UnityEngine;

public class LightRay : MonoBehaviour {

	static GameObject boss;

	bool triggered = false;

	// Use this for initialization
	void Start () {
		boss = GameObject.Find("Boss");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!triggered && other.gameObject == Boss.S.gameObject) {
			triggered = true;
			Boss.S.takeDamage();
		}

		if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<Enemy>().die();
		}
		
	}
}
