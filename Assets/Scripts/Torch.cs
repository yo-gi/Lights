using UnityEngine;
using System.Collections.Generic;

public class Torch : MonoBehaviour
{
    GameObject flame;

    bool active;

    float activationRadius = 1f;

    private static bool initialized = false;
    private static Dictionary<int, List<Torch>> torches;
    private static Dictionary<int, int> activeTorchCounts;

    public int TorchCount
    {
        get
        {
            return torches[MainCam.currentLevel].Count;
        }
    }

    // Use this for initialization
    void Start()
    {
        flame = transform.Find("Flame").gameObject;
        flame.SetActive(false);
        active = false;

        // Initialize torches dictionary
        if (initialized) return;
        torches = new Dictionary<int, List<Torch>>();
        activeTorchCounts = new Dictionary<int, int>();
        foreach (KeyValuePair<int, GameObject> entry in MainCam.levelTable)
        {
            // If the level is not active, we need to reactivate it in order to find Torch children
            bool levelActive = entry.Value.activeInHierarchy;
            if (!levelActive) entry.Value.SetActive(true);
            torches.Add(entry.Key, MainCam.FilterByTag<Torch>(entry.Value, "Torch"));
            activeTorchCounts.Add(entry.Key, 0);
            if (!levelActive) entry.Value.SetActive(false);
        }
        Events.Register<OnResetEvent>(() =>
        {
            foreach (Torch torch in torches[MainCam.currentLevel])
            {
                torch.Reset();
            }
            activeTorchCounts[MainCam.currentLevel] = 0;
        });
        Events.Register<OnLevelLoadEvent>((e) => {
            if (torches[e.level].Count == 0)
            {
                Events.Broadcast(new OnTorchesLitEvent());
            }
        });
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active) return;
        if (Vector3.Distance(transform.position, Player.S.transform.position) < activationRadius)
        {
            Activate();
        }
    }

    private void Activate()
    {
        active = true;
        flame.SetActive(true);
        int currentLevel = MainCam.currentLevel;
        activeTorchCounts[currentLevel] += 1;
        if (activeTorchCounts[currentLevel] == torches[currentLevel].Count)
        {
            Events.Broadcast(new OnTorchesLitEvent());
        }
    }

    private void Reset()
    {
        active = false;
        flame.SetActive(false);
    }
}
