using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour
{
    public GameObject barrier;

    private BoxCollider2D box;

    void Awake()
    {
        box = gameObject.GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == Player.S.gameObject)
        {
            barrier.SetActive(true);
            box.enabled = false;
        }
    }
}
