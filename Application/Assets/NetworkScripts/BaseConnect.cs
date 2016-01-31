using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BaseConnect : MonoBehaviour {

	public bool isHost;

	public Image[] candidates;

	// Use this for initialization
	void Start () 
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	public void OnJoinedLobby()
	{
		if(isHost)
			PhotonNetwork.CreateRoom(null);
		else
    		PhotonNetwork.JoinRandomRoom();
	}

	public void AttemptRejoin()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
    	Debug.Log("Can't join random room!");
    	Invoke("AttemptRejoin", 2.5f);
	}

	void OnPhotonPlayerConnected()
	{
		if(PhotonNetwork.isMasterClient)
		{
			int x = PhotonNetwork.playerList.Length;
			for(int i=0; i<x-1; i++)
			{
				candidates[i].color = Color.white;
			}
		}
	}

	void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
