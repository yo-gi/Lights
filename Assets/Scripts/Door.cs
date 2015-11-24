using UnityEngine;

public class Door : MonoBehaviour
{
    public float triggerDistance;

    private int level;

    private bool locked;
    private bool altarsCleared;
    private bool torchesCleared;

    void Awake()
    {
        level = int.Parse(transform.parent.name.Substring(6));
        Reset();
        Events.Register<OnAltarsActivatedEvent>(() =>
        {
            if (MainCam.currentLevel != level) return;
            altarsCleared = true;
            if (torchesCleared) Unlock();
        });
        Events.Register<OnTorchesLitEvent>(() =>
        {
            if (MainCam.currentLevel != level) return;
            torchesCleared = true;
            if (altarsCleared) Unlock();
        });
        Events.Register<OnResetEvent>(() =>
        {
            Reset();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && Input.GetKeyDown(Key.Activate))
        {
            if (Vector3.Distance(transform.position, Player.S.transform.position) < triggerDistance)
            {
                MainCam.NextLevel();
            }
        }
    }

    void Unlock()
    {
        locked = false;
        Events.Broadcast(new OnLevelCompleteEvent());
    }

    void Reset()
    {
        locked = true;
        altarsCleared = false;
        torchesCleared = false;
    }
}
