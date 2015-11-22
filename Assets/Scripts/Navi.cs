using UnityEngine;
using LOS;

public class Navi : MonoBehaviour
{
	public static Navi S;
	
	public float dampTime;
	public Vector3 speed;
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

	Rigidbody2D rb;

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

		Events.Register<OnDeathEvent>(() => {
			Player.S.transform.position = Checkpoint.latestCheckpoint;
			resetNavi ();
		});

		rb = GetComponent<Rigidbody2D>();
		Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), Player.S.GetComponent<PolygonCollider2D>());
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
	
	// Update is called once per frame
	void Update ()
	{
		Vector3.SmoothDamp(transform.position, playerRelativePosition(), ref speed, dampTime);
		rb.velocity = speed;
		
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

	public void resetNavi()
	{
		naviLight.radius = maxLightRadius;
		updatePosition();
	}

	Vector3 playerRelativePosition()
	{
		Vector3 player = Player.S.gameObject.transform.position;
		return new Vector3 (player.x + followX + Random.Range (-randXOffset, randXOffset), player.y + followY + Random.Range (-randYOffset, randYOffset));
	}

	public void updatePosition()
	{
		transform.position = Player.S.transform.position;
	}
}
