using UnityEngine;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

    AudioSource sorce;
    public List<AudioClip> Clips = new List<AudioClip>();

	// Use this for initialization
	void Start () {
        sorce = GetComponent<AudioSource>();
	}

    public void Play()
    {
        sorce.clip = Clips[Random.Range(0, Clips.Count)];
        sorce.Play();
    }
}
