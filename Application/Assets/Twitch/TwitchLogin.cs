using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwitchLogin : MonoBehaviour {

	public InputField user;
	public InputField oauth;

	public TwitchIRC irc;

	bool connected;

	public GameObject LoginPanel;


	void Start()
	{
		user.text = PlayerPrefs.GetString("user");
		oauth.text = PlayerPrefs.GetString("auth");
		if(user.text.Length > 2)
			Submit();
	}

	public void Submit ()
	{
		Debug.Log("Submit Fired");
		irc.Login(user.text, oauth.text);
		irc.Connected += Connected;
		StopCoroutine("reconnect");
		StartCoroutine("reconnect");
	}

	void Connected()
	{
		connected = true;
		Debug.Log("Connected to Chat");
	}

	void Update()
	{
		if(connected)
		{
			PlayerPrefs.SetString("user", user.text);
			PlayerPrefs.SetString("auth", oauth.text);
			PlayerPrefs.Save();
			LoginPanel.SetActive(false);
		}
			
	}

	IEnumerator reconnect()
	{
		yield return new WaitForSeconds(5.0f);
		if(!connected)
		{
			Debug.Log("Failed to connect");
		}
	}

}
