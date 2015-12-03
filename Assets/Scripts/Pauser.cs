using UnityEngine;
using System.Collections.Generic;

public static class Pauser {

	private static Dictionary<MonoBehaviour, State> states = new Dictionary<MonoBehaviour, State>();

	public static void Pause(MonoBehaviour item) {
		var state = new State();

		item.enabled = false;

		Rigidbody2D r = item.GetComponent<Rigidbody2D>();

		if (r) {
			state.gravityScale = r.gravityScale;
			state.velocity = r.velocity;
			state.angularVelocity = r.angularVelocity;

			r.gravityScale = 0f;
			r.velocity = Vector2.zero;
			r.angularVelocity = 0f;
		}

		Pauser.states[item] = state;
	}

	public static void Resume(MonoBehaviour item) {
		item.enabled = true;

		if (Pauser.states.ContainsKey(item)) {
			var state = Pauser.states[item];

			var r = item.GetComponent<Rigidbody2D>();

			if (r) {
				r.gravityScale = state.gravityScale;
				r.velocity = state.velocity;
				r.angularVelocity = state.angularVelocity;
			}
		}
	}

	private class State {
		// RigidBody2D
		public float gravityScale;
		public Vector2 velocity;
		public float angularVelocity;
	}
}
