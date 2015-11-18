using UnityEngine;
using System.Collections.Generic;

public enum Ability
{
    None,
    Dash,
    Teleport,
}

public class Altar : MonoBehaviour {

    public Ability ability;

    public bool ________________________;

    GameObject flame;
  	
    private static bool initialized = false;

    private static Dash dash;
    private static Teleport teleport;

	// Use this for initialization
	void Start ()
    {
        flame = this.transform.Find("Flame").gameObject;
        flame.SetActive(false);
        Events.Register<OnResetEvent>(()=> { Reset(); });

        // Get references to the different ability scripts
        if (initialized) return;
        dash = Player.S.gameObject.GetComponent<Dash>();
        dash.enabled = false;
        teleport = Player.S.gameObject.GetComponent<Teleport>();
        teleport.enabled = false;
        initialized = true;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject == Player.S.gameObject) {
			this.Activate();
		}
	}

    private void Activate()
    {
        flame.SetActive(true);
        ToggleAbility(ability, true);
    }

    private void Reset()
    {
        flame.SetActive(false);
        ToggleAbility(ability, false);
    }

    private void ToggleAbility(Ability ability, bool enabled)
    {
        switch(ability){
            case Ability.Dash:
                dash.enabled = enabled;
                break;
            case Ability.Teleport:
                teleport.enabled = enabled;
                break;
            default:
                break;
        }
    }
}
