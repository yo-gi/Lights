using UnityEngine;

public class Swim : MonoBehaviour
{
    public static Swim S;

    public float swimSpeed;
    public float swimGravity;

    public float breathInSeconds;
    public float breathRegenTime; // Amount of time to fully recover from no breath

    public bool ________________;

    private bool active = true;

    private float breathLeftInSeconds;
    private float lastDrownUpdate;

    private Rigidbody2D rigidBody;
    private float originalGravityScale;

    public float BreathPercentage
    {
        get
        {
            return breathLeftInSeconds / breathInSeconds;
        }
    }

    void Awake()
    {
        S = this;
        rigidBody = GetComponent<Rigidbody2D>();
        originalGravityScale = rigidBody.gravityScale;
        OnReset();
        Events.Register<OnResetEvent>(OnReset);
        Events.Register<OnDeathEvent>(OnReset);
    }

    public void OnDisable()
    {
        rigidBody.gravityScale = 1f;
    }

    public void Update()
    {
        if (!MainCam.S.invincible)
        {
            UpdateDrowning();
            if (BreathPercentage <= 0)
            {
                Events.Broadcast(new OnDeathEvent());
            }
        }
    }

    public void FixedUpdate()
    {
        if (!active) return;
        rigidBody.angularVelocity *= 0.95f;
        UpdateVelocity();
    }

    public void StopPlayer()
    {
        rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, new Vector2(0, rigidBody.velocity.y), 0.05f);
    }

    private void UpdateDrowning()
    {
        float currentTime = Time.time;
        if (IsSubmergedInWater())
        {
            if (lastDrownUpdate <= 0)
            {
                lastDrownUpdate = currentTime;
            }
            breathLeftInSeconds -= currentTime - lastDrownUpdate;
            breathLeftInSeconds = Mathf.Max(0, breathLeftInSeconds);
            lastDrownUpdate = currentTime;
        }
        else
        {
            if (lastDrownUpdate >= 0)
            {
                lastDrownUpdate = -currentTime;
            }
            float regeneratedBreath = (currentTime + lastDrownUpdate) * breathInSeconds / breathRegenTime;
            breathLeftInSeconds = Mathf.Min(breathInSeconds, breathLeftInSeconds + regeneratedBreath);
            lastDrownUpdate = -currentTime;
        }
    }

    private void UpdateVelocity()
    {
        var velocity = rigidBody.velocity;
        if (IsSubmergedInWater())
        {
            velocity.x *= 0.95f;
        }
        else
        {
            velocity.x = 0;
        }

        // Lateral swimming.
        if (Input.GetKey(Key.Left) && Input.GetKey(Key.Right) == false)
        {
            velocity.x = -1f * swimSpeed;
        }
        else if (Input.GetKey(Key.Right) && Input.GetKey(Key.Left) == false)
        {
            velocity.x = swimSpeed;
        }

        // Up/down swimming.
        if (Input.GetKey(Key.Up) && Input.GetKey(Key.Down) == false)
        {
            velocity.y = swimSpeed;
        }
        else if (Input.GetKey(Key.Down) && Input.GetKey(Key.Up) == false)
        {
            velocity.y = -1f * swimSpeed - swimGravity;
        }

        rigidBody.velocity = velocity;
    }

    private GameObject CurrentBodyOfWater
    {
        get
        {
            return Player.S.water;
        }
    }

    private bool IsSubmergedInWater()
    {
        GameObject water = CurrentBodyOfWater;
        if (water == null)
        {
            return false;
        }
        float waterLevel = water.transform.position.y;
        
        // 90% of height
        float topOfPlayer = transform.position.y + GetHeightFromTransform(transform) * 0.4f;

        return topOfPlayer < waterLevel;
    }

    private float GetHeightFromTransform(Transform transform)
    {
        return transform.GetComponent<Renderer>().bounds.size.y;
    }

    public void Enable(bool enable)
    {
        active = enable;
        if (enable) rigidBody.gravityScale = swimGravity;
        else rigidBody.gravityScale = originalGravityScale;
    }

    private void OnReset()
    {
        breathLeftInSeconds = breathInSeconds;
        lastDrownUpdate = 0;
    }
}
