using UnityEngine;
using System.Collections.Generic;

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

        // -----------------------------  CORRIDOR -----------------------------

        Events.Register<OnAltarLitEvent>(e => {
            if (e.ability != Ability.Teleport) return;

            Navi.S.dialog.Queue(new Speech {
                dialog = "You unlocked the teleport ability!\nPress J to teleport forward!",
                duration = 4f
            });
        });        
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            this.TeleportTo(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            this.TeleportTo(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            this.TeleportTo(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            this.TeleportTo(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            this.TeleportTo(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            this.TeleportTo(5);
        }
    }

    private void TeleportTo(int room) {
        var positions = new List<Vector3> {
            new Vector3(1f, 0.5f, 0f),
            new Vector3(33f, 0.5f, 0f),
            new Vector3(69f, 0.5f, 0f),
            new Vector3(109f, 0.5f, 0f),
            new Vector3(100.5f, 15.5f, 0f),
            new Vector3(65f, 30.5f, 0f)
        };

        Player.S.transform.position = positions[room];

        if (room >= 3) {
            Teleport.S.Toggle(true);
        }
    }
}
