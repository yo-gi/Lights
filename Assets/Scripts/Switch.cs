using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	public Color color;

	// Use this for initialization
	void Start () {
		SpriteRenderer s = GetComponent<SpriteRenderer>();
		s.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject.name == "Player") {
			//Player.S.switchColors(gameObject.tag);
			Bullets.incrementColor(gameObject.tag);
			deactivate();
		}
	}

	public void activate() {
		PolygonCollider2D p = GetComponent<PolygonCollider2D>();
		p.enabled = true;
		SpriteRenderer s = GetComponent<SpriteRenderer>();
		s.enabled = true;

	}

	void deactivate() {
		PolygonCollider2D p = GetComponent<PolygonCollider2D>();
		p.enabled = false;
		SpriteRenderer s = GetComponent<SpriteRenderer>();
		s.enabled = false;
	}
}
