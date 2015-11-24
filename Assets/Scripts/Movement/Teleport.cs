using UnityEngine;
using System.Collections.Generic;

public class Teleport : MonoBehaviour, Rechargeable
{
    public static Teleport S;

    public GameObject teleportUI;

    private float teleportBackInSeconds = 2f;
    private float cooldown = 2f;

    private float lastTeleport;
    private float rechargeTime;
    private Queue<Location> locationHistory = new Queue<Location>();

    private Object trailPrefab;
    private Object trail;

    public class Location
    {
        public Vector3 vector;
        public float time;
    }

    public int MaxCharges
    {
        get
        {
            return 1;
        }
    }

    public int Charges
    {
        get
        {
            return TeleportIsAvailable() ? 1 : 0;
        }
    }

    public bool Charging
    {
        get
        {
            return !TeleportIsAvailable();
        }
    }

    public float ChargePercentage
    {
        get
        {
            return Mathf.Min(rechargeTime / cooldown, 1f);
        }
    }

    void Awake()
    {
        S = this;

        trailPrefab = Resources.Load("Teleport Trail");
        ((GameObject)trailPrefab).GetComponent<TrailRenderer>().time = cooldown;

        Reset();
        Toggle(false);

        Events.Register<OnResetEvent>(Reset);
        Events.Register<OnLevelCompleteEvent>(ResetLocationHistory);
    }

    void Update()
    {
        rechargeTime = Time.time - lastTeleport;
        if (TeleportIsAvailable() && Input.GetKeyDown(Key.Teleport))
        {
            gameObject.transform.position = GetTeleportVector();
			Navi.S.updatePosition();

            ResetLocationHistory();
        }

        UpdateLocationHistory(gameObject.transform.position);
    }

    private void ResetLocationHistory()
    {
        lastTeleport = Time.time;

        locationHistory.Clear();
        CreateTrailObject();
    }

    private void CreateTrailObject()
    {
        Destroy(trail);
        GameObject newTrail = (GameObject)Instantiate(trailPrefab, transform.position, transform.rotation);

        newTrail.transform.parent = transform;
        trail = newTrail;
    }

    private bool TeleportIsAvailable()
    {
        return (rechargeTime >= cooldown);
    }

    private Vector3 GetTeleportVector()
    {
        float goal = Time.time - teleportBackInSeconds;

        while (true)
        {
            Location location = locationHistory.Dequeue();
            if (location.time >= goal)
            {
                return location.vector;
            }
        }
    }

	public Vector3 GetTeleportLocation()
	{
		return locationHistory.Peek().vector;
	}

    private void UpdateLocationHistory(Vector3 location)
    {
        float currentTime = Time.time;
        locationHistory.Enqueue(new Location()
        {
            vector = gameObject.transform.position,
            time = currentTime,
        });

        float rewindTime = currentTime - teleportBackInSeconds;
        while (locationHistory.Peek().time < rewindTime) locationHistory.Dequeue();
    }

    private void Reset()
    {
        ResetLocationHistory();
        lastTeleport = -cooldown;
        rechargeTime = cooldown;
    }

    public void Toggle(bool enable)
    {
        this.enabled = enable;
        teleportUI.SetActive(enable);
        if (enable)
        {
            ResetLocationHistory();
            lastTeleport = -cooldown;
        }
        else
        {
            Destroy(trail);
        }
    }
}
