using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MainCam : MonoBehaviour {

	public static MainCam S;

	public GameObject playerObj;
	public int numLevels;

	public static int level = 1;

	public static Dictionary<int, Vector3> startTable = new Dictionary<int, Vector3>();
	public static Dictionary<int, GameObject> levelTable = new Dictionary<int, GameObject>();

	void Awake()
	{
		S = this;
	}

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
	
	public string colortoString(LightColor color)
	{
		switch (color) {
		case LightColor.Blue:
			return "blue";
		case LightColor.Red:
			return "red";
		case LightColor.Yellow:
			return "yellow";
		default:
			return "white";
		}
	}
	
	public LightColor getColorfromString(string tag)
	{
		switch(tag)
		{
		case "red":
			return LightColor.Red;
		case "blue":
			return LightColor.Blue;
		case "yellow":
			return LightColor.Yellow;
		default:
			return LightColor.White;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Transform t = playerObj.transform;
		Vector3 pos = new Vector3(t.position.x + 3 , t.position.y, -10f);
		transform.position = pos;
	}
}
