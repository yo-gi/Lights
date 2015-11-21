using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public int speed = 1;

    // Update is called once per frame
    void Update()
    {
        Vector3 r = transform.rotation.eulerAngles;
        r.z -= speed;
        if (r.z == -180)
            r.z = 0;
        transform.rotation = Quaternion.Euler(r);
    }
}
