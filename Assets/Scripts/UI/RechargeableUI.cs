using UnityEngine;
using UnityEngine.UI;

public class RechargeableUI : MonoBehaviour {

    public GameObject Icon;
    public GameObject Text;

    public bool _______________________;

    private Image image;
    private Text text;
    public Rechargeable ability;

    // Use this for initialization
    virtual protected void Awake () {
        image = Icon.GetComponent<Image>();
        if (Text != null)
        {
            text = Text.GetComponent<Text>();
        }
	}
	
	// Update is called once per frame
	virtual protected void Update () {
        if (text != null)
        {
            text.text = ability.Charges.ToString();
        }
        image.fillAmount = ability.ChargePercentage;
	}
}
