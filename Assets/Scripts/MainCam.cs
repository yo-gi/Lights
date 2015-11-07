using UnityEngine;
using System.Collections.Generic;

public class MainCam : MonoBehaviour
{
    public static MainCam S;

	public GameObject playerObj;
    public int numLevels;

    public bool __________________;

	public static int level = 1;
	public static Dictionary<int, Vector3> startTable = new Dictionary<int, Vector3>();
	public static Dictionary<int, GameObject> levelTable = new Dictionary<int, GameObject>();

	void Awake()
	{
		S = this;

        for (int i = 1; i <= numLevels; i++)
        {
            levelTable[i] = GameObject.Find("Level_" + i);
        }

        for (int i = 2; i <= numLevels; i++)
            levelTable[i].SetActive(false);

        startTable[1] = new Vector3(1, 3, 0);
        startTable[2] = new Vector3(23, 3, 0);
        startTable[3] = new Vector3(55, 13, 0);
        startTable[4] = new Vector3(93, 18, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
		Transform t = playerObj.transform;
		Vector3 pos = new Vector3(t.position.x + 3 , t.position.y, -10f);
		transform.position = pos;

		if (Input.GetKey(KeyCode.R) && MainCam.level != 1) {
			Door.switchLevels(MainCam.level - 1);

			Events.Broadcast(new OnResetEvent());
		}
	}

    // Given an object in the Hierarchy, returns list of children filtered by tag. If T is GameObject,
    // will return the children GameObjects, but if T is something else, will return component T from
    // the children GameObjects.
    public static List<T> FilterByTag<T>(GameObject parent, string tag)
    {
        List<T> objects = new List<T>();
        foreach(Transform transform in parent.GetComponentsInChildren<Transform>())
        {
            if (transform.tag == tag)
            {
                if (typeof(T) == typeof(GameObject)) objects.Add((T)(object)transform.gameObject);
                else objects.Add(transform.gameObject.GetComponent<T>());
            }
        }
        return objects;
    }
}
