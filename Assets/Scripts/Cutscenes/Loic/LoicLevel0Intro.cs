using UnityEngine;

public class LoicLevel0Intro : Cutscene {

    public GameObject torch;

    protected override void DefineCutscene() {
        Do(duration: 2f, action: this.NaviSpeech);
        Do(duration: 3f, action: this.LookAtTorch);
    }

    private void NaviSpeech() {
        NaviSay("Look! A torch, up there!");
    }

    private void LookAtTorch() {
        LockCamera(torch.transform.position);
    }
}
