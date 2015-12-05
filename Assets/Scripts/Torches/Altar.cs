using UnityEngine;

public enum Ability
{
    None,
    Teleport,
    Rewind,
    DoubleJump,
}

public class Altar : MonoBehaviour {

    public static int count = 0;
    public static int activated = 0;

    public Ability ability;

    public bool ________________________;

	GameObject flame;
	GameObject altarLight;

    private bool active;

    private static Walk walk;
    private static Teleport teleport;
    private static Rewind rewind;
    
	void Start ()
    {
        ++count;
        active = false;

        flame = transform.Find("Flame").gameObject;
		flame.SetActive(false);
		altarLight = transform.Find("Torchlight").gameObject;
		altarLight.SetActive(false);

        // Get references to the different ability scripts
        walk = Walk.S;
        teleport = Teleport.S;
        rewind = Rewind.S;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject == Player.S.gameObject) {
			Activate();
		}
	}

    private void Activate()
    {
        if (active) return;
        ++activated;
        flame.SetActive(true);
		altarLight.SetActive(true);
        ToggleAbility(ability, true);
        active = true;

        Events.Broadcast(new OnAltarLitEvent { });
    }

    private void Reset()
    {
        --activated;
        flame.SetActive(false);
        ToggleAbility(ability, false);
        active = false;
    }

    private void ToggleAbility(Ability ability, bool enabled)
    {
        switch(ability){
            case Ability.Teleport:
                teleport.Toggle(enabled);
                break;
            case Ability.Rewind:
                rewind.Toggle(enabled);
                break;
            case Ability.DoubleJump:
                walk.ToggleDoubleJump(enabled);
                break;
            default:
                break;
        }
    }
}
