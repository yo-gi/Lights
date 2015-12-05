using UnityEngine;

public class Teleport : MonoBehaviour, Rechargeable
{
    public static Teleport S;

    public GameObject teleportUI;

    private float maxTeleportDistance = 3f;

    private int maxCharges = 2;
    private int currentCharges;
    private float cooldown = 2f;
    private float rechargeTime = 0f;
    private float lastCharge;

    private Rigidbody2D r;

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

        Events.Register<OnResetEvent>(Reset);
    }

    void Update()
    {
        UpdateCharges();

        if (CanDash() && Input.GetKeyDown(Key.Teleport))
        {
            if (!Charging)
            {
                Charging = true;
            }
            currentCharges -= 1;

            var dashVector = GetDashVector();
            var velocity = r.velocity;

            velocity.x = dashVector.x;
            velocity.y = dashVector.y;

            r.velocity = velocity;

            gameObject.transform.position += dashVector;
			Navi.S.updatePosition();
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

    private bool CanDash()
    {
        return currentCharges > 0;
    }

    private Vector3 GetDashVector()
    {
        var dashDirection = GetDashDirection();
        var dashDistance = GetDashDistance(dashDirection);

        return dashDirection.normalized * dashDistance;
    }

    private Vector3 GetDashDirection()
    {
        var direction = Vector3.zero;

        if (Input.GetKey(Key.Jump)) direction.y += 1;
        if (Input.GetKey(Key.Down)) direction.y -= 1;
        if (Input.GetKey(Key.Left)) direction.x -= 1;
        if (Input.GetKey(Key.Right)) direction.x += 1;

        return direction;
    }

    private float GetDashDistance(Vector3 direction)
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

    private void Reset()
    {
        currentCharges = maxCharges;
        Charging = false;
    }

    public void Toggle(bool enable)
    {
		this.enabled = enable;
        teleportUI.SetActive(enable);
    }
}
