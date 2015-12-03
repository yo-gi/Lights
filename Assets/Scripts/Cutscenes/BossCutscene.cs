using System;
using UnityEngine;
using System.Collections.Generic;

public class BossCutscene : Cutscene {

    float start = -1f;

    protected override bool PlayCustcene() {
        if (start < 0f) {
            start = Time.time + 5f;
        }

        if (Time.time > start) {
            return false;
        }
        else {
            Debug.Log("Hi");
            
            return true;
        }
    }
}
