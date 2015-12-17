using UnityEngine;

public class Platforms : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public float lerpSpeed;
    public float platformStartPauseTime = 0f;
    public float platformPauseTime;

    float startTime;
    Vector3 startOriginal;
    Vector3 endOriginal;

    GameObject player;
    bool colliding = false;

    private float waitUntil;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        endOriginal = transform.position + end - start;
		startOriginal = transform.position;

        Reset();

        Events.Register<OnDeathEvent>(Reset);
    }

    void FixedUpdate()
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

    void Reset()
    {
        transform.position = startOriginal;
        start = startOriginal;
        end = endOriginal;
        startTime = Time.time + platformStartPauseTime;
        waitUntil = platformPauseTime + startTime;
    }
}
