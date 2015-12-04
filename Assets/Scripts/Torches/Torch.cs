using UnityEngine;
using System.Collections.Generic;

public class Torch : MonoBehaviour
{
    public static int count = 0;
    public static int activated = 0;

    public List<TorchGroup> groups;

    GameObject flame;
    GameObject torchLight;

    bool active;

    float activationRadius = 1f;
    
    void Awake()
    {
        ++count;
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
        ++activated;
        flame.SetActive(true);
        torchLight.SetActive(true);

        Events.Broadcast(new OnTorchLitEvent { torch = this });
    }

    private void Reset()
    {
        active = false;
        flame.SetActive(false);
        --activated;
    }
}
