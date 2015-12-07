using UnityEngine;

public class ParticleSystemAutoDestruct : MonoBehaviour
{
    ParticleSystem ps;

	void Awake()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
    }
	
	void Update ()
    {
	    if (!ps.IsAlive())
        {
            Destroy(gameObject);
        }
	}
}
