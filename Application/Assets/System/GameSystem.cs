using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class GameSystem : MonoBehaviour {

	public bool gameStarted;
	bool readyForNewQ = true;
	int Qs;
	bool inElim;

	public ReadInRandomText rq;
	public NetworkQuestions nq;

	public Image timerImage;

	public TextOverTime txt;

	public EasyTween et;

	bool intro;
	bool doneIntro;

	public string[] introStrings;
	public EasyTween[] tweens;

	public TwitchVotes tvotes;

	public string[] pNames;
	public List<string> elimStart;
	public EasyTween[] elimAnims;

	public DeathStrings dstr;

	public List<string> WaitMessages;

	public List<string> userQuestions;

	void Start()
	{
		lost = new List<int>();
	}

	// Update is called once per frame
	void Update () 
	{
		if(!gameStarted && PhotonNetwork.inRoom && PhotonNetwork.playerList.Length > 1)
		{
			gameStarted = true;
		}	
		else if(gameStarted)
		{
			
			if(!intro)
			{
				DoIntro();
			}
			if(readyForNewQ && doneIntro)
			{
				if(Qs > 3 && !inElim)
				{
					if(lost.Count == 3)
					{
						//End Game
						inElim = true;
						EndGame();
					}
					else
					{
						inElim = true;
						Elimination();
						Qs = 0;
					}
				}
				else if(!inElim)
				{
					Qs++;
					readyForNewQ = false;
					NextQuestion();
				}
			}
		}
	}

	public bool canVote()
	{
		return doneIntro && gameStarted && !inElim;
	}

	int lowest = 0;
	public List<int> lost;

	void EndGame()
	{

	}

	void Elimination()
	{
		lowest = tvotes.lowestCandidate(lost);
		Debug.Log("Lowest: " + lowest);
		lost.Add(lowest);
		Debug.Log(lost[0]);
		StopCoroutine("DoElimination");
		StartCoroutine("DoElimination");
	}

	IEnumerator DoElimination()
	{
		et.isRunning = true;
		txt.ChangeQ("Round's Over!");
		yield return new WaitForSeconds(2.5f);
		string s = elimStart[Random.Range(0,elimStart.Count)];
		elimStart.Remove(s);
		txt.ChangeQ(s);
		yield return new WaitForSeconds(5f);
		txt.ChangeQ("Looks like " + pNames[lowest] + " is going in the stew!");
		yield return new WaitForSeconds(4f);
		elimAnims[lowest].OpenCloseObjectAnimation();
		yield return new WaitForSeconds(2f);
		txt.ChangeQ(dstr.getString(lowest));
		yield return new WaitForSeconds(4f);
		txt.ChangeQ("Time for the next round!");
		yield return new WaitForSeconds(2.5f);
		et.isRunning = false;
		inElim = false;
	}

	void DoIntro()
	{
		intro = true;
		StartCoroutine("dointro");
	}

	IEnumerator dointro()
	{
		et.isRunning = true;
		foreach(var t in tweens)
			t.isRunning = true;
		foreach(var v in introStrings)
		{
			txt.ChangeQ(v);
			yield return new WaitForSeconds(6f);
		}
		foreach(var t in tweens)
			t.isRunning = false;
		et.isRunning = false;
		doneIntro = true;
	}

	void NextQuestion()
	{
		StopCoroutine("AskQuestion");
		StartCoroutine("AskQuestion");
	}

	IEnumerator AskQuestion()
	{
		string Q = "";
		if(tvotes.voterQ())
		{
			string s  = userQuestions[Random.Range(0, userQuestions.Count)];
			if(userQuestions.Count > 1)
				userQuestions.Remove(s);
			ChatQuestion q = tvotes.getChatQuestion();
			s = s.Replace("_username", q.name);
			txt.ChangeQ(s);
			Q += q.name + " asks:" + q.q;
			yield return new WaitForSeconds(2.2f);
		}
		else
			Q = rq.GetNewQuestion();
		nq.SendQuestion(Q);
		txt.ChangeQ(Q);
		tvotes.sendMessage("A new round of voting has started!");
		tvotes.resetVotes();
		timerImage.fillAmount = 1;
		et.isRunning = true;
		for(int i=0; i<54; i++)
		{
			if(i > 15)
			{
				et.isRunning = false;
			}
			timerImage.fillAmount = (54-i)/54f;
			yield return new WaitForSeconds(.5f);
		}
		timerImage.fillAmount = 0;
		yield return new WaitForSeconds(2.5f);
		string wt = WaitMessages[Random.Range(0, WaitMessages.Count)];
		txt.ChangeQ(wt);
		yield return new WaitForSeconds(4f);
		if(WaitMessages.Count > 1)
			WaitMessages.Remove(wt);
		readyForNewQ = true;
	}
}
