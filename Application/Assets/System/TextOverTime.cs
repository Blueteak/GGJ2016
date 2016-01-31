using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TextOverTime : MonoBehaviour {

	public string QStr;
	public Text qText;
	int idx = 0;

	public void ChangeQ(string s)
	{
		qText.text = "";
		QStr = s;
		idx = 0;
	}

	void Update () 
	{
		if(idx < QStr.Length)
		{
			qText.text += QStr.Substring(idx,1);
			idx++;
		}
	}
}
