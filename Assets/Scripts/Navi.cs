using UnityEngine;
using LOS;

public class Navi : MonoBehaviour
{
	public static Navi S;
	
	public float lerpTime;
	public float lerpSpeed;
	public float randXOffset;
	public float randYOffset;
	public float followX;
	public float followY;

	public float maxLightRadius;
	public float recoveryRate;
	public float recoveryAmount;
	float nextRecoveryTime = 0;

	SpriteRenderer sprite;
    public LOSRadialLight naviLight;

	public Color defaultColor;
	public Color waterColor;

    float startTime;
	Vector3 end;
	float length;

	void Awake ()
	{
		S = this;
        naviLight.color = defaultColor;
	}
    
	void Start ()
	{
		sprite = gameObject.transform.Find ("LightSprite").GetComponent<SpriteRenderer> ();
		naviLight = GameObject.Find("Navi Light").GetComponent<LOSRadialLight>();
        sprite.color = naviLight.color;
		end = Player.S.gameObject.transform.position;
		InvokeRepeating ("orbit", 0, lerpTime);

		Events.Register<OnDeathEvent>(() => {
			Player.S.transform.position = Checkpoint.latestCheckpoint;
			naviLight.radius = maxLightRadius;
		});
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "water") {
			ChangeColor(waterColor);
		}
	}
	
	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "water") {
			ChangeColor(defaultColor);
		}
	}

	void orbit ()
	{
		startTime = Time.time;
		Vector3 p = Player.S.gameObject.transform.position;
		end = new Vector3 (p.x + followX + Random.Range (-randXOffset, randXOffset), p.y + followY + Random.Range (-randYOffset, randYOffset));
		length = Vector3.Distance (transform.position, end);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float distanceCovered = (Time.time - startTime) * lerpSpeed;
		float fracCovered = distanceCovered / length;
		transform.position = Vector3.Lerp (transform.position, end, fracCovered);
		if (naviLight.radius < maxLightRadius)
		{
			if(nextRecoveryTime < Time.time)
			{
				naviLight.radius += recoveryAmount;
				nextRecoveryTime = Time.time + recoveryRate;
			}
		}

		if (naviLight.radius <= 0) {
			Events.Broadcast(new OnDeathEvent { });
		}
	}

	public void ChangeColor (Color color)
	{
		sprite.color = color;
		naviLight.color = color;
	}
}
