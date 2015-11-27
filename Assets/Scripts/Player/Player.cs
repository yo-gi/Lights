using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player S;

    public GameObject water;

    Walk walk;
    Swim swim;

	public int direction;

    void Awake()
    {
        S = this;
    }

    // Use this for initialization
    void Start()
    {
        walk = GetComponent<Walk>();
        swim = GetComponent<Swim>();

        walk.enabled = true;
        swim.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "water")
        {
            walk.enabled = false;
            swim.enabled = true;

            water = collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "water")
        {
            walk.enabled = true;
            swim.enabled = false;

            if (water == collider.gameObject) water = null;
        }
    }
}











