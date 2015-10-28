using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Bullets : MonoBehaviour {

	public static Dictionary<string, int> numBullets = new Dictionary<string, int>();

	public static string[] colors = new String[] {"white", "blue", "orange", "green"};

	// Use this for initialization
	void Start () {
		resetBullets();
	
	}

	public static void resetBullets() {
		foreach(string col in colors) {
			numBullets[col] = 0;
		}
		updateCanvas();
	}

	
	// Update is called once per frame
	void Update () {
	}

	public static void updateCanvas() {
		foreach (string col in colors) {
			if (col != "white")
				GameObject.Find(col + "Text").GetComponent<Text>().text = numBullets[col].ToString();
			GameObject button = GameObject.Find(col + "Button");
			button.transform.localScale = new Vector3(1f, 1f, 1f);
		}

		GameObject curButton = GameObject.Find(Player.S.color + "Button");
		curButton.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
	}

	public static void incrementColor(string col) {
		numBullets[col] += 1;
		updateCanvas();
	}

	public static void decrementColor(string col) {
		numBullets[col] -= 1;
		updateCanvas();
	}

	public static bool hasColor(string col) {
		return numBullets[col] > 0;
	}
}
