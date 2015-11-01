using UnityEngine;
using System.Collections;

public class Navi : MonoBehaviour {

	public static Navi S;
	
	public float lerpTime;
	public float lerpSpeed;
	public float xOffset;
	public float yOffset;

	GameObject player;

	float startTime;
	Vector3 end;
	float length;

	// Set the luminosity. Accepts values from 0, 100.
	public int Luminosity {
		set {
			if (value < 0 || value > 100) return;

			// TODO
		}
	}

	void Awake() {
		Navi.S = this;
	}

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		InvokeRepeating("orbit", 0, lerpTime + 0.1f);
		end = player.transform.position;
	}

	void orbit() {
		startTime = Time.time;
		Vector3 p = player.transform.position;
		end = new Vector3(p.x + Random.Range(-1 * xOffset, xOffset), p.y + Random.Range(1, yOffset));
		length = Vector3.Distance(transform.position, end);
	}
	
	// Update is called once per frame
	void Update () {
		float distanceCovered = (Time.time - startTime) * lerpSpeed;
		float fracCovered = distanceCovered/length;
		transform.position = Vector3.Lerp(transform.position, end, fracCovered);
	}
}
