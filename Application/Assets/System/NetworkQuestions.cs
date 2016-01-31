using UnityEngine;
using System.Collections;

public class NetworkQuestions : MonoBehaviour {

	PhotonView v;

	public string CurrentQuestion;
	public bool newQuestion;



	// Use this for initialization
	void Start () 
	{
		v = GetComponent<PhotonView>();
	}
	
	public void SendQuestion(string q)
	{
		UILineDraw[] d = FindObjectsOfType<UILineDraw>();
		foreach(UILineDraw x in d)
		{
			x.reset();
		}
		PhotonNetwork.RPC(v, "question", PhotonTargets.All, false, q);
	}

	[PunRPC]
	void question(string q)
	{
		newQuestion = true;
		CurrentQuestion = q;
	}

}
