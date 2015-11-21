public class DashUI : RechargeableUI {

	// Use this for initialization
	protected override void Awake () {
        base.Awake();
        ability = Player.S.GetComponent<Dash>();
	}
}
