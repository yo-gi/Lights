using UnityEngine;

public class Drown : MonoBehaviour
{
    public float timeToDrown = 5;
    private float drowningStartTime;

    public void Start()
    {
        drowningStartTime = -timeToDrown;
        Events.Register<OnResetEvent>(()=> { Drowning = false; });
    }

    public void Update()
    {
        if (!Drowning)
        {
            // TODO: Raise light intensity if it isn't at max.
            if (IsSubmergedInWater())
            {
                Drowning = true;
            }
        }
        else
        {
            // No longer drowning
            if (IsSubmergedInWater() == false)
            {
                // TODO: Start increasing light intensity.
                Drowning = false;
                return;
            }
            // Still drowning
            float drownTime = Time.time - drowningStartTime;
            if (drownTime >= timeToDrown)
            {
                MainCam.RestartLevel();
            }
        }
    }

    private GameObject CurrentBodyOfWater
    {
        get
        {
            return Player.S.water;
        }
    }

    private bool Drowning
    {
        get
        {
            return drowningStartTime != -timeToDrown;
        }
        set
        {
            if (value)
            {
                drowningStartTime = Time.time;
            }
            else
            {
                drowningStartTime = -timeToDrown;
            }
        }
    }

    private bool IsSubmergedInWater()
    {
        GameObject water = CurrentBodyOfWater;
        if (water == null)
        {
            return false;
        }
        float waterLevel = water.transform.position.y;
        // 90% of height
        float topOfPlayer = transform.position.y + GetHeightFromTransform(transform) * 0.4f;

        return topOfPlayer < waterLevel;
    }

    private float GetHeightFromTransform(Transform transform)
    {
        return transform.GetComponent<Renderer>().bounds.size.y;
    }
}
