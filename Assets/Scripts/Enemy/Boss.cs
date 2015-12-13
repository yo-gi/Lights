using UnityEngine;
using LOS;

public enum BossState
{ 
	Waiting,
	Attacking,
    Chasing,
	Dying
}

public class Boss : MonoBehaviour
{
	public static Boss S;

	public BossState state;
	
	public float sightRange;
    public float attackSpeed;
	public GameObject projectile;

    bool __________________;

    public LOSRadialLight enemyLight;

    float nextAttackTime;
    float pauseTime;

	bool ___________________;
    
	public int health = 2;
	Enemy enemyComponent;

    public void Awake()
    {
		S = this;

        transform.GetComponent<Renderer>().sortingLayerName = "Default";
        transform.GetComponent<Renderer>().sortingOrder = 16;

        Events.Register<OnTorchGroupLitEvent>((e) => {
			if(e.group == TorchGroup.BossFight) // Trigger Stage Two
			{
				state = BossState.Chasing;
				enemyComponent.enabled = true;
				enemyComponent.emitSmoke();
				MainCam.ShakeForSeconds(2f);
				//state = BossState.Dying;
			}
		});

        Events.Register<OnPauseEvent>(OnPause);
    }

	public void Start()
	{
		state = BossState.Waiting;
		enemyComponent = GetComponent<Enemy>();
        enemyComponent.enabled = false;
	}

    // Update is called once per frame
    void Update()
	{
		if (state == BossState.Waiting) {
			//check if player is in sightrange and move to stealing if true
			if(Vector2.Distance(Navi.S.transform.position, transform.position) < sightRange)
			{
				state = BossState.Attacking;
			}
		}
		else if (state == BossState.Attacking) {
			//check if dead

			if(nextAttackTime < Time.time)
			{
				//spawn enemy
				GameObject proj = Instantiate(projectile);
				proj.transform.position = this.transform.position;
				nextAttackTime = Time.time + attackSpeed;
			}
        }
        else if (state == BossState.Chasing)
        {
            if (Vector2.Distance(Player.S.transform.position, transform.position) > 15f)
            {
                enemyComponent.followSpeed = 17f;
            }
            else
            {
                enemyComponent.followSpeed = 6f;
            }
        }
        if (state == BossState.Dying) {
			enemyComponent.die();
			Music.S.setDefaultMusic();
			MainCam.ShakeForSeconds(5f);
			Destroy(gameObject);
		}
    }

	public void takeDamage()
	{
		health -=1;
		attackSpeed = 10000f;
		transform.localScale *= 0.7f;
		enemyLight.radius *= 0.7f;

		if (health == 0) {
			state = BossState.Dying;
		}

	}

    void OnPause(OnPauseEvent e)
    {
        if (e.paused)
        {
            pauseTime = Time.time;
            Pauser.Pause(this);
        }
        else
        {
            nextAttackTime += Time.time - pauseTime;
            Pauser.Resume(this);
        }
    }
}