using UnityEngine;
using System.Collections;

public class TweenRun : MonoBehaviour {

	float ResetTime;

	EasyTween et;

	bool nxt;

	// Use this for initialization
	void Start () 
	{

		et = GetComponent<EasyTween>();
		ResetTime = et.GetAnimationDuration();
	}

	void Update()
	{
		if(!nxt)
		{
			StopCoroutine("Swap");
			StartCoroutine("Swap");
		}
	}

	IEnumerator Swap()
	{
		nxt = true;
		float t =0;
		while(t < ResetTime)
		{
			t += Time.deltaTime;
			yield return true;
			while(!et.isRunning)
				yield return true;
		}
		et.OpenCloseObjectAnimation();
		nxt = false;
	}

}
