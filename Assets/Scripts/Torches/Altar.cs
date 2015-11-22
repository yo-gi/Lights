using UnityEngine;
using System.Collections.Generic;

public enum Ability
{
    None,
    Dash,
    Teleport,
    DoubleJump,
}

public class Altar : MonoBehaviour {

    public Ability ability;

    public bool ________________________;

    GameObject flame;
  	
    private static bool initialized = false;
    private static Dictionary<int, List<Altar>> altars;
    private static Dictionary<int, int> activeAltarCounts;

    private bool active;

    private static Walk walk;
    private static Dash dash;
    private static Teleport teleport;
    
	void Start ()
    {
        active = false;

        flame = transform.Find("Flame").gameObject;
        flame.SetActive(false);

        // Get references to the different ability scripts
        if (initialized) return;
        walk = Walk.S;
        dash = Dash.S;
        teleport = Teleport.S;
        altars = new Dictionary<int, List<Altar>>();
        activeAltarCounts = new Dictionary<int, int>();
        foreach (KeyValuePair<int, GameObject> entry in MainCam.levelTable)
        {
            // If the level is not active, we need to reactivate it in order to find Torch children
            bool levelActive = entry.Value.activeInHierarchy;
            if (!levelActive) entry.Value.SetActive(true);
            altars.Add(entry.Key, MainCam.FilterByTag<Altar>(entry.Value, "Altar"));
            activeAltarCounts.Add(entry.Key, 0);
            if (!levelActive) entry.Value.SetActive(false);
        }
        Events.Register<OnResetEvent>(() =>
        {
            foreach (Altar altar in altars[MainCam.currentLevel])
            {
                altar.Reset();
            }
            activeAltarCounts[MainCam.currentLevel] = 0;
        });
        Events.Register<OnLevelLoadEvent>((e) => {
            if (altars[e.level].Count == 0)
            {
                Events.Broadcast(new OnAltarsActivatedEvent());
            }
        });
        initialized = true;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject == Player.S.gameObject) {
			Activate();
		}
	}

    private void Activate()
    {
        if (active) return;
        flame.SetActive(true);
        ToggleAbility(ability, true);
        int currentLevel = MainCam.currentLevel;
        activeAltarCounts[currentLevel] += 1;
        if (activeAltarCounts[currentLevel] == altars[currentLevel].Count)
        {
            Events.Broadcast(new OnAltarsActivatedEvent());
        }
        active = true;
    }

    private void Reset()
    {
        flame.SetActive(false);
        ToggleAbility(ability, false);
        active = false;
    }

    private void ToggleAbility(Ability ability, bool enabled)
    {
        switch(ability){
            case Ability.Dash:
                dash.Toggle(enabled);
                break;
            case Ability.Teleport:
                teleport.Toggle(enabled);
                break;
            case Ability.DoubleJump:
                walk.ToggleDoubleJump(enabled);
                break;
            default:
                break;
        }
    }
}
