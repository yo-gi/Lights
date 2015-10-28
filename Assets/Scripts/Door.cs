using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Vector3 teleportTo;
	public int level;
	public float triggerDistance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (Vector3.Distance(transform.position, Player.S.transform.position) < triggerDistance) {
				switchLevels (level);
			}
		}
	}

	public static void switchLevels(int curLevel) {
		if (curLevel == 5) {
			MainCam.levelTable[curLevel].SetActive(false);
			MainCam.levelTable[1].SetActive(true);
			MainCam.level = 1;
			Player.S.transform.position = MainCam.startTable[1];
			Player.S.switchColors(Player.S.color);
			return;
		}
		MainCam.levelTable[curLevel].SetActive(false);
		MainCam.levelTable[curLevel+1].SetActive(true);
		MainCam.level = curLevel + 1;
		Player.S.transform.position = MainCam.startTable[curLevel+1];
		Player.S.switchColors(Player.S.color);
	}
}
