using System;
using UnityEngine;
using System.Collections.Generic;

public class BossCutscene : Cutscene {

    protected override float CutSceneLength {
        get {
            return 5f;
        }
    }

    protected override void DefineCutscene() {
        var pos1 = new Vector3(6, 5, 0);
        var pos2 = new Vector3(1, 5, 0);

        Do(0f, 1f, () => {
            NaviGo(pos1);
            PlayerGo(pos1);
            NaviSay("Loic you're the best at making cutscenes");
        });

        Do(1f, 5f, () => {
            NaviGo(pos2);
            NaviSay("Literally the best.");
        });
    }
}
