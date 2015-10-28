using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 r = transform.rotation.eulerAngles;
		r.z -= 1;
		if (r.z == -180)
			r.z = 0;
		transform.rotation = Quaternion.Euler(r);
	}
}
