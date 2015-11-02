using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Bullets : MonoBehaviour {

	public static Dictionary<LightColor, int> numBullets = new Dictionary<LightColor, int>();

	public static LightColor[] colors = new LightColor[] {LightColor.White, LightColor.Blue, LightColor.Red, LightColor.Yellow};

	// Use this for initialization
	void Start () {
		resetBullets();
	
	}

	public static void resetBullets() {
		foreach(LightColor col in colors) {
			numBullets[col] = 0;
		}
		//updateCanvas();
	}

	
	// Update is called once per frame
	void Update () {
	}

	public static void updateCanvas() {
		foreach (LightColor col in colors) {
			if (col != LightColor.White)
				GameObject.Find(MainCam.S.colortoString(col) + "Text").GetComponent<Text>().text = numBullets[col].ToString();
			print (MainCam.S.colortoString(col) + "Button");
			GameObject button = GameObject.Find(MainCam.S.colortoString(col) + "Button");
			button.transform.localScale = new Vector3(1f, 1f, 1f);
		}

		GameObject curButton = GameObject.Find(MainCam.S.colortoString(Player.S.color) + "Button");
		curButton.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
	}

	public static void incrementColor(LightColor col) {
		numBullets[col] += 1;
		//updateCanvas();
	}

	public static void decrementColor(LightColor col) {
		numBullets[col] -= 1;
		//updateCanvas();
	}

	public static bool hasColor(LightColor col) {
		print (MainCam.S.colortoString (col));
		return numBullets[col] > 0;
	}
}
