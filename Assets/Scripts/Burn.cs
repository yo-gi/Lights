using UnityEngine;

public class Burn : MonoBehaviour
{

    public static Burn S;

    public float burnRate;

    public bool ________________;

    public float timeRemaining = 0;
    public float nextTick;

    void Awake()
    {
        S = this;
    }

    public void setBurning(float burntime)
    {
        timeRemaining = Time.time + burntime;
    }

    void Update()
    {
        if (timeRemaining > Time.time)
        {
            timeRemaining = 0;
            MainCam.Reset();
        }
    }
}
