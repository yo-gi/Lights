using UnityEngine;

public class BossCutscene : Cutscene {

    private Vector3 bossCamPos = new Vector3(-9, 36, 0);
    private float bossCamScale = 20f;

    protected override void DefineCutscene() {
        Do(duration: 2f, action: this.NaviDescribeBoss);
        Do(duration: 5f, action: this.LookAtBoss);
    }

    private void NaviDescribeBoss() {
        NaviSay("The evil shadow lurks up above!");
    }

    private void LookAtBoss() {
        LockCamera(bossCamPos, bossCamScale);
    }
}
