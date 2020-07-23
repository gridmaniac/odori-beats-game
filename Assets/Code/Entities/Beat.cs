using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Beat : MonoBehaviour {
    public int sampleCreated;
    public int sampleDestination;
    public int distanceSamples;
    public int lifeTime = 7;
    public Color color;
    public RectTransform rect;

    private AudioSource audioSource;
    
    private int sampleCurrent;

    private float t;
    private float bm;
    private float dm;

    public int rnd;

    private Vector2 initialPos;
    private Color[] colors;

    void Awake ()
    {
        colors = new Color[5];
        colors[0] = ColorUtils.hexToColor("67F8FF");
        colors[1] = ColorUtils.hexToColor("FF83FB");
        colors[2] = ColorUtils.hexToColor("A0FF30");
        colors[3] = ColorUtils.hexToColor("F2FF27");

        rect = GetComponent<RectTransform>();
        audioSource = GameObject.Find("Track").GetComponentInChildren<AudioSource>();
    }

    void Start()
    {
        initialPos = rect.anchoredPosition;
        rect.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        rnd = Random.Range(0, 4);
        GetComponent<Image>().color = colors[rnd];
        color = colors[rnd];
    }
	
	void Update () {
        sampleCurrent = audioSource.timeSamples;
        bm = sampleDestination - sampleCreated;
        dm = sampleCurrent - sampleCreated;
        t = dm / bm;

        rect.anchoredPosition = Vector2.Lerp(initialPos, new Vector2(0, 0), t);
        rect.Rotate(Vector3.forward * -50.0f);
        distanceSamples = sampleDestination - sampleCurrent;

        if (Vector2.Distance(rect.anchoredPosition, new Vector2(0, 0)) > 0.1f)
        {
             rect.localScale = Vector3.Lerp(new Vector3(0.4f, 0.4f, 0.4f), 
                 new Vector3(1.0f, 1.0f, 1.0f), t);
        }
    }
}
