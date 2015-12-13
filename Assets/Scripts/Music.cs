using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public enum MusicState
{ 
	defaultFadeIn,
	defaultFadeOut,
	bossFadeIn,
	bossFadeOut,
	steady
}

public class Music : MonoBehaviour {

	public static Music S;

	//public AudioClip defaultMusic;
	//public AudioClip bossMusic;

	public AudioSource defaultSource;
	public AudioSource bossSource;

	public MusicState state;

	public float volchangefreq;

	bool isBossMusic = false;
	float nextVolchangeTime;


	void Awake()
	{
		S = this;
	}

	// Use this for initialization
	void Start ()
	{
		defaultSource.loop = true;
		bossSource.loop = true;
		bossSource.Stop ();
		defaultSource.Play ();
		nextVolchangeTime = Time.time;
		state = MusicState.steady;
	}

	public void setBossMusic()
	{
		if (isBossMusic == false) {
			isBossMusic = true;
			state = MusicState.defaultFadeOut;
		}
	}
	
	public void setDefaultMusic()
	{
		isBossMusic = false;
		state = MusicState.bossFadeOut;
	}

	void Update()
	{
		if (nextVolchangeTime <= Time.time) {
			switch (state) {
			case MusicState.defaultFadeIn:
				defaultSource.volume += .05f;
				if(defaultSource.volume >= 1)
				{
					defaultSource.volume = 1;
					state = MusicState.steady;
				}
				break;
			case MusicState.defaultFadeOut:
				defaultSource.volume -= .05f;
				if(defaultSource.volume <= 0)
				{
					defaultSource.volume = 0;
					state = MusicState.bossFadeIn;
					defaultSource.Stop ();
					bossSource.volume = 0;
					bossSource.Play ();
				}
				break;
			case MusicState.bossFadeIn:
				bossSource.volume += .02f;
				if(bossSource.volume >= .3f)
				{
					bossSource.volume = .3f;
					state = MusicState.steady;
				}
				break;
			case MusicState.bossFadeOut:
				bossSource.volume -= .01f;
				if(bossSource.volume <= 0)
				{
					bossSource.volume = 0;
					state = MusicState.defaultFadeIn;
					defaultSource.Play ();
					defaultSource.volume = 0;
					bossSource.Stop ();
				}
				break;
			case MusicState.steady:
				break;
			}
			nextVolchangeTime = Time.time + volchangefreq;
		}
	}
}
