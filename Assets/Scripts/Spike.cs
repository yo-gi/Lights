using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == Player.S.gameObject) {
            Events.Broadcast(new OnDeathEvent());
        }
    }
}
