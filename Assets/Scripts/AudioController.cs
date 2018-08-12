using UnityEngine;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

    AudioSource source;
    public List<AudioClip> Clips = new List<AudioClip>();

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}

    public void Play()
    {
        source.clip = Clips[Random.Range(0, Clips.Count)];
        source.Play();
        if (source.loop)
            source.loop = false;
    }
}
