using System;
using System.Collections.Generic;

public enum Event {
	OnReset,
	TorchesLit,
	AltarsActivated,
	LevelComplete
}

public class Events {
	
	private static Dictionary<Event, List<Action>> handlers = new Dictionary<Event, List<Action>>();
	
	public static void Register(Event e, Action callback) {
		if (Events.handlers.ContainsKey(e) == false) {
			Events.handlers[e] = new List<Action>();
		}
		
		Events.handlers[e].Add(callback);
	}
	
	public static void Broadcast(Event e) {
		if (Events.handlers.ContainsKey(e) == false) return;
		
		foreach (var events in Events.handlers[e]) {
			events();
		}
	}
}