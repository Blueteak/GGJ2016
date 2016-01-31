using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EndSequence : MonoBehaviour {

	public GameObject WinPanel;
	public string[] PlayerNames;
	public string[] SideStories;

	public string[] DumpQuips;
	public string[] PailQuips;
	public string[] DevilQuips;
	public string[] FoxQuips;

	public Sprite[] candidates;

	public Text TitleText;
	public Text QuipText;
	public Text SideText;
	public Image cdImage;

	public ReadInRandomText rtext;

	public void StartWinSeq(int idx)
	{
		WinPanel.SetActive(true);
		TitleText.text = PlayerNames[idx] + " wins the Election!";
		cdImage.sprite = candidates[idx];

		SideText.text = replaceString(SideStories[Random.Range(0,SideStories.Length)], idx);

		/*
		if(idx == 0)
			QuipText.text = DumpQuips[Random.Range(0, DumpQuips.Length)];
		if(idx == 1)
			QuipText.text = PailQuips[Random.Range(0, PailQuips.Length)];
		if(idx == 2)
			QuipText.text = DevilQuips[Random.Range(0, DevilQuips.Length)];
		if(idx == 3)
			QuipText.text = FoxQuips[Random.Range(0, FoxQuips.Length)];
		*/
	}

	public string replaceString(string inpt, int idx)
	{
		inpt = inpt.Replace("_person1", rtext.randomPerson());
		inpt = inpt.Replace("_character", PlayerNames[idx]);
		inpt = inpt.Replace("_place1", rtext.randomPlace());
		return inpt;
	}
}
