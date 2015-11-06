using UnityEngine;

public class Fixture : MonoBehaviour {
    
    public LightColor currentColor;
    public LightColor specialColor;

    public bool ____________________;

    private SpriteRenderer lightObject;
    private DynamicLight lightScript;
    private float triggerDistance;

    // Use this for initialization
	void Start ()
    {
        lightObject = this.transform.Find("Inner").GetComponent<SpriteRenderer>();
        lightScript = this.GetComponent<DynamicLight>();
        UpdateFixtureColor();
        triggerDistance = 1f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Vector3.Distance(transform.position, Player.S.transform.position) < triggerDistance)
            {
                SwapLights(Player.S.color);
            }
        }
    }

    private void SwapLights(LightColor newColor)
    {
        Player.S.switchColors(currentColor);
        currentColor = newColor;
        UpdateFixtureColor();
    }

    private void UpdateFixtureColor()
    {
        lightObject.color = Colors.GetColor(currentColor);
        lightScript.lightMaterial = Colors.GetColorMaterial(currentColor);
    }
}
