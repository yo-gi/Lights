using UnityEngine;
using LOS;

public class Enemy : MonoBehaviour
{
    public float attackSpeed;
    public float attackDamage;

    public float range;
    public float sightRange;

    public float runSpeed;

    bool __________________;

    public RaycastHit hitInfo;
    public LOSRadialLight enemyLight;

    bool attacking = false;
    float nextAttackTime;

    Rigidbody2D r;

    public void Awake()
    {
        r = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            if (Time.time > nextAttackTime)
            {
                //player is in range
                if (Vector2.Distance(Player.S.transform.position, transform.position) < range)
                {
                    Navi.S.naviLight.radius -= attackDamage;
                    nextAttackTime = Time.time + attackSpeed;
                }
                else
                {
                    attacking = false;
                    if (Player.S.transform.position.x > transform.position.x)
                    {
                        r.velocity = new Vector2(runSpeed, 0);
                    }
                    else
                    {
                        r.velocity = new Vector2(-runSpeed, 0);
                    }
                }
            }
        }
        else
        {
            //if player is in range
            if (Vector2.Distance(Player.S.transform.position, transform.position) < range)
            {
                attacking = true;
                nextAttackTime = Time.time + attackSpeed;
                r.velocity = new Vector2(0, 0);
            }
            //move at the player
            else if (Vector2.Distance(Player.S.transform.position, transform.position) < sightRange)
            {
                //print ("saw player!");
                if (Player.S.transform.position.x > transform.position.x)
                {
                    r.velocity = new Vector2(runSpeed, 0);
                }
                else
                {
                    r.velocity = new Vector2(-runSpeed, 0);
                }
            }
        }
    }
}
