using UnityEngine;

public class Parallax : MonoBehaviour {
    
    private Vector3 originalPosition;
    private float factor;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
        factor = Mathf.Atan(originalPosition.z) * 2 / Mathf.PI;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Direction from original position to the camera
        Vector3 direction = Vector3.zero;
        direction.x = MainCam.S.transform.position.x - originalPosition.x;
        transform.position = originalPosition + factor * direction;
	}
}
