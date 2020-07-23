using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SongController : MonoBehaviour {

    public string id;

    public Text score;
    public GameObject s;
    public GameObject a;
    public GameObject b;
    public GameObject c;
    public GameObject d;

    // Use this for initialization
    void Start () {
        int sc;
        string rank = "";

        string scoreKey = id + "@s";
        string rankKey = id + "@r";

        if (PlayerPrefs.HasKey(scoreKey))
        {
            sc = PlayerPrefs.GetInt(scoreKey);
        }
        else
        {
            sc = 0;
        }

        if (PlayerPrefs.HasKey(rankKey))
        {
            rank = PlayerPrefs.GetString(rankKey);
        }

        if (rank != "")
        {
            switch (rank)
            {
                case "S":
                    s.SetActive(true);
                    break;
                case "A":
                    a.SetActive(true);
                    break;
                case "B":
                    b.SetActive(true);
                    break;
                case "C":
                    c.SetActive(true);
                    break;
                case "D":
                    d.SetActive(true);
                    break;
            }
        }

        if (sc != 0)
        {
            int zeros = 6 - sc.ToString().Length;
            string s = "";

            for (int i = 0; i < zeros; i++)
                s += "0";
            score.text = s + sc.ToString();
        }
    }
}
