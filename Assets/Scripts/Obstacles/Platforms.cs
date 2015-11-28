using UnityEngine;
using System.Collections;

public class Platforms : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public float lerpSpeed;
    public float platformPauseTime;

    float startTime;

    GameObject player;
    bool colliding = false;

    private float waitUntil;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        var currentTime = Time.time;

        if (currentTime < waitUntil) return;

        if (transform.position == end)
        {
            startTime = currentTime;
            Vector3 temp = new Vector3(end.x, end.y, end.z);
            end = start;
            start = temp;

            waitUntil = platformPauseTime + currentTime;
        }

        float length = Vector3.Distance(transform.position, end);
        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        float fracCovered = distanceCovered / length;
        Vector3 newPos = Vector3.Lerp(transform.position, end, fracCovered);
        Vector3 diff = newPos - transform.position;
        transform.position = newPos;

        if (colliding)
        {
            player.transform.position = player.transform.position + diff;
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject == player)
        {
            colliding = true;
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject == player)
        {
            colliding = false;
        }
    }
}
