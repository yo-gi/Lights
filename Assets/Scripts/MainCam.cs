using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MainCam : MonoBehaviour {

	public GameObject playerObj;
	public int numLevels;

	public static int level = 5;

	public static Dictionary<int, Vector3> startTable = new Dictionary<int, Vector3>();
	public static Dictionary<int, GameObject> levelTable = new Dictionary<int, GameObject>();

	// Use this for initialization
	void Start () {

		for (int i = 1; i <= numLevels; i++) {
			levelTable[i] = GameObject.Find("Level_" + i);
			print (levelTable[i].name);
		}

		for (int i = 2; i <= numLevels; i++)
			levelTable[i].SetActive(false);

		startTable[1] = new Vector3(1,3,0);
		startTable[2] = new Vector3(23,3,0);
		startTable[3] = new Vector3(55,3,0);
		startTable[4] = new Vector3(93,18,0);
		startTable[5] = new Vector3(134,7,0);
	}
	
	// Update is called once per frame
	void Update () {
		Transform t = playerObj.transform;
		Vector3 pos = new Vector3(t.position.x + 3 , t.position.y, -10f);
		transform.position = pos;
	}
}
