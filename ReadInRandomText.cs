using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReadInRandomText : MonoBehaviour {

    string[] people;
    string[] randomQuestions;
    string[] things;
    string[] places;
    string[] verbs;
    string[] questions;
    int counter = 0;
    int counter2 = 0;
    int counter3 = 0;
    int counter4 = 0;
    int counter5 = 0;
    int counter6 = 0;
    int countertimer = 0;
    Text txt;
    Random rnd;
    //C:\Users\Nick\Desktop\Temp
    // Use this for initialization
    void Start()
    {
        txt = GetComponent<Text>();
        people = new string[100];
        randomQuestions = new string[50];
        things = new string[50];
        places = new string[50];
        verbs = new string[50];
        questions = new string[100]; 
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader("C:\\Users\\Nick\\Desktop\\Temp\\People.txt");
        while ((line = file.ReadLine()) != null)
        {
            people[counter] = line;
            counter++;
        }

        file.Close();
        System.IO.StreamReader file2 = new System.IO.StreamReader("C:\\Users\\Nick\\Desktop\\Temp\\RandomQuestions.txt");
        while ((line = file2.ReadLine()) != null)
        {
            randomQuestions[counter2] = line;
            counter2++;
        }

        file.Close();
        System.IO.StreamReader file3 = new System.IO.StreamReader("C:\\Users\\Nick\\Desktop\\Temp\\Places.txt");
        while ((line = file3.ReadLine()) != null)
        {
            places[counter3] = line;
            counter3++;
        }

        file.Close();
        System.IO.StreamReader file4 = new System.IO.StreamReader("C:\\Users\\Nick\\Desktop\\Temp\\Things.txt");
        while ((line = file4.ReadLine()) != null)
        {
            things[counter4] = line;
            counter4++;
        }

        file.Close();
        System.IO.StreamReader file5 = new System.IO.StreamReader("C:\\Users\\Nick\\Desktop\\Temp\\Verbs.txt");
        while ((line = file5.ReadLine()) != null)
        {
            verbs[counter5] = line;
            counter5++;
        }
        System.IO.StreamReader file6 = new System.IO.StreamReader("C:\\Users\\Nick\\Desktop\\Temp\\Questions.txt");
        while ((line = file6.ReadLine()) != null)
        {
            questions[counter6] = line;
            counter6++;
        }

        file.Close();

        file.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (countertimer > 30)
        {
            updateQuestion();
            countertimer = 0;
        }
        countertimer++;
    }

    string updateQuestion()
    {
        int r = Random.Range(0, 100);
        if (r <= 9)
        {
            string cur = randomQuestions[Random.Range(0, counter2)];
            cur = cur.Replace("_person1", people[Random.Range(0, counter)]);
            cur = cur.Replace("_person2", people[Random.Range(0, counter)]);
            cur = cur.Replace("_person3", people[Random.Range(0, counter)]);
            cur = cur.Replace("_verb1", verbs[Random.Range(0, counter5)]);
            cur = cur.Replace("_place1", places[Random.Range(0, counter3)]);
            cur = cur.Replace("_thing1", things[Random.Range(0, counter4)]);
            cur = cur.Replace("_thing2", things[Random.Range(0, counter4)]);
            //txt.text = cur;
            return cur;
        }
        else
        {
            string cur = questions[Random.Range(0, counter6)];
            //txt.text = cur;
            return cur;
        }
    }
}
