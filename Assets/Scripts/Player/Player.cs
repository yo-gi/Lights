using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player S;

    public Rigidbody2D r;

    Walk walk;

    public int maxHealth;
    public int regenRate;
    public float regenDelay;

    public bool __________________;

    public int direction = 1;

    public int health;
    private int regenStartHealth;
    private float lastDamage;
    private float regenStart;

    private Vector3 scale;

    public float HealthPercentage
    {
        get
        {
            return health / (float) maxHealth;
        }
    }

    void Awake()
    {
        S = this;
        r = gameObject.GetComponent<Rigidbody2D>();
        scale = transform.localScale;

        OnReset();
    }

    // Use this for initialization
    void Start()
    {
        walk = GetComponent<Walk>();

        walk.enabled = true;

        Events.Register<OnDeathEvent>(OnReset);
        Events.Register<OnPauseEvent>(OnPause);
    }

    void FixedUpdate()
    {
        float time = Time.time;
        if (time >= lastDamage + regenDelay)
        {
            if (regenStart <= 0.01f)
            {
                regenStart = time;
                regenStartHealth = health;
            }
            health = Mathf.Min(maxHealth, regenStartHealth + (int)((time - regenStart) * regenRate));
        }
    }

    public void TakeDamage(int damage)
    {
        lastDamage = Time.time;
        regenStart = 0;
        if (MainCam.S.invincible) return;
        MainCam.ShakeForSeconds(0.5f);
        health = Mathf.Max(0, health - damage);
        if (health <= 0)
        {
            Events.Broadcast(new OnDeathEvent());
        }
    }

    void OnPause(OnPauseEvent e) {
        if (e.paused) {
            walk.enabled = false;

            Pauser.Pause(this);
        }
        else {
            walk.enabled = true;

            Pauser.Resume(this);
        }
    }

    void OnReset()
    {
        health = maxHealth;
        lastDamage = 0;
        regenStart = 0;
        regenStartHealth = health;
        transform.localScale = scale;
    }
}











