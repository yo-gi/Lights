using UnityEngine;

public class BossMusic : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        Music.S.setBossMusic();
        MainCam.S.gameObject.GetComponent<Camera>().orthographicSize = 10;

        Destroy(this.gameObject);
    }
}
