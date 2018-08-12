using UnityEngine;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

    AudioSource sorce;
    public List<AudioClip> Clips = new List<AudioClip>();

	// Use this for initialization
	void Start () {
        sorce = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
