using UnityEngine;
using System.Collections;

public class NetworkDraw : MonoBehaviour {

	public UILineDraw drw;

	public UILineDraw[] usrs;

	public EasyTween[] avatars;

	public GameSystem gs;

	void Start()
	{
		InvokeRepeating("Send", 1, 0.01f);
	}

	void Send()
	{
		if(drw.Points.Count > 0 && PhotonNetwork.inRoom)
		{
			string s = "";
			s += PhotonNetwork.player.ID + "+";
			while(drw.Points.Count > 0)
			{
				Vector2 v = drw.Points.Dequeue();
				s += (int)v.x+","+(int)v.y+":";
			}
			PhotonNetwork.RPC(GetComponent<PhotonView>(), "sendPoints", PhotonTargets.OthersBuffered, false, s.Substring(0, s.Length-1));
		}
	}

	[PunRPC]
	void sendPoints(string ptData)
	{
		if(PhotonNetwork.isMasterClient)
		{
			string[] dat = ptData.Split('+');
			int pid;
			int.TryParse(dat[0], out pid);
			pid -= 2;
			string[] points = dat[1].Split(':');
			foreach(var p in points)
			{
				string[] P = p.Split(',');
				int x=0; int y=0;
				int.TryParse(P[0], out x); int.TryParse(P[1], out y);
				Vector2 v = new Vector2(x,y);
				usrs[pid].Draw(v, pid);
				if(v.x >= 0 && !gs.lost.Contains(pid) )
					avatars[pid].isRunning = true;
				else if(gs.doneIntro)
					avatars[pid].isRunning = false;
			}
		}
	}
}
