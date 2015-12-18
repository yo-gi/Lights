using UnityEngine;

public class MainCam : MonoBehaviour
{
    public static MainCam S;

    public GameObject playerObj;
    public Vector3 speed;
    public float dampTime;

    public float minSize;
    public float maxSize;
    public float maxDist;

    public bool __________________;

    public bool paused = false;
    public bool invincible = false;

    private Camera cam;
    public bool cameraLocked = false;
    public Vector3 cameraLockPosition;
    public float cameraLockScale;
    public float defaultScale;

    private float shakeDuration = 0f;
    private float shakeAmount = 0.4f;

    void Awake()
    {
        S = this;
        cam = GetComponent<Camera>();

        defaultScale = cam.orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos;
        float targetScale;

        if (cameraLocked) {
            targetPos = cameraLockPosition;
            targetScale = cameraLockScale;
        }
        else {
            targetPos = playerObj.transform.position;
            targetScale = defaultScale;
        }

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z), ref speed, dampTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetScale, 0.2f);

        if (shakeDuration > 0) {
            transform.localPosition += Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }
        else {
            shakeDuration = 0.0f;
        }
    }

    void Update()
    {
        // TODO: Add visual indication in game for Invincibility mode
        if (Input.GetKeyDown(Key.Invincibility))
        {
            invincible = !invincible;
            print("invincibility: " + invincible);
        }
        if (Input.GetKeyDown(Key.Unlock))
        {
            invincible = !invincible;
            print("invincibility: " + invincible);
            Teleport.S.Toggle(enabled);
            Rewind.S.Toggle(enabled);
            Walk.S.ToggleDoubleJump(enabled);
        }
        if (Input.GetKey(Key.Reset))
        {
            Reset();
        }

        if (Input.GetKeyDown(Key.Pause))
        {
            this.paused = ! this.paused;

            Events.Broadcast(new OnPauseEvent { paused = this.paused });
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Player.S.transform.position = new Vector3(1.5f, 1.5f, 0);
            transform.position = new Vector3(1.5f, 1.5f, -10f);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            Player.S.transform.position = new Vector3(86.5f, 14.5f, 0);
            transform.position = new Vector3(86.5f, 14.5f, -10f);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            Player.S.transform.position = new Vector3(91.5f, 67.5f, 0);
            transform.position = new Vector3(91.5f, 67.5f, -10f);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            Player.S.transform.position = new Vector3(7f, 100.5f, 0);
            transform.position = new Vector3(7f, 100.5f, -10f);
        }
    }

    public void LockCamera(Vector3 position, float scale) {
        cameraLocked = true;
        cameraLockPosition = position;
        cameraLockScale = scale;
    }

    public void ReleaseCameraLock() {
        cameraLocked = false;
    }

    public static void ShakeForSeconds(float seconds) {
        MainCam.S.shakeDuration = seconds;
    }

    public static void Reset()
    {
        Player.S.transform.position = Checkpoint.getClosestCheckpoint();
        Navi.S.resetNavi();
        Events.Broadcast(new OnResetEvent());
    }
}
