using UnityEngine;

public class Swim : MonoBehaviour
{
    public float swimSpeed;
    public float swimGravity;

    public float hurtRate;
    public float hurtamount;

    public bool ________________;

    public float nextTick;

    private Rigidbody2D rigidBody;

    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        rigidBody.gravityScale = swimGravity;
        nextTick = Time.time + hurtRate;
    }

    public void OnDisable()
    {
        rigidBody.gravityScale = 1f;
    }

    public void Update()
    {
        if (Navi.S.naviLight.radius <= 1 && !MainCam.S.invincible)
        {
            //StopPlayer();
        }
        else
        {
            UpdatePosition();
        }
    }

    public void FixedUpdate()
    {
        rigidBody.angularVelocity *= 0.95f;
    }

    public void StopPlayer()
    {
        rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, new Vector2(0, rigidBody.velocity.y), 0.05f);
    }

    private void UpdatePosition()
    {
        //handle navi getting hurt
        if (Time.time > nextTick)
        {
            nextTick = Time.time + hurtRate;
			Navi.S.takeDamage(hurtamount);
        }

        var velocity = rigidBody.velocity;

        // Lateral swimming.
        if (Input.GetKey(Key.Left) && Input.GetKey(Key.Right) == false)
        {
            Debug.Log("Left");
            velocity.x = -1f * swimSpeed;
        }
        else if (Input.GetKey(Key.Right) && Input.GetKey(Key.Left) == false)
        {
            Debug.Log("Left");
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

        // TODO: handle collisions.

        rigidBody.velocity = velocity;
    }
}
