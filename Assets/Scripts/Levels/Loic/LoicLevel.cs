using UnityEngine;
using System.Collections;

public class LoicLevel : MonoBehaviour {

    void Awake() {
        this.Room0();
    }

    private void Room0() {
        Events.Register<OnCutsceneEndEvent>(e => {
            if (e.cutscene is LoicLevel0OnTorchLit) {
                Navi.S.dialog.Queue(new Speech {
                    dialog = "Looks like the door opened!",
                    duration = 2f
                });
            }
        });
    }
}
