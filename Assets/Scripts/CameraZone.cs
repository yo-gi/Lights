using UnityEngine;

public class CameraZone : MonoBehaviour {

    public static CameraZone current;

    public float scale;
    public Vector3 position;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == Player.S.gameObject) {
            CameraZone.current = this;

            MainCam.S.LockCamera(position, scale);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (CameraZone.current != this) return;

        if (other.gameObject == Player.S.gameObject) {
            MainCam.S.ReleaseCameraLock();
        }
    }
}
