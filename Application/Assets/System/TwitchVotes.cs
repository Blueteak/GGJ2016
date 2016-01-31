using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class TwitchVotes : MonoBehaviour {

	private TwitchIRC IRC;
	public Image[] VoteBars;
	public int[] voteCounts;

	public GameSystem gsg;

	public List<ChatQuestion> qs;

	void Start()
    {

        IRC = FindObjectOfType<TwitchIRC>();
        qs = new List<ChatQuestion>();
        //IRC.SendCommand("CAP REQ :twitch.tv/tags"); //register for additional data such as emote-ids, name color etc.
        IRC.messageRecievedEvent.AddListener(OnChatMsgRecieved);
    }

	void OnChatMsgRecieved(string msg)
    {
        //parse from buffer.
        int msgIndex = msg.IndexOf("PRIVMSG #");
        string msgString = msg.Substring(msgIndex + IRC.channelName.Length + 11);
        string user = msg.Substring(1, msg.IndexOf('!') - 1);

		if(msgString.Contains("vote:") && gsg.canVote())
        {
        	Debug.Log("Vote Counted");
        	string[] v = msgString.Split(':');
        	int i = 0;
        	int.TryParse(v[1], out i);
        	if(i > 0 && i-1 < voteCounts.Length)
        		voteCounts[i-1]++;
        }
        if(msgString.Contains("question:") && msgString.Replace("question:", "").Length < 65)
        {
        	ChatQuestion q = new ChatQuestion();
			q.q = msgString.Replace("question:", "");
			q.name = user;
			qs.Add(q);
        }
    }

	public bool voterQ()
	{
		return qs.Count > 0 && Random.Range(0, 100) < 20;
	}

	public ChatQuestion getChatQuestion()
    {
    	Debug.Log("Getting Chat Question: there are " + qs.Count + " questions to pick");
    	ChatQuestion m = qs[Random.Range(0, qs.Count)];
    	Debug.Log("Question: " + m.q);
    	qs.Remove(m);
    	return m;
    }

    public void sendMessage(string t)
    {
    	IRC.SendMsg(t);
    }

    public void resetVotes()
    {
    	for(int i=0 ;i<voteCounts.Length; i++)
    	{
    		voteCounts[i] = 0;
    	}
    }

    void Update()
    {
    	for(int i=0; i<voteCounts.Length; i++)
    	{
    		VoteBars[i].fillAmount = percent(i);
    	}
    }

    public int lowestCandidate(List<int> lost)
    {
    	int x = int.MaxValue;
    	int id = 0;
    	int q = 0;
    	foreach(var v in voteCounts)
    	{
    		if(v < x && !lost.Contains(q))
    		{
				id = q;
				x = v;
    		}
    		q++;
    	}
    	return id;
    }

    public float percent(int idx)
    {
    	float f = voteCounts[idx];
    	float x = 1;
    	foreach(var v in voteCounts)
    		x += v;
    	return f/x;
    }
}

public class ChatQuestion
{
	public string name;
	public string q;
}
