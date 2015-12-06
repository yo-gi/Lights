using UnityEngine;
using System.Collections.Generic;
using LOS;

public class Torch : MonoBehaviour
{
    public static int count = 0;
    public static int activated = 0;
	public float radius;
	public float maxRadius;

	public float recoveryAmount;
	public float recoveryFreq;
	public float nextRecoveryTime;

    public List<TorchGroup> groups;

    GameObject flame;
    public GameObject torchLight;

    public bool active;

    float activationRadius = 1f;
    
    void Awake()
    {
        ++count;
    }

	public void takeDamage(float damage)
	{
		radius -= damage;
		GetComponent<CircleCollider2D> ().radius = radius;
		torchLight.GetComponent<LOSRadialLight> ().radius = radius;
	}

    void Start()
    {
        // Register the groups this torch belongs to.
        if (this.groups != null && this.groups.Count > 0) {
            Torches.Register(this);
        }

        flame = transform.Find("Flame").gameObject;
        flame.SetActive(false);
        torchLight = transform.Find("Torchlight").gameObject;
        torchLight.SetActive(false);
        active = false;

        Events.Register<OnResetEvent>(this.Reset);
    }

    void OnTriggerEnter2D(Collider2D other) {
		print (other.name);
        // Destroy enemies that enter torche's light.
        if (active == false) return;

        /*if (other.GetComponent<Enemy>() != null || other.GetComponent<BossProjectile>()) {
			print ("torched in torch!");
            Destroy(other.gameObject);
        }*/
    }

    void OnTriggerStay2D(Collider2D other) {
        // Light up the torch when the player is near it.
        if (active) return;

        if (other.gameObject == Player.S.gameObject) {
            if (Vector3.Distance(transform.position, Player.S.transform.position) < activationRadius) {
                Activate();
            }
        }
    }

    private void Activate()
    {
        active = true;
        ++activated;
        flame.SetActive(true);
        torchLight.SetActive(true);
		radius = 2.5f;

        Events.Broadcast(new OnTorchLitEvent { torch = this });
    }

    private void Reset()
    {
        active = false;
        flame.SetActive(false);
        --activated;
    }

	void Update()
	{
		if (!active)
			return;
		if (Time.time > nextRecoveryTime) {
			nextRecoveryTime = Time.time + recoveryFreq;
			radius += recoveryAmount;
			if(radius > maxRadius)
			{
				radius = maxRadius;
			}
		}
		if (radius < 1f) {
			active = false;
			--activated;
			flame.SetActive(false);
			radius = 0.0001f;
			Events.Broadcast(new OnTorchUnlitEvent{torch = this});
		}
		GetComponent<CircleCollider2D> ().radius = radius;
		torchLight.GetComponent<LOSRadialLight> ().radius = radius;
	}
}
