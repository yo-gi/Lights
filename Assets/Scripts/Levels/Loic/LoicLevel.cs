using UnityEngine;

public class LoicLevel : MonoBehaviour {

    void Awake() {
        // ------------------------------  ROOM 0 ------------------------------

        // Show a message after the open door cutscene.
        Events.Register<OnCutsceneEndEvent>(e => {
            var cutscene = e.cutscene as LoicOpenDoorCutscene;

            if (cutscene != null && cutscene.requiredGroup == TorchGroup.LoicLevel0) {
                Navi.S.dialog.Queue(new Speech {
                    dialog = "Looks like the door opened!",
                    duration = 2f
                });
            }
        });
    }
}
