using UnityEngine;

public class LightRay : MonoBehaviour {

	bool triggered = false;

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
