using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float runSpeed;
	public float jumpHeight;
	public float friction;
	public GameObject bombPrefab;
	public float lifeTime;

	public static Player S;
	public string color = "white";
	Material lightMaterial;
	bool doubleJump = false;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		lightMaterial = GameObject.Find("Navi").GetComponent<DynamicLight>().lightMaterial;
	}

	void OnCollisionEnter2D(Collision2D c) {
	}

	void OnCollisionExit2D(Collision2D c) {
	}
	
	// Update is called once per frame
	void Update () {
		/* Movement */
		Rigidbody2D r = GetComponent<Rigidbody2D>();
		if (Input.GetKey(KeyCode.A)) {
			r.velocity = new Vector2(-1f * runSpeed, r.velocity.y);
			//r.AddForce(new Vector2((-1f * runSpeed) + -1f * r.velocity.x, 0), ForceMode2D.Force);
		}
		if (Input.GetKey(KeyCode.D)) {
			r.velocity = new Vector2(runSpeed, r.velocity.y);
			//r.AddForce(new Vector2(runSpeed - r.velocity.x, 0), ForceMode2D.Force);
		}
		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
			r.velocity = new Vector2(0, r.velocity.y);
		}
		if (Input.GetKeyDown(KeyCode.W)) {
			if (doubleJump) {
				r.velocity = new Vector2(r.velocity.x, jumpHeight);
				doubleJump = false;
				print ("false");
			}
			RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -Vector2.up, 0.45f, ~(1 << 10));
			foreach(RaycastHit2D hit in hits) {
				if (hit.collider != null) {
					Platform p = hit.collider.GetComponent<Platform>();
					if (p == null || p.started) {
						r.velocity = new Vector2(r.velocity.x, jumpHeight);
						doubleJump = true;
						print ("true");
						break;
					}
				}
			}

		}

		/* Colors */
		if (Input.GetKey(KeyCode.Alpha1)) {
			switchColors("white");
		} else if (Input.GetKey(KeyCode.Alpha2)) {
			switchColors ("orange");
		} else if (Input.GetKey(KeyCode.Alpha3)) {
			switchColors("blue");
		} else if (Input.GetKey(KeyCode.Alpha4)) {
			switchColors("green");
		}

		if (Input.GetKey(KeyCode.R) && MainCam.level != 1) {
			Door.switchLevels(MainCam.level - 1);
			foreach (string col in Bullets.colors) {
				if (col == "white") continue;
				GameObject[] switches = GameObject.FindGameObjectsWithTag(col);
				foreach (GameObject o in switches) {
					if (o.GetComponent<Rotate>() != null) {
						o.GetComponent<Switch>().activate();
					}
				}
			}
			Bullets.resetBullets();
		}

		/* Light Bomb */
		if (Input.GetMouseButtonDown(0)) {
			if (Bullets.hasColor(color)) {
				Bullets.decrementColor(color);
				Vector3 spawn = new Vector3(transform.position.x, transform.position.y, transform.position.z);
				GameObject newBomb = (GameObject)Instantiate(bombPrefab, spawn, transform.rotation);
				newBomb.GetComponent<Bomb>().destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
		}
	}

	void toggleObjectsWithTag(string tag, bool toggle) {
		GameObject[] oldColorObjects = GameObject.FindGameObjectsWithTag(tag);
		foreach (GameObject obj in oldColorObjects) {
			obj.GetComponent<PolygonCollider2D>().enabled = toggle;
			SpriteRenderer s = obj.GetComponent<SpriteRenderer>();
			if (toggle) {
				Color old = s.color;
				Color newC = new Color(old.r, old.g, old.b, 255f/255f);
				s.color = newC;
			} else {
				Color old = s.color;
				Color newC = new Color(old.r, old.g, old.b, 10f/255f);
				s.color = newC;
			}
		}
	}
	
	public void switchColors(string newColor) {
		string oldColor = color;
		color = newColor;

		/*if (oldColor != "white")
			toggleObjectsWithTag(oldColor, false);
		if (color != "white")
			toggleObjectsWithTag(color, true);
		*/
		switch(color) {
		case "orange":
			lightMaterial.color = new Color(229/255f, 149/255f, 0f, 1f);
			break;
		case "white":
			lightMaterial.color = Color.white;
			break;
		case "blue":
			lightMaterial.color = new Color(83/255f, 161/255f, 200/255f, 1f);
			break;
		case "green":
			lightMaterial.color = new Color(93f/255f, 238f/255f, 142f/255f, 1f);
			break;
		default:
			break;
		}
		Bullets.updateCanvas();
	}

	void OnApplicationQuit() {
		lightMaterial.color = Color.white;
	}
}











