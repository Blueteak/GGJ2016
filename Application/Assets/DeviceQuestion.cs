using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeviceQuestion : MonoBehaviour {

	NetworkQuestions nq;

	public Image timer;
	public Text txt;

	private string lastQ;

	public UILineDraw drw;

	// Use this for initialization
	void Start () 
	{
		nq = GetComponent<NetworkQuestions>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(lastQ != nq.CurrentQuestion && nq.newQuestion)
		{
			nq.newQuestion = false;
			StopCoroutine("Timer");
			StartCoroutine("Timer");
			AllowDraw();
			txt.text = nq.CurrentQuestion;
			lastQ = nq.CurrentQuestion;
		}
	}

	void AllowDraw()
	{
		drw.canDraw = true;
		drw.reset();
	}

	IEnumerator Timer()
	{
		timer.fillAmount = 1;
		for(int i=0; i<50; i++)
		{
			timer.fillAmount = (50-i)/50f;
			yield return new WaitForSeconds(.5f);
		}
		timer.fillAmount = 0;
		drw.canDraw = false;
	}
}
