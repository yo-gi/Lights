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
    virtual protected void Start () {
        image = Icon.GetComponent<Image>();
        text = Text.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = ability.Charges.ToString();
        image.fillAmount = ability.ChargePercentage;
	}
}
