using UnityEngine;
using System.Collections.Generic;
using LOS;
using System;
using System.Collections;

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

    public float startingLightRadius;
    public float finalLightRadius;

    //GameObject spriteObject;
    //SpriteRenderer sprite;
    public LOSRadialLight naviLight;
    public Dialog dialog;

    public List<Color> colors;
    public Color waterColor;

    public float moveFreq;
    public Vector3 targetPos;

    public bool ____________________;

    public float nextMovetime;

    public bool stolen = false;

    public Color currentColor;
    private int currentColorIndex;

    public float maxLightRadius;

    float startTime;
    float length;

    Rigidbody2D rb;

    void Awake ()
    {
        S = this;

        maxLightRadius = startingLightRadius;
        currentColorIndex = 0;
        currentColor = colors[currentColorIndex];
    }
    
    void Start ()
    {
        //spriteObject = gameObject.transform.Find ("LightSprite").gameObject;
        //sprite = spriteObject.GetComponent<SpriteRenderer> ();
        naviLight = GameObject.Find("Navi Light").GetComponent<LOSRadialLight>();
        dialog = GetComponent<Dialog>();

        ChangeColor(currentColor);

        Events.Register<OnPauseEvent>(OnPause);
        Events.Register<OnDeathEvent>(() => {
            Player.S.transform.position = Checkpoint.getClosestCheckpoint();
            resetNavi ();
        });

        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), Player.S.GetComponent<PolygonCollider2D>());

        Events.Register<OnTorchLitEvent>(OnTorchLit);
        Events.Register<OnAltarLitEvent>(OnAltarLit);
        nextMovetime = Time.time + moveFreq;
        targetPos = playerRelativePosition();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "water") {
            ChangeColor(waterColor);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.tag == "water") {
            ChangeColor(currentColor);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (stolen) {
            Vector3.SmoothDamp(transform.position, Boss.S.naviStolenPos, ref speed, dampTime);
            rb.velocity = speed;
            naviLight.radius = deathThreshold + (maxLightRadius - deathThreshold) * Player.S.HealthPercentage * Swim.S.BreathPercentage;
            return;
        }

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

    public void ChangeColor (Color color)
    {
        //sprite.color = color;
        naviLight.color = color;
    }

    public void resetNavi()
    {
        naviLight.radius = maxLightRadius;
        updatePosition();
    }

    Vector3 playerRelativePosition()
    {
        Vector3 player = Player.S.gameObject.transform.position;
        float theta = UnityEngine.Random.Range (-randThetaOffset, randThetaOffset) + (float)Math.PI/2;
        return new Vector3 ((float)((followDist + UnityEngine.Random.Range (-distanceOffset, distanceOffset)) * Math.Cos (theta)),
                            (float)((followDist + UnityEngine.Random.Range (-distanceOffset, distanceOffset)) * Math.Sin (theta)));
    }

    public void updatePosition()
    {
        if (stolen == false) {
            transform.position = Player.S.transform.position;
        }
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

    void OnTorchLit(OnTorchLitEvent e)
    {
        int lowerBound = Torch.count * currentColorIndex / (colors.Count - 1);
        int upperBound = Torch.count * (currentColorIndex + 1) / (colors.Count - 1);
        int range = upperBound - lowerBound;
        float percentage = (Torch.activated - lowerBound) / (float)range;

        currentColor = Color.Lerp(colors[currentColorIndex], colors[currentColorIndex + 1], percentage);

        if(Torch.activated == upperBound)
        {
            ++currentColorIndex;
        }

        ChangeColor(currentColor);
    }

    void OnAltarLit()
    {
        // TODO: make change LERP
        float fraction = Altar.activated / (float)Altar.count;
        maxLightRadius = (finalLightRadius - startingLightRadius) * fraction + startingLightRadius;
    }
}
