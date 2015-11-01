using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ParticleTest : MonoBehaviour {
	void Update () {
		GetComponent<ParticleSystem>().startSize = transform.lossyScale.magnitude;
	}
}