using UnityEngine;
using System.Collections.Generic;

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

    private float shakeDuration = 0f;
    private float shakeAmount = 0.2f;

    void Awake()
    {
        S = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		Transform t = playerObj.transform;
		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(t.position.x, t.position.y, transform.position.z), ref speed, dampTime);

		/*
		if (Teleport.S.enabled) {
			float dist = Vector3.Distance(Teleport.S.GetTeleportLocation(), Player.S.transform.position);
			float desiredCamSize = minSize * (1 + (dist/maxDist));
			if (desiredCamSize > maxSize)
				desiredCamSize = maxSize;

			cam.orthographicSize = Mathf.Lerp(desiredCamSize, cam.orthographicSize, 0.99f);
		}
		*/
		
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
        if (Input.GetKey(Key.Invincibility))
        {
            invincible = !invincible;
            print("invincibility: " + invincible);
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
    }

    public static void ShakeForSeconds(float seconds) {
        MainCam.S.shakeDuration = seconds;
    }

    public static void Reset()
    {
        Player.S.transform.position = Checkpoint.getClosestCheckpoint();
		Navi.S.resetNavi();
    }
}
