using UnityEngine;

public class Swim : MonoBehaviour {

	public float swimSpeed;
	public float swimGravity;
	
	public float hurtRate;
	public float hurtamount;

	
	public bool ________________;

	public float nextTick;

	private Rigidbody2D rigidBody;
	
	public void Awake() {
		this.rigidBody = this.GetComponent<Rigidbody2D>();
	}
	
	public void Start() {
		this.rigidBody.gravityScale = this.swimGravity;
		nextTick = Time.time + hurtRate;
	}

	public void OnDisable() {
		this.rigidBody.gravityScale = 1f;
	}
	
	public void Update() {
		if (Navi.S.naviLight.radius <= 0 && !MainCam.S.invincible) {
			this.StopPlayer();
		}
		else {
			this.UpdatePosition();
		}
	}

	public void FixedUpdate()
	{
		this.rigidBody.angularVelocity *= 0.95f;
	}

	public void StopPlayer() {
		this.rigidBody.velocity = Vector2.Lerp(this.rigidBody.velocity, new Vector2(0, this.rigidBody.velocity.y), 0.05f);
	}

	private void UpdatePosition() {
		//handle navi getting hurt
		if (Time.time > nextTick) {
			nextTick = Time.time + hurtRate;
			Navi.S.naviLight.radius -= hurtamount;
		}

		var velocity = this.rigidBody.velocity;
		
		// Lateral swimming.
		if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) == false) {
			velocity.x = -1f * this.swimSpeed;
		}
		else if (Input.GetKey(KeyCode.D) && Input.GetKey (KeyCode.A) == false) {
			velocity.x = this.swimSpeed;
		}
		
		// Up/down swimming.
		if (Input.GetKey(KeyCode.W) && Input.GetKey (KeyCode.S) == false) {
			velocity.y = this.swimSpeed;
		}
		else if (Input.GetKey(KeyCode.S) && Input.GetKey (KeyCode.W) == false) {
			velocity.y = -1f * this.swimSpeed - this.swimGravity;
		}
		
		// TODO: handle collisions.
		
		this.rigidBody.velocity = velocity;
	}
}
