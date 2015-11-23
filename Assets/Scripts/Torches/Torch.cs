using UnityEngine;
using System.Collections.Generic;

public class OnTorchLitEvent {
    public Torch torch;

    public OnTorchLitEvent(Torch torch) { this.torch = torch; }
}

public class Torch : MonoBehaviour
{
    public List<TorchGroup> groups;

    GameObject flame;
	GameObject light;

    bool active;

    float activationRadius = 1f;

    // Use this for initialization
    void Start()
    {
        // Register the groups this torch belongs to.
        if (this.groups != null && this.groups.Count > 0) {
            Torches.Register(this);
        }

        flame = transform.Find("Flame").gameObject;
		flame.SetActive(false);
		light = transform.Find("Torchlight").gameObject;
		light.SetActive(false);
        active = false;

        Events.Register<OnResetEvent>(this.Reset);
    }

    void OnTriggerStay2D(Collider2D other) {
        bool isEnemy = (other.GetComponent<Enemy>() != null);

        if (active && isEnemy) {
            Destroy(other.gameObject);
        }
        else if (other.gameObject == Player.S.gameObject) {
            if (Vector3.Distance(transform.position, Player.S.transform.position) < activationRadius)
            {
                Activate();
            }
        }
    }

    private void Activate()
    {
        active = true;
        flame.SetActive(true);
		light.SetActive(true);

        Events.Broadcast(new OnTorchLitEvent(this));
    }

    private void Reset()
    {
        active = false;
        flame.SetActive(false);
    }
}
