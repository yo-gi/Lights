using UnityEngine;
using System.Collections.Generic;
using LOS;
using System;

public class Navi : MonoBehaviour
{
    public static Navi S;

    public float deathThreshold;
    public float dampTime;
    public Vector3 speed;
    public float randThetaOffset;
    public float distanceOffset;
    public float followDist;
    public float followX;
    public float followY;

    public float maxLightRadius;
    
    public LOSRadialLight naviLight;
    public Dialog dialog;
    public ParticleSystem ps;

    public float moveFreq;
    public Vector3 targetPos;

    public bool ____________________;

    public float nextMovetime;

    float startTime;
    float length;

    Rigidbody2D rb;

    void Awake ()
    {
        S = this;
    }
    
    void Start ()
    {
        naviLight = GameObject.Find("Navi Light").GetComponent<LOSRadialLight>();
        dialog = GetComponent<Dialog>();
        ps = GameObject.Find("particle glow master").GetComponent<ParticleSystem>();

        Events.Register<OnPauseEvent>(OnPause);
        Events.Register<OnDeathEvent>(() => {
            Player.S.transform.position = Checkpoint.getClosestCheckpoint();
            resetNavi ();
        });

        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), Player.S.GetComponent<PolygonCollider2D>());

        nextMovetime = Time.time + moveFreq;
        targetPos = playerRelativePosition();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Vector3.Distance(transform.position, Player.S.transform.position) > 10f) {
            // Navi may have gotten stuck. Teleport navi back to the player.
            this.updatePosition();
        }
        else {
            if (Time.time > nextMovetime) {
                nextMovetime = Time.time + moveFreq;
                targetPos = playerRelativePosition();
            }
            Vector3.SmoothDamp(transform.position, targetPos + Player.S.gameObject.transform.position, ref speed, dampTime);
            rb.velocity = speed;
        }
        
        naviLight.radius = deathThreshold + (maxLightRadius - deathThreshold) * Player.S.HealthPercentage;
        ChangeColor(Player.S.naviColor);
    }

    void ChangeColor (Color color)
    {
        naviLight.color = color;
        ps.startColor = color;
    }

    public void resetNavi()
    {
        naviLight.radius = maxLightRadius;
        updatePosition();
    }

    Vector3 playerRelativePosition()
    {
        float theta = UnityEngine.Random.Range (-randThetaOffset, randThetaOffset) + (float)Math.PI/2;
        return new Vector3 ((float)((followDist + UnityEngine.Random.Range (-distanceOffset, distanceOffset)) * Math.Cos (theta)),
                            (float)((followDist + UnityEngine.Random.Range (-distanceOffset, distanceOffset)) * Math.Sin (theta)));
    }

    public void updatePosition()
    {
        transform.position = Player.S.transform.position;
    }

    void OnPause(OnPauseEvent e) {
        if (e.paused) {
            //Pauser.Pause(this.spriteObject);
            Pauser.Pause(this);
        }
        else {
            Pauser.Resume(this);
            //Pauser.Resume(this.spriteObject);
        }
    }
}
