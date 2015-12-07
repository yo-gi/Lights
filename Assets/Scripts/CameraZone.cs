using UnityEngine;

public class CameraZone : MonoBehaviour {

    public static bool initialized = false;
    public static CameraZone current;

    public float scale;
    public Vector3 position;

    void Awake() {
        if (CameraZone.initialized == false) {
            // A cutscene may mess up the current camera zone's lock. Thus, at the
            // end of a cutscene we'll lock the camera again.
            Events.Register<OnCutsceneEndEvent>(() => {
                if (CameraZone.current == null) return;

                MainCam.S.LockCamera(CameraZone.current.position, CameraZone.current.scale);
            });

            CameraZone.initialized = true;
        }
    }

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

            CameraZone.current = null;
        }
    }
}
