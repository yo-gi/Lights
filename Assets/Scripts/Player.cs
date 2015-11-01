using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public GameObject bombPrefab;
	public float lifeTime;

	public LightColor color;


	public static Player S;
	Material lightMaterial;
	Walk walk;
	Swim swim;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		lightMaterial = GameObject.Find("Navi").GetComponent<DynamicLight>().lightMaterial;
		walk = GetComponent<Walk>();
		swim = GetComponent<Swim>();

		// TODO: This will not work if player starts in water.
		walk.enabled = true;
		swim.enabled = false;
	}

	void OnCollisionEnter2D(Collision2D c) {
	}

	void OnCollisionExit2D(Collision2D c) {
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "water") {
			this.walk.enabled = false;
			this.swim.enabled = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "water") {
			this.walk.enabled = true;
			this.swim.enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {
		/* Colors */
		if (Input.GetKey(KeyCode.Alpha1)) {
			switchColors(LightColor.White);
		} else if (Input.GetKey(KeyCode.Alpha2)) {
			switchColors (LightColor.Red);
		} else if (Input.GetKey(KeyCode.Alpha3)) {
			switchColors(LightColor.Blue);
		} else if (Input.GetKey(KeyCode.Alpha4)) {
			switchColors(LightColor.Yellow);
		}

		if (Input.GetKey(KeyCode.R) && MainCam.level != 1) {
			Door.switchLevels(MainCam.level - 1);
			foreach (LightColor col in Bullets.colors) {
				if (col == LightColor.White) continue;
				GameObject[] switches = GameObject.FindGameObjectsWithTag(MainCam.S.colortoString(col));
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

	/*void toggleObjectsWithTag(string tag, bool toggle) {
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
	}*/
	
	public void switchColors(LightColor newColor) {
		LightColor oldColor = color;
		color = newColor;

		/*if (oldColor != "white")
			toggleObjectsWithTag(oldColor, false);
		if (color != "white")
			toggleObjectsWithTag(color, true);
		*/
		switch(color) {
		case LightColor.Red:
			lightMaterial.color = new Color(229/255f, 149/255f, 0f, 1f);
			break;
		case LightColor.White:
			lightMaterial.color = Color.white;
			break;
		case LightColor.Blue:
			lightMaterial.color = new Color(83/255f, 161/255f, 200/255f, 1f);
			break;
		case LightColor.Yellow:
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











