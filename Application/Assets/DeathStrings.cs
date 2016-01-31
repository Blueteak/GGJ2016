using UnityEngine;
using System.Collections;

public class DeathStrings : MonoBehaviour {

	public string[] dump;
	public string[] pail;
	public string[] devil;
	public string[] fox;

	// Use this for initialization
	public string getString(int idx)
	{
		if(idx == 0)
			return dump[Random.Range(0, dump.Length)];
		if(idx == 1)
			return pail[Random.Range(0, pail.Length)];
		if(idx == 2)
			return devil[Random.Range(0, devil.Length)];
		else
			return fox[Random.Range(0, fox.Length)];
	}

}
