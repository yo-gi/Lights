using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	float attackSpeed;
	int attackDamage;

	float range;
	float sightRange;

	bool __________________;

	public RaycastHit hitInfo;
	
	bool attacking;
	float nextAttackTime;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (attacking) {
			if(Time.time > nextAttackTime)
			{
				//player is in range
				if(Vector3.Distance (Player.S.transform.position, transform.position) < range)
				{
					Navi.S.Luminosity -= attackDamage;
				}
				else
				{
					//attack missed
					nextAttackTime = Time.time + attackSpeed;
				}
			}
		} else {
			//if player is in range
			if(Vector3.Distance (Player.S.transform.position, transform.position) < range)
			{
				attacking = true;
				nextAttackTime = Time.time + attackSpeed;
			}
			//move at the player
			else if(Vector3.Distance (Player.S.transform.position, transform.position) < sightRange)
			{
				
			}
		}
	}
}
