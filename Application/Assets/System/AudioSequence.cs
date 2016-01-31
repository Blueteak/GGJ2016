using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AudioSequence : MonoBehaviour {

	public float Volume;
	public List<AudioClip> clips;

	AudioSource src;


	// Use this for initialization
	void Start () 
	{
		src = GetComponent<AudioSource>();
	}
	
	public void NextSong()
	{
		StopCoroutine("ChangeSong");
		StartCoroutine("ChangeSong");
	}

	IEnumerator ChangeSong()
	{
		while(src.volume > 0)
		{
			src.volume -= Time.deltaTime*0.25f;
			yield return true;
		}
		src.clip = clips[0];
		src.Play();
		if(clips.Count > 1)
			clips.Remove(clips[0]);
		while(src.volume < Volume)
		{
			src.volume += Time.deltaTime*0.25f;
			yield return true;
		}

	}
}
