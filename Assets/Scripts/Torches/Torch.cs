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

            // if (this.groups.Contains(TorchGroup.BossFight)) {
            //     Events.Register<OnDeathEvent>(this.Reset);
            // }
        }

        flame = transform.Find("Flame").gameObject;
        flame.SetActive(false);
        torchLight = transform.Find("Torchlight").gameObject;
        torchLight.SetActive(false);
        active = false;

        Events.Register<OnResetEvent>(this.Reset);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Destroy enemies that enter torche's light.
        if (active == false) return;
        if (other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy>().die();
		} else if (other.GetComponent<BossProjectile>() != null) {
			Destroy(other.gameObject);
		}
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
        Debug.Log("Resetting torch");

        active = false;
        flame.SetActive(false);
        --activated;
        radius = 0.0001f;

        GetComponent<CircleCollider2D> ().radius = radius;
        torchLight.GetComponent<LOSRadialLight> ().radius = radius;

        Events.Broadcast(new OnTorchUnlitEvent{torch = this});
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

            GetComponent<CircleCollider2D> ().radius = radius;
            torchLight.GetComponent<LOSRadialLight> ().radius = radius;
		}
		if (radius < 1f) {
            this.Reset();
		}
	}
}
