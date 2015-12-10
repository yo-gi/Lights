using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float xCenter;
    private float xOriginal;
    private float factor;

    // Use this for initialization
    void Start()
    {
        xOriginal = transform.position.x;
        factor = Mathf.Atan(transform.position.z) * 2 / Mathf.PI;
    }

    // Update is called once per frame
    void Update()
    {
        // Direction from original position to the camera
        float distance = MainCam.S.transform.position.x - xCenter;
        Vector3 newPosition = transform.position;
        newPosition.x = xOriginal + factor * distance;
        transform.position = newPosition;
    }
}
