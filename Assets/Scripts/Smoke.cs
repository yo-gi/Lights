using UnityEngine;
using System.Collections;

public class Smoke : MonoBehaviour {

	float endTime;

	// Use this for initialization
	void Start () {
		endTime = Time.time + GetComponent<ParticleSystem>().duration;	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > endTime) {
			Destroy(this.gameObject);
		}
	}
}
