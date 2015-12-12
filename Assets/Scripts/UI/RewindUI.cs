public class RewindUI : RechargeableUI {

	// Use this for initialization
	protected void Start () {
        ability = Player.S.GetComponent<Rewind>();
	}
}
