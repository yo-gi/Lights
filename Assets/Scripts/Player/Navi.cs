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

    //public Color waterColor;

    public float moveFreq;
    public Vector3 targetPos;

    public bool ____________________;

    public float nextMovetime;

    //public Color currentColor;

    float startTime;
    float length;

    Rigidbody2D rb;

    void Awake ()
    {
        S = this;
    }
    
    void Start ()
    {
        //spriteObject = gameObject.transform.Find ("LightSprite").gameObject;
        //sprite = spriteObject.GetComponent<SpriteRenderer> ();
        naviLight = GameObject.Find("Navi Light").GetComponent<LOSRadialLight>();
        dialog = GetComponent<Dialog>();

        //ChangeColor(currentColor);

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

    //void OnTriggerEnter2D(Collider2D collider) {
    //    if (collider.tag == "water") {
    //        ChangeColor(waterColor);
    //    }
    //}

    //void OnTriggerExit2D(Collider2D collider) {
    //    if (collider.tag == "water") {
    //        ChangeColor(currentColor);
    //    }
    //}

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


        naviLight.radius = deathThreshold + (maxLightRadius - deathThreshold) * Player.S.HealthPercentage * Swim.S.BreathPercentage;
    }

    //public void ChangeColor (Color color)
    //{
    //    //sprite.color = color;
    //    naviLight.color = color;
    //}

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
