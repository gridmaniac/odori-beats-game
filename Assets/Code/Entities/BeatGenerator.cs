using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeatGenerator : MonoBehaviour {

    public string songBeat;

    public string starts;
    public string ends;

    public Text s;
    public Text e;
    public Text c;

    public float beginFrom = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            songBeat += "@" + GetComponent<AudioSource>().timeSamples;
        }

        s.text = "S: " + starts;
        e.text = "E: " + ends;
        c.text = "C: " + GetComponent<AudioSource>().time;
    }

    public void Play()
    {
        GetComponent<AudioSource>().time = beginFrom;
        GetComponent<AudioSource>().Play();
    }
}
