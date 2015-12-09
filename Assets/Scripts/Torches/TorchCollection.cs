using System.Collections.Generic;

// All the different groups that a torch can belong to.
public enum TorchGroup {
    None,
    AlphaLevel,
    BossFight,

    LoicLevel0,
    LoicLevel1,

    Group0,
    Group1,
    Group2,
    Group3,
    Group4,
    Group5,
}

public static class Torches {

    private static bool initialized = false;
    private static readonly Dictionary<TorchGroup, HashSet<Torch>> groups = new Dictionary<TorchGroup, HashSet<Torch>>();

    public static void Register(Torch torch) {
        if (initialized == false) {
            Torches.Initialize();
        }

        foreach (var group in torch.groups) {
            if (Torches.groups.ContainsKey(group) == false) {
                Torches.groups.Add(group, new HashSet<Torch> {
                    torch
                });
            }
            else {
                Torches.groups[group].Add(torch);
            }
        }
    }

    private static void Initialize() {
        Events.Register<OnTorchLitEvent>(Torches.OnTorchLit);
        Events.Register<OnTorchUnlitEvent>(Torches.OnTorchUnlit);

        Torches.initialized = true;
    }

    private static void OnTorchLit(OnTorchLitEvent e) {
        var torch = e.torch;

        foreach (var group in torch.groups) {
            if (groups.ContainsKey(group) == false) continue;

            Torches.groups[group].Remove(torch);

            if (Torches.groups[group].Count == 0) {
                Events.Broadcast(new OnTorchGroupLitEvent {
                    group = group
                });
            }
        }
    }
    
    private static void OnTorchUnlit(OnTorchUnlitEvent e) {
        Register(e.torch);
    }
}