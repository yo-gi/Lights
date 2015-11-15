using UnityEngine;

public class DoubleJump : MonoBehaviour {

    private bool canDoubleJump;

	// Use this for initialization
	void Start ()
    {
        canDoubleJump = true;
        Events.Register<OnLanding>(() => { canDoubleJump = true; });
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if ((canDoubleJump || MainCam.S.invincible) && !Walk.S.grounded && Input.GetKeyDown(KeyCode.W))
        {
            canDoubleJump = false;
            Walk.S.Jump();
        }
	}
}
