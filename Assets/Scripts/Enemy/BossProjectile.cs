using UnityEngine;
using System.Collections;

public class BossProjectile : MonoBehaviour {

	public float lifetime;
    public float fadeDuration;

	public int attackDamage;
    
    public float speed;
	public Vector3 velocity;

    private ParticleSystem ps;
    private Color color;

	// Use this for initialization
	void Start () {
		lifetime += Time.time;
        ps = gameObject.GetComponent<ParticleSystem>();
        color = ps.startColor;
        velocity = Player.S.transform.position - transform.position;
        velocity = velocity.normalized * speed;

        Events.Register<OnPauseEvent>(OnPause);
	}
	
	void OnCollisionEnter2D(Collision2D other) {
		// Destroy the projectile if it hits the player.
		if (other.gameObject == Player.S.gameObject) {
			Player.S.TakeDamage(attackDamage);
			Destroy(this.gameObject);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Time.time > lifetime) {
            StartCoroutine(Fade());
		}
        transform.position += velocity * Time.fixedDeltaTime;
	}

    IEnumerator Fade()
    {
        float endTime = Time.time + fadeDuration;
        while (Time.time < endTime)
        {
            color.a = (endTime - Time.time) / fadeDuration;
            ps.startColor = color;
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnPause(OnPauseEvent e)
    {
        if (e.paused)
        {
            Pauser.Pause(this);
        }
        else
        {
            Pauser.Resume(this);
        }
    }
}
