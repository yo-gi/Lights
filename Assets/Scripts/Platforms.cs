using UnityEngine;
using System.Collections;

public class Platforms : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public float lerpSpeed;
    public float platformPauseTime;
    public bool active = true;

    float startTime;

    GameObject player;
    bool colliding = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine("MovePlatform");
    }

    IEnumerator MovePlatform()
    {
        while (active)
        {
            if (transform.position == end)
            {
                startTime = Time.time;
                Vector3 temp = new Vector3(end.x, end.y, end.z);
                end = start;
                start = temp;
                yield return new WaitForSeconds(platformPauseTime);
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

            yield return null;
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
