public class TeleportUI : RechargeableUI {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        ability = Player.S.GetComponent<Teleport>();
	}
}
