using UnityEngine;
using LOS;

public enum EnemyState
{ 
	Patrolling,
	Following
}

public class Enemy : MonoBehaviour
{
    public float attackSpeed;
    public int attackDamage;

    public float attackRange;
    public float sightRange;

    public float runSpeed;
	public float followSpeed;

	public bool canFollow = false;
	Vector2 patrolStart;
	public Vector2 patrolEnd;

	public GameObject deathSmoke;

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
		patrolStart = transform.position;
		Events.Register<OnDeathEvent>(() => {
			transform.position = patrolStart;
		});
	}
    // Update is called once per frame
    void FixedUpdate()
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
				if (Vector2.Distance(Player.S.transform.position, transform.position) < sightRange)
					state = EnemyState.Following;
			}
		} else if (state == EnemyState.Following) {
			transform.position = Vector2.MoveTowards(transform.position, Player.S.transform.position, followSpeed * Time.deltaTime);
			if (Vector2.Distance(Player.S.transform.position, transform.position) > sightRange)
				state = EnemyState.Patrolling;
		}
		if (Vector2.Distance(Player.S.transform.position, transform.position) < attackRange) {
			if (Time.time > nextAttackTime) {
				Player.S.takeDamage(attackDamage);
				nextAttackTime = Time.time + attackSpeed;
			}
		}
    }

	public void die() {
		emitSmoke();
		MainCam.ShakeForSeconds(0.2f);
		Destroy(this.gameObject);
	}

	public void emitSmoke() {
		Instantiate(deathSmoke, transform.position, transform.rotation);
	}
}
