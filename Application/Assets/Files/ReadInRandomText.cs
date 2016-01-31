using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ReadInRandomText : MonoBehaviour {

    string[] people;
    string[] randomQuestions;
    string[] things;
    string[] places;
    string[] verbs;
    string[] badAct;
	string[] nats;
	string[] theThings;
    List<string> questions;

   	public TextAsset peopleText;
	public TextAsset randomQuestionsText;
	public TextAsset thingsText;
	public TextAsset placesText;
	public TextAsset verbsText;
	public TextAsset questionsText;
	public TextAsset horribActText;
	public TextAsset natText;
	public TextAsset theThingText;


    //Text txt;
    Random rnd;

    void Start()
    {
        //txt = GetComponent<Text>();
        UsedQuestions = new List<string>();

        people = peopleText.text.Split('\n');
		randomQuestions = randomQuestionsText.text.Split('\n');
		things = thingsText.text.Split('\n');
		places = placesText.text.Split('\n');
		verbs = verbsText.text.Split('\n');
		badAct = horribActText.text.Split('\n');
		nats = natText.text.Split('\n');
		theThings = theThingText.text.Split('\n');
		string[] qs = questionsText.text.Split('\n');
		questions = new List<string>();
		foreach(var q in qs)
		{
			questions.Add(q);
		}

    }

    public string GetNewQuestion()
    {
    	return updateQuestion();
    }

    public List<string> UsedQuestions;

    string updateQuestion()
    {
        int r = Random.Range(0, 100);
		 if(r < 63)
        {
        	//Replace with question from audience
        	return randomQ();
        }
        else if(questions.Count > 0)
        {
			string cur = questions[Random.Range(0, questions.Count)];
			questions.Remove(cur);
            return cur;
        }
        else
        {
        	return randomQ();
        }
    }

    public string randomPerson()
    {
		return people[Random.Range(0, people.Length)];
    }

    public string randomPlace()
    {
		return places[Random.Range(0, verbs.Length)];
    }

    string randomQ()
    {
		string cur = randomQuestions[Random.Range(0, randomQuestions.Length)];
        cur = cur.Replace("_person1", people[Random.Range(0, people.Length)]);
        cur = cur.Replace("_person2", people[Random.Range(0, people.Length)]);
        cur = cur.Replace("_person3", people[Random.Range(0, people.Length)]);
        cur = cur.Replace("_verb1", verbs[Random.Range(0, verbs.Length)]);
        cur = cur.Replace("_place1", places[Random.Range(0, verbs.Length)]);
        cur = cur.Replace("_thing1", things[Random.Range(0, things.Length)]);
        cur = cur.Replace("_thing2", things[Random.Range(0, things.Length)]);
		cur = cur.Replace("_nationality1", nats[Random.Range(0, nats.Length)]);
		cur = cur.Replace("_nationality2", nats[Random.Range(0, nats.Length)]);
		cur = cur.Replace("_concatThing1", theThings[Random.Range(0, theThings.Length)]);
		cur = cur.Replace("_concatThing2", theThings[Random.Range(0, theThings.Length)]);
		cur = cur.Replace("_concatThing3", theThings[Random.Range(0, theThings.Length)]);
		cur = cur.Replace("_horribleAction", badAct[Random.Range(0, badAct.Length)]);
		cur = cur.Replace("_person4", people[Random.Range(0, people.Length)]);
        return cur;
    }
}
