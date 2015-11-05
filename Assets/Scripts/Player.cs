using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player S;

	public LightColor color;
	public GameObject water;
    
	Walk walk;
	Swim swim;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		walk = GetComponent<Walk>();
		swim = GetComponent<Swim>();
        
		walk.enabled = true;
		swim.enabled = false;

        color = LightColor.White;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "water") {
			walk.enabled = false;
			swim.enabled = true;

			water = collider.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "water") {
			walk.enabled = true;
			swim.enabled = false;

			if (water == collider.gameObject) water = null;
		}
	}

	// Update is called once per frame
	void Update () {
		/* Colors */
		if (Input.GetKey(KeyCode.Alpha1)) {
			switchColors(LightColor.White);
		} else if (Input.GetKey(KeyCode.Alpha2)) {
			switchColors (LightColor.Red);
		} else if (Input.GetKey(KeyCode.Alpha3)) {
			switchColors(LightColor.Blue);
		} else if (Input.GetKey(KeyCode.Alpha4)) {
			switchColors(LightColor.Yellow);
		}
	}
	
    public void switchColors(LightColor newColor)
    {
        color = newColor;
        Navi.S.ChangeColor(color);
    }
}











