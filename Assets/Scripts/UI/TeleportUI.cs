﻿public class TeleportUI : RechargeableUI {

	// Use this for initialization
	protected override void Awake () {
        base.Awake();
        ability = Player.S.GetComponent<Teleport>();
	}
}
