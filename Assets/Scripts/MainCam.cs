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

    public static int currentLevel = 1;
    public static Dictionary<int, Vector3> startTable = new Dictionary<int, Vector3>();
    public static Dictionary<int, GameObject> levelTable = new Dictionary<int, GameObject>();

    private float shakeDuration = 0f;
    private float shakeAmount = 0.2f;

	Camera cam;

    void Awake()
    {
        S = this;
		cam = GetComponent<Camera>();

        GameObject levelObject = GameObject.Find("Level_" + currentLevel);
        while (levelObject != null)
        {
            levelTable[currentLevel] = levelObject;
            levelObject.SetActive(false);
            levelObject = GameObject.Find("Level_" + (++currentLevel));
        }
        currentLevel = 1;
        levelTable[1].SetActive(true);

        startTable[1] = new Vector3(1, 3, 0);
        startTable[2] = new Vector3(23, 3, 0);
        startTable[3] = new Vector3(55, 13, 0);
        startTable[4] = new Vector3(93, 18, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		Transform t = playerObj.transform;
		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(t.position.x, t.position.y, transform.position.z), ref speed, dampTime);
		if (Teleport.S.enabled) {
			float dist = Vector3.Distance(Teleport.S.GetTeleportLocation(), Player.S.transform.position);
			float desiredCamSize = minSize * (1 + (dist/maxDist));
			if (desiredCamSize > maxSize)
				desiredCamSize = maxSize;

			cam.orthographicSize = Mathf.Lerp(desiredCamSize, cam.orthographicSize, 0.99f);
		}
		
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
            RestartLevel();
        }

        if (Input.GetKeyDown(Key.Pause))
        {
            this.paused = ! this.paused;

            Events.Broadcast(new OnPauseEvent { paused = this.paused });
        }
    }

    // Given an object in the Hierarchy, returns list of children filtered by tag. If T is GameObject,
    // will return the children GameObjects, but if T is something else, will return component T from
    // the children GameObjects.
    public static List<T> FilterByTag<T>(GameObject parent, string tag)
    {
        List<T> objects = new List<T>();
        foreach (Transform transform in parent.GetComponentsInChildren<Transform>())
        {
            if (transform.tag == tag)
            {
                if (typeof(T) == typeof(GameObject)) objects.Add((T)(object)transform.gameObject);
                else objects.Add(transform.gameObject.GetComponent<T>());
            }
        }
        return objects;
    }

    public static void ShakeForSeconds(float seconds) {
        MainCam.S.shakeDuration = seconds;
    }

    public static void NextLevel()
    {
        if (currentLevel == levelTable.Count)
        {
            // Win
        }
        else
        {
            SwapToLevel(currentLevel + 1);
        }
    }

    public static void RestartLevel()
    {
        //Events.Broadcast(new OnResetEvent());
		Player.S.transform.position = Checkpoint.latestCheckpoint;
		Navi.S.resetNavi();
        //SwapToLevel(currentLevel);
        //Player.S.transform.position = startTable[currentLevel];
    }

    private static void SwapToLevel(int level)
    {
        levelTable[currentLevel].SetActive(false);
        levelTable[level].SetActive(true);
        currentLevel = level;
        Player.S.transform.position = startTable[currentLevel];
        Events.Broadcast(new OnLevelLoadEvent { level = currentLevel });
    }
}
