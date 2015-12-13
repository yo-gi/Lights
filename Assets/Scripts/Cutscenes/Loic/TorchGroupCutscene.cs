public class TorchGroupCutscene : LoicInspectCutscene {
    public TorchGroup requiredGroup;

    protected override void InitializeCutscene()
    {
        Events.Register<OnTorchGroupLitEvent>(e => {
            if (e.group != requiredGroup) return;
            StartCutscene();
        });
    }
}
