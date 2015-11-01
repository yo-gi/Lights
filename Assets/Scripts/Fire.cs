using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float burntime;

	public bool collided;

	Collider2D target;

	void OnTriggerEnter2D(Collider2D c) {
		print ("entered collision!");
		collided = true;
		target = c;
		if (collided && target.gameObject.name == "Player") {
			Burn.S.setBurning(burntime);
		}
	}

	void OnTriggerExit2D(Collider2D c) {
		print ("exited collision!");
		collided = false;
	}

	void Update()
	{
		if (collided && target.gameObject.name == "Player") {
			Burn.S.setBurning(burntime);
		}
	}
}
