using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour, Rechargeable
{
    public static Teleport S;

    public GameObject teleportUI;
	public GameObject dashIndicator;

    public AudioClip sound;

    private float maxTeleportDistance = 3f;

    private bool activated = false;
    private int maxCharges = 2;
    private int currentCharges;
    private float cooldown = 2f;
    private float rechargeTime = 0f;
    private float lastCharge;
    private bool teleporting;

    private Rigidbody2D r;

    private GameObject poof;
    private Walk walk;
    private AudioSource teleportAudio;

    public int MaxCharges
    {
        get
        {
            return maxCharges;
        }
    }

    public int Charges
    {
        get
        {
            return currentCharges;
        }
    }

    public bool Charging
    {
        get
        {
            return currentCharges < maxCharges;
        }
        private set
        {
            if (value)
            {
                lastCharge = Time.time;
            }
            else
            {
                lastCharge = -cooldown;
            }
        }
    }

    public float ChargePercentage
    {
        get
        {
            return Charging ? rechargeTime / cooldown : 1f;
        }
    }

    void Awake()
    {
        S = this;

        Reset();
        Toggle(false);

        r = GetComponent<Rigidbody2D>();
        walk = gameObject.GetComponent<Walk>();
        teleportAudio = GetComponent<AudioSource>();
        teleportAudio.volume = 0.6f;

		dashIndicator = GameObject.Find("Dash Indicator");
        poof = (GameObject)Resources.Load("Poof");

        dashIndicator.SetActive(false);

        Events.Register<OnResetEvent>(Reset);
        Events.Register<OnPauseEvent>(Pause);
    }

    void Update()
    {
        if (activated == false) return;

        var teleportVector = GetTeleportVector();

        UpdateCharges();
        UpdateIndicator(teleportVector);

        if (CanTeleport() && Input.GetKeyDown(Key.Teleport) && teleportVector != Vector3.zero)
        {
            teleportAudio.clip = this.sound;
            teleportAudio.Play();

            ActivateTeleport(teleportVector);
        }
    }

    private void UpdateCharges()
    {
        rechargeTime = Time.time - lastCharge;
        if (Charging && ChargePercentage >= 1)
        {
            ++currentCharges;
            if (currentCharges < maxCharges)
            {
                Charging = true;
            }
            else
            {
                Charging = false;
            }
        }
    }

    private void UpdateIndicator(Vector3 teleportVector)
    {
        var hit = Physics2D.Raycast(transform.position, teleportVector, maxTeleportDistance, 1 << LayerMask.NameToLayer("Terrain"));
        var indicatorPos = Player.S.transform.position + teleportVector;
        var indicatorPos2 = new Vector2(indicatorPos.x, indicatorPos.y);

        if (hit.collider != null && hit.point != indicatorPos2) {
            dashIndicator.SetActive(true);
            dashIndicator.transform.position = Vector3.Lerp(dashIndicator.transform.position, indicatorPos, 0.5f);
        }
        else {
            dashIndicator.transform.position = indicatorPos;
            dashIndicator.SetActive(false);
        }
    }

    private bool CanTeleport()
    {
        return !teleporting && currentCharges > 0;
    }

    private Vector3 GetTeleportDirection()
    {
        var direction = Vector3.zero;

        if (Input.GetKey(Key.Jump)) direction.y += 1;
        if (Input.GetKey(Key.Down)) direction.y -= 1;
        if (Input.GetKey(Key.Left)) direction.x -= 1;
        if (Input.GetKey(Key.Right)) direction.x += 1;

        return direction;
    }

    private Vector3 GetTeleportVector()
    {
        var direction = GetTeleportDirection();
        var distance = GetTeleportDistance(direction);
        return direction * distance;
    }


    private float GetTeleportDistance(Vector3 direction)
    {
        // Find all the walls in the teleport's path.
        var start = gameObject.transform.position;
        var mask = (1 << LayerMask.NameToLayer("Terrain"));

        var hits = Physics2D.RaycastAll(start, direction, maxTeleportDistance, mask);

        // Return the max teleport distance if there are no walls.
        if (hits.Length == 0) return maxTeleportDistance;

        // Raycast backwards to find the end point of the wall.
        var lastHit = hits[hits.Length - 1];
        var end = start + direction * maxTeleportDistance;

        var reverseHit = Physics2D.Raycast(end, -1 * direction, maxTeleportDistance, mask);
        var reverseHitPoint = reverseHit.point;

        if (Vector3.Distance(start, reverseHitPoint) < maxTeleportDistance)
        {
            // The reverse hitpoint is within the teleport's range. Use the max teleport distance for the teleport.
            return maxTeleportDistance;
        }
        else
        {
            // The reverse hitpoint is within the teleport's range. Teleport to the last wall's hit point.
            return lastHit.distance;
        }
    }

    private void ActivateTeleport(Vector3 teleportVector)
    {
        if (!Charging)
        {
            Charging = true;
        }
        currentCharges -= 1;

        StartCoroutine(Vanish(teleportVector));
    }

    IEnumerator Vanish(Vector3 teleportVec)
    {
        teleporting = true;
        walk.enabled = false;

        float duration = 0.1f;

        // Vanish phase
        float endTime = Time.time + duration;
        Vector3 scaleVec = transform.localScale;

        Instantiate(poof, transform.position, transform.rotation);
        while (Time.time <= endTime)
        {
            float factor = (endTime - Time.time) / duration;
            transform.localScale = scaleVec * factor;
            r.velocity = Vector2.zero;
            yield return null;
        }

        // Reappear phase
        gameObject.transform.position += teleportVec;
        Navi.S.updatePosition();
        endTime = Time.time + duration;

        Instantiate(poof, transform.position, transform.rotation);
        while (Time.time <= endTime)
        {
            float factor = (Time.time + duration - endTime) / duration;
            transform.localScale = scaleVec * factor;
            r.velocity = Vector2.zero;
            yield return null;
        }
        transform.localScale = scaleVec;

        // Only enable walking if we're not in a cutscene.
        if (Cutscene.current == null) {
            r.velocity = teleportVec;

            walk.enabled = true;
        }

        teleporting = false;
    }

    private void Reset()
    {
        currentCharges = maxCharges;
        Charging = false;
        teleporting = false;
    }

    private void Pause(OnPauseEvent e)
    {
        activated = ! e.paused;
    }

    public void Toggle(bool enable)
    {
        activated = enable;
        enabled = enable;
        teleportUI.SetActive(enable);
    }
}
