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
		timeRemaining = Time.time + burntime;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeRemaining > Time.time && Time.time > nextTick && Player.S.color != LightColor.Red) {
			//change limin
			print("fire tick!");
			nextTick = Time.time + burnRate;
		}
	}
}
