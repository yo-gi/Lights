using UnityEngine;
using System.Collections.Generic;

public enum Ability
{
    DoubleJump,
    Dash,
    Teleport,
}

public class Altar : MonoBehaviour {

    public Ability ability;

    public bool ________________________;

    GameObject flame;
    bool active;

    float activationRadius = 2f;

    private static bool initialized = false;

    private static DoubleJump doubleJump;
    private static Dash dash;
    private static Teleport teleport;

	// Use this for initialization
	void Start ()
    {
        flame = this.transform.Find("Flame").gameObject;
        flame.SetActive(false);
        active = false;
        Events.Register<OnResetEvent>(()=> { Reset(); });

        // Get references to the different ability scripts
        if (initialized) return;
        doubleJump = Player.S.gameObject.GetComponent<DoubleJump>();
        doubleJump.enabled = false;
        dash = Player.S.gameObject.GetComponent<Dash>();
        dash.enabled = false;
        teleport = Player.S.gameObject.GetComponent<Teleport>();
        teleport.enabled = false;
        initialized = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (active) return;
        if (Input.GetKeyDown(KeyCode.Space) &&
            Vector3.Distance(transform.position, Player.S.transform.position) < activationRadius)
        {
            Activate();
        }
    }

    private void Activate()
    {
        active = true;
        flame.SetActive(true);
        ToggleAbility(ability, true);
    }

    private void Reset()
    {
        active = false;
        flame.SetActive(false);
        ToggleAbility(ability, false);
    }

    private void ToggleAbility(Ability ability, bool enabled)
    {
        switch(ability){
            case Ability.DoubleJump:
                doubleJump.enabled = enabled;
                break;
            case Ability.Dash:
                dash.enabled = enabled;
                break;
            case Ability.Teleport:
                teleport.enabled = enabled;
                break;
            default:
                print("Unrecognized Ability.");
                break;
        }
    }
}
