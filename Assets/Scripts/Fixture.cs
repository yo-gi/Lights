using UnityEngine;
using System.Collections;

public class Fixture : MonoBehaviour {
    
    public LightColor currentColor;
    public LightColor specialColor;

    public bool ____________________;

    private SpriteRenderer lightObject;
    private Material lightMaterial;
    private float triggerDistance;

    // Use this for initialization
	void Start () {
        lightObject = this.transform.Find("Inner").GetComponent<SpriteRenderer>();
        lightMaterial = this.GetComponent<DynamicLight>().lightMaterial;
        UpdateFixtureColor();
        triggerDistance = 1f;
	}
	
	// Update is called once per frame
	void Update () {
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
        Color color = Colors.GetColor(currentColor);
        lightObject.color = color;
        lightMaterial.color = color;
    }
}
