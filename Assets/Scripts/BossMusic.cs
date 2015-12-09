using UnityEngine;

public class BossMusic : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        Music.S.setBossMusic();

        Destroy(this.gameObject);
    }
}
