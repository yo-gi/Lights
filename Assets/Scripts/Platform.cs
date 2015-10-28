using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	float startTime;
	static float lifeTime = 3f;
	Color startColor;
	Color endColor;

	public bool started = false;

	// Use this for initialization
	void Start () {
		reset ();
	}

	void Awake() {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnEnable() {
	}

	void reset() {
		lifeTime = Player.S.lifeTime;
		Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), Player.S.gameObject.GetComponent<PolygonCollider2D>());
		//GetComponent<PolygonCollider2D>().enabled = false;
		SpriteRenderer s = GetComponent<SpriteRenderer>();
		s.color = new Color(s.color.r, s.color.g, s.color.b, 39f/255f);
		started = false;
	}

	void FixedUpdate() {
		if (started) {
			SpriteRenderer s = GetComponent<SpriteRenderer>();
			s.color = Color.Lerp(startColor, endColor, (Time.time - startTime)/(lifeTime));
			if (s.color == endColor) {
				reset ();
			}
		} else {
			reset ();
		}
	}

	public void activate() {
		startTime = Time.time;
		GetComponent<PolygonCollider2D>().enabled = true;
		SpriteRenderer s = GetComponent<SpriteRenderer>();
		s.color = new Color(s.color.r, s.color.g, s.color.b, 255f/255f);
		startColor = s.color;
		endColor = new Color(s.color.r, s.color.g, s.color.b, 39f/255f);
		started = true;
		Physics2D.IgnoreCollision(gameObject.GetComponent<PolygonCollider2D>(), Player.S.gameObject.GetComponent<PolygonCollider2D>(), false);
	}
}
