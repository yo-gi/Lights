﻿using UnityEngine;
using LOS;

public enum BossState
{ 
	Waiting,
	Stealing,
	Attacking,
	Dying
}

public class Boss : MonoBehaviour
{
	public static Boss S;

	public BossState state;
	
	public float sightRange;
	public float naviStolenRange;
    public float attackSpeed;
	public GameObject projectile;

	public Vector3 naviStolenPos;

    bool __________________;

    public LOSRadialLight enemyLight;

    float nextAttackTime;

    public void Awake()
    {
		S = this;
    }

	public void Start()
	{
		state = BossState.Waiting;
	}

    // Update is called once per frame
    void Update()
	{
		if (state == BossState.Waiting) {
			//check if player is in sightrange and move to stealing if true
			if(Vector2.Distance(Navi.S.transform.position, transform.position) < sightRange)
			{
				state = BossState.Stealing;
				Navi.S.stolen = true;
			}
		}
		else if (state == BossState.Stealing) {
			print ("Boss stealing!");
			if(Vector2.Distance(Navi.S.transform.position, transform.position) < naviStolenRange)
			{
				state = BossState.Attacking;
				nextAttackTime = Time.time + attackSpeed;
			}
		}
		else if (state == BossState.Attacking) {
			//check if dead
			if(nextAttackTime < Time.time)
			{
				print ("Boss attacking!");
				//spawn enemy
				GameObject proj = Instantiate(projectile);
				proj.transform.position = this.transform.position;
				BossProjectile script = proj.GetComponent<BossProjectile>();
				script.targetPos = Player.S.transform.position + 4*(Player.S.transform.position - transform.position);
				nextAttackTime = Time.time + attackSpeed;
			}
		}
    }
}