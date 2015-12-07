using UnityEngine;
using System.Collections.Generic;

public class Rewind : MonoBehaviour, Rechargeable
{
    public static Rewind S;

    public GameObject rewindUI;

    private float maximumRewindInSeconds = 2f;
    private float rewindRate = 1f;

    // timeGap accounts for discontinuities in the timeline caused from rewinding
    private float timeGap;
    private float rewindStart;

    private float percentage;

    private LinkedList<Location> locationHistory = new LinkedList<Location>();

    private Object trailPrefab;
    private Object trail;
    LineRenderer line;

    public class Location
    {
        public float time;
        public Vector3 playerLocation;
        public Vector3 naviLocation;
        public Vector3 playerVelocity;
        public Quaternion playerRotation;
    }

    public int MaxCharges
    {
        get { return 1; }
    }

    public int Charges
    {
        get { return TeleportIsAvailable() ? 1 : 0; }
    }

    public bool Charging
    {
        get { return !TeleportIsAvailable(); }
    }

    public float ChargePercentage
    {
        get { return percentage; }
    }

    private bool Rewinding
    {
        get { return rewindStart != -maximumRewindInSeconds; }
        set
        {
            if (value == true) rewindStart = Time.time;
            else rewindStart = -maximumRewindInSeconds;
        }
    }

    void Awake()
    {
        S = this;

        trailPrefab = Resources.Load("Rewind Trail");
        line = ((GameObject)trailPrefab).GetComponent<LineRenderer>();

        Reset();
        Toggle(false);

        Events.Register<OnResetEvent>(Reset);
        Events.Register<OnDeathEvent>(Reset);
    }

    void Update()
    {
        if (ShouldRewind()) RewindTime();
        else UpdateLocationHistory(gameObject.transform.position);
        UpdateTrail();
    }

    private void ResetLocationHistory()
    {
        percentage = 0;
        locationHistory.Clear();
        CreateTrailObject();
    }

    private void CreateTrailObject()
    {
        Destroy(trail);
        GameObject newTrail = (GameObject)Instantiate(trailPrefab, transform.position, transform.rotation);
        line = newTrail.GetComponent<LineRenderer>();

        newTrail.transform.parent = transform;
        trail = newTrail;
    }

    private bool TeleportIsAvailable()
    {
        return locationHistory.Count > 0;
    }

    private bool ShouldRewind()
    {
        if (!TeleportIsAvailable()) return false;
        if (Rewinding) return Input.GetKey(Key.Rewind);
        return Input.GetKeyDown(Key.Rewind);
    }

    private void UpdateLocationHistory(Vector3 location)
    {
        if (Rewinding)
        {
            SetGap();
            Rewinding = false;
        }
        float currentTime = Time.time;
        locationHistory.AddLast(CreateLocation());
        percentage = Mathf.Min(1f, (currentTime - locationHistory.First.Value.time - timeGap) / maximumRewindInSeconds);

        float rewindTime = currentTime - maximumRewindInSeconds - timeGap;
        while (true)
        {
            Location first = locationHistory.First.Value;
            if (first.time < 0) timeGap += first.time;
            else if (first.time >= rewindTime) return;
            locationHistory.RemoveFirst();
        }
    }

    private void RewindTime()
    {
        // We can only rewind when there is a location history
        if (locationHistory.Count == 0) return;

        // If this is the first frame of rewinding, this must be updated
        if (!Rewinding)
        {
            // This will correct invariants
            Rewinding = true;
        }
        // Calculate the time we are rewinding to. We should rewind beyond locations above this maximum time.
        float currentTime = Time.time;
        float rewindTime = currentTime - rewindStart; // How long we've been rewinding
        float maxTime = rewindStart - (rewindRate * rewindTime);

        // Keep track of time gaps removed from the history and combine them
        float consumedGap = 0;

        while (locationHistory.Count > 0)
        {
            Location last = locationHistory.Last.Value;

            // If the last value is a gap, we must keep track of it
            if (last.time < 0)
            {
                consumedGap += last.time;
                // maxTime must also be updated to account for the time gap
                maxTime += last.time;
            }
            // The last value is an actual location
            else
            {
                // Move the player backwards in time
                SetLocation(last);
                // If last time <= maxTime, we must stop rewinding
                if (last.time <= maxTime)
                {
                    // Add a new gap entry if we have consumed any pre-existing gap entries
                    if (consumedGap < 0)
                    {
                        locationHistory.AddLast(CreateGapLocation(-consumedGap));
                    }
                    percentage = (last.time - locationHistory.First.Value.time - timeGap - consumedGap) / maximumRewindInSeconds;
                    return;
                }
            }

            locationHistory.RemoveLast();
        }

        // If there are no locations remaining, we cannot rewind;
        // first entry will be caught up in time (no gap)
        timeGap = 0;
        percentage = 0;
    }

    private void UpdateTrail()
    {
        line.SetVertexCount(locationHistory.Count);
        Vector3 vector = Vector3.zero;
        int i = 0;
        foreach (Location location in locationHistory)
        {
            if (location.time > 0) vector = location.playerLocation;
            line.SetPosition(i++, vector);
        }
    }

    private void SetGap()
    {
        float currentTime = Time.time;
        float elapsedTime = currentTime - rewindStart;
        float addedGap = (1 + rewindRate) * elapsedTime;
        locationHistory.AddLast(CreateGapLocation(addedGap));
        timeGap += addedGap;
    }

    private void Reset()
    {
        ResetLocationHistory();
        Rewinding = false;
        timeGap = 0;
    }

    private Location CreateLocation()
    {
        return new Location
        {
            time = Time.time,
            playerLocation = Player.S.transform.position,
            naviLocation = Navi.S.transform.position,
            playerVelocity = Player.S.r.velocity,
            playerRotation = Player.S.transform.rotation
        };
    }

    private Location CreateGapLocation(float gap)
    {
        return new Location
        {
            time = -gap,
            playerLocation = Vector3.zero,
            naviLocation = Vector3.zero,
            playerVelocity = Vector3.zero,
            playerRotation = new Quaternion()
        };
    }

    private void SetLocation(Location location)
    {
        Player.S.transform.position = location.playerLocation;
        Navi.S.transform.position = location.naviLocation;
        Player.S.r.velocity = location.playerVelocity;
        Player.S.transform.rotation = location.playerRotation;
    }

    public void Toggle(bool enable)
    {
        enabled = enable;
        rewindUI.SetActive(enable);
        if (enable) ResetLocationHistory();
        else Destroy(trail);
    }
}
