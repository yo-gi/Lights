using UnityEngine;
using System.Collections;

public class Burn : MonoBehaviour {

	public static Burn S;

	public float burnRate;

	public bool ________________;

	public float timeRemaining = 0;
	public float nextTick;

	void Awake()
	{
		S = this;
	}

	public void setBurning(float burntime)
	{
		if (Player.S.color == LightColor.Red) return;

//		Door.switchLevels(MainCam.level == 1 ? 5 : MainCam.level - 1);
		timeRemaining = Time.time + burntime;
	}
	
	// Update is called once per frame
	void Update () {
//		if (timeRemaining > Time.time && Time.time > nextTick && Player.S.color != LightColor.Red) {
//			//change limin
//			Navi.S.Luminosity -= 1;
//			print ("fire tick: " + Navi.S.Luminosity);
//			nextTick = Time.time + burnRate;
//			timeRemaining = Time.time;
//
//		} else if(timeRemaining < Time.time) {
//			Navi.S.Luminosity++;
//			if(Navi.S.Luminosity > 100)
//			{
//				Navi.S.Luminosity = 100;
//			}
//		}

		if (timeRemaining > Time.time && Player.S.color != LightColor.Red) {
			Door.switchLevels(MainCam.level == 1 ? 5 : MainCam.level - 1);

			timeRemaining = 0;
		}
	}
}
