﻿using UnityEngine;
using LOS;

public enum EnemyState
{ 
	Patrolling,
	Following
}

public class Enemy : MonoBehaviour
{
    public float attackSpeed;
    public float attackDamage;

    public float attackRange;
    public float sightRange;

    public float runSpeed;
	public float followSpeed;

	public bool canFollow = false;
	public Vector2 patrolStart;
	public Vector2 patrolEnd;

    bool __________________;

    public RaycastHit hitInfo;
    public LOSRadialLight enemyLight;

	public EnemyState state = EnemyState.Patrolling;
    float nextAttackTime;

    public void Awake()
    {

    }

	public void Start()
	{
		Events.Register<OnDeathEvent>(() => {
			transform.position = patrolStart;
		});
	}
    // Update is called once per frame
    void Update()
    {
		if (state == EnemyState.Patrolling) {
			// Reverse direction if at destination
			if ((Vector2)transform.position == patrolEnd) {
				Vector3 temp = patrolEnd;
				patrolEnd = patrolStart;
				patrolStart = temp;
			}

			transform.position = Vector2.MoveTowards(transform.position, patrolEnd, runSpeed * Time.deltaTime);

			if (canFollow) {
				if (Vector2.Distance(Navi.S.transform.position, transform.position) < sightRange)
					state = EnemyState.Following;
			}
		} else if (state == EnemyState.Following) {
			transform.position = Vector2.MoveTowards(transform.position, Navi.S.transform.position, followSpeed * Time.deltaTime);
			if (Vector2.Distance(Navi.S.transform.position, transform.position) > sightRange)
				state = EnemyState.Patrolling;
		}

		if (Vector2.Distance(Navi.S.transform.position, transform.position) < attackRange) {
			if (Time.time > nextAttackTime) {
				Navi.S.naviLight.radius -= attackDamage;
				nextAttackTime = Time.time + attackSpeed;
			}
		}
    }
}