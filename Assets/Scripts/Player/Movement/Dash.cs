using UnityEngine;

public class Dash : MonoBehaviour, Rechargeable
{
    public static Dash S;

    public GameObject dashUI;

    private float maxDashDistance = 3f;

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

        if (CanDash() && Input.GetKeyDown(Key.Dash))
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
        // Find all the walls in the dash's path.
        var start = gameObject.transform.position;
        var mask = (1 << LayerMask.NameToLayer("Terrain"));

        var hits = Physics2D.RaycastAll(start, direction, maxDashDistance, mask);

        // Return the max dash distance if there are no walls.
        if (hits.Length == 0) return maxDashDistance;

        // Raycast backwards to find the end point of the wall.
        var lastHit = hits[hits.Length - 1];
        var end = start + direction * maxDashDistance;

        var reverseHit = Physics2D.Raycast(end, -1 * direction, maxDashDistance, mask);
        var reverseHitPoint = reverseHit.point;

        if (Vector3.Distance(start, reverseHitPoint) < maxDashDistance)
        {
            // The reverse hitpoint is within the dash's range. Use the max dash distance for the dash.
            return maxDashDistance;
        }
        else
        {
            // The reverse hitpoint is within the dash's range. Teleport to the last wall's hit point.
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
        dashUI.SetActive(enable);
    }
}
