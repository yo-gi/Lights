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
    GameObject torchLight;

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
        torchLight = transform.Find("Torchlight").gameObject;
        torchLight.SetActive(false);
        active = false;

        Events.Register<OnResetEvent>(this.Reset);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Destroy enemies that enter torche's light.
        if (active == false) return;

        if (other.GetComponent<Enemy>() != null || other.GetComponent<BossProjectile>()) {
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
        flame.SetActive(true);
        torchLight.SetActive(true);

        Events.Broadcast(new OnTorchLitEvent(this));
    }

    private void Reset()
    {
        active = false;
        flame.SetActive(false);
    }
}
