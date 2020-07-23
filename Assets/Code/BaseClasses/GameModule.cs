using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using GoogleMobileAds.Api;

public class GameModule : MonoBehaviour
{

    public ColorScheme LocationScheme = ColorScheme.MultiColor;
    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.3f;

    public GameObject trackObject;
    public Image innerDisc;
    public Image outerDisc;
    public LensFlare mus;
    public Text scoreField;
    public Text chainField;
    public Text chainFieldOverlay;

    public Text rScore;
    public GameObject rRecord;
    public Text rRank;
    public Text rRankO;
    public Text rMiss;
    public Text rMissO;
    public Text rWeak;
    public Text rWeakO;
    public Text rGood;
    public Text rGoodO;
    public Text rGreat;
    public Text rGreatO;
    public Text rExcellent;
    public Text rExcellentO;
    public Text rMaxchain;
    public Text rMaxchainO;
    public Text rMaxx;
    public Text rMaxxO;

    public GameObject upMessage;
    public GameObject canvaska;
    public GameObject inGameUI;
    public GameObject resultUI;
    public GameObject pauseUI;
    public GameObject externalUI;
    public GameObject gmUI;

    public GameObject actionCamera;
    public GameObject completeCamera;

    public AudioSource ChainSound;
    public AudioSource BadSound;
    public AudioSource CompleteSound;
    public AudioSource GameOverSound;
    public AudioSource ActionSound;
    public AudioSource EpicSong;

    private int drawDepth = -1000;
    private float fadeAlpha = 1.0f;
    private float fadeDir = -1.0f;

    private TrackIdentity trackIdentity;
    private AudioSource track;
    private GameObject character;
    private int bCo = 100000;
    private string[] sps;
    private int iLimit = 0;
    private int s;
    private int cSample;
    private bool tapped = false;
    private bool missed = false;

    private int score = 0;
    private int multiplier = 1;
    private int chain = 0;
    private bool upActivated = false;
    private bool wasteActivated = false;

    private int miss = 0;
    private int weak = 0;
    private int good = 0;
    private int great = 0;
    private int excellent = 0;

    private int maxchain = 0;
    private int maxx = 0;

    private int lastRecord = 0;
    private string rank = "";

    private bool isPaused = false;
    private int gmCounter = 0;

    private const string bannedId = "ca-app-pub-5649429931709993/1334157868";
    private const string interstitialId = "ca-app-pub-5649429931709993/2810891061";

    private BannerView banner;
    private InterstitialAd interstitial;

    void OnGUI()
    {
        if (isPaused)
        {
            fadeAlpha = 0.0f;
        }
        else
        {
            fadeAlpha += fadeDir * fadeSpeed * Time.deltaTime;
            fadeAlpha = Mathf.Clamp01(fadeAlpha);
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeAlpha);
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
        }
    }

    void Awake() {
        SimpleLocalization.TranslateAll();
    }

    void Start()
    {
        GameStatic.isFirstLoad = false;

        var trackInstance = CreateTrack(GameStatic.Track);
        track = trackInstance.GetComponent<AudioSource>();
        trackIdentity = trackInstance.GetComponent<TrackIdentity>();

        character = CreateCharacter(GameStatic.Character, GameStatic.Dance);

        sps = track.GetComponent<TrackIdentity>().Beats.Split('@');
        iLimit = 1;

        track.time = track.GetComponent<TrackIdentity>().StartTime;
        track.Play();

        banner = new BannerView(bannedId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        banner.LoadAd(request);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Continue();
            else
                Pause();
        }

        if (!isPaused)
        {
            track.UnPause();
            if (track.time < trackIdentity.Duration + 3.0f)
            {
                if (gmCounter < 7)
                {
                    UpdateBeats();
                    CheckBeats();
                }
                else
                {
                    GameOver();
                }
            }
            else
            {
                inGameUI.SetActive(false);
                track.volume = Mathf.Lerp(track.volume, 0.5f, Time.deltaTime * 0.5f);

                int maxScore = sps.Length * 1000;

                if (score < 0.04 * maxScore) rank = "D";
                if (score >= 0.04 * maxScore) rank = "C";
                if (score >= 0.07 * maxScore) rank = "B";
                if (score >= 0.1 * maxScore) rank = "A";
                if (score >= 0.3 * maxScore) rank = "S";

                actionCamera.SetActive(false);
                completeCamera.SetActive(true);

                character.GetComponent<Animator>().runtimeAnimatorController =
                    (RuntimeAnimatorController)Resources.Load("Cheers/" + rank,
                    typeof(RuntimeAnimatorController));
                character.GetComponent<YRootFix>().enabled = false;

                int zeros = 6 - score.ToString().Length;
                string s = "";

                for (int i = 0; i < zeros; i++)
                    s += "0";
                rScore.text = s + score.ToString();
                rRank.text = rank.ToString();
                rRankO.text = rank.ToString();
                rMiss.text = miss.ToString();
                rMissO.text = miss.ToString();
                rWeak.text = weak.ToString();
                rWeakO.text = weak.ToString();
                rGood.text = good.ToString();
                rGoodO.text = good.ToString();
                rGreat.text = great.ToString();
                rGreatO.text = great.ToString();
                rExcellent.text = excellent.ToString();
                rExcellentO.text = excellent.ToString();
                rMaxchain.text = maxchain.ToString();
                rMaxchainO.text = maxchain.ToString();
                rMaxx.text = maxx.ToString();
                rMaxxO.text = maxx.ToString();

                if (PlayerPrefs.HasKey(GameStatic.SongId + "@s"))
                {
                    lastRecord = PlayerPrefs.GetInt(GameStatic.SongId + "@s");
                }

                if (score > lastRecord)
                {
                    rRecord.SetActive(true);
                    PlayerPrefs.SetInt(GameStatic.SongId + "@s", score);
                    PlayerPrefs.SetString(GameStatic.SongId + "@r", rank);
                }

                StartCoroutine("ShowResult");
            }

            if (!track.isPlaying)
            {
                if (!EpicSong.isPlaying)
                    EpicSong.Play();
                if (EpicSong.volume < 1.0f)
                {
                    EpicSong.volume = Mathf.Lerp(EpicSong.volume, 1.0f, Time.deltaTime * 2.0f);
                }
            }
        } else
        {
            track.Pause();
        }
        
    }

    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(3);
        resultUI.SetActive(true);
        externalUI.SetActive(false);
    }

    GameObject CreateTrack(Track track)
    {
        string name = track.ToString("g");
        GameObject instance = Instantiate(Resources.Load("Tracks/" + name)) as GameObject;
        instance.transform.SetParent(trackObject.transform);
        return instance;
    }

    GameObject CreateCharacter(Character character, Dance dance)
    {
        string charName = character.ToString("g");
        string danceName = dance.ToString("g");

        GameObject instance = Instantiate(Resources.Load("Characters/" + charName)) as GameObject;
        var spawn = GameObject.Find("PlayerSpawnPoint").transform;
        instance.transform.position = spawn.position;
        instance.transform.localScale = spawn.localScale;
        instance.transform.rotation = spawn.rotation;

        instance.GetComponent<Animator>().runtimeAnimatorController =
            (RuntimeAnimatorController)Resources.Load("Dances/" + danceName,
            typeof(RuntimeAnimatorController));

        return instance;
    }

    void CreateBeat(int sD, int sC)
    {
        GameObject beat = Instantiate(Resources.Load("Beat")) as GameObject;
        beat.transform.SetParent(GameObject.Find("Canvaska").transform);
        beat.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        beat.GetComponent<Beat>().sampleDestination = sD;
        beat.GetComponent<Beat>().sampleCreated = sC;

        float w0 = (float)Screen.width * 0.5f + 100.0f;
        float w1 = (float)Screen.width + 100.0f;
        float x = Random.Range(w0, w1);
        if (Random.Range(0, 10) > 5) x *= -1.0f;


        float h0 = (float)Screen.height * 0.7f + 100.0f;
        float h1 = (float)Screen.height + 100.0f;
        float y = Random.Range(h0, h1);
        if (Random.Range(0, 10) > 5) y *= -1.0f;

        beat.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
    }

    void UpdateBeats()
    {
        cSample = track.timeSamples;
        for (var i = iLimit; i < sps.Length; i++)
        {
            s = int.Parse(sps[i]);
            if (s > cSample && s - cSample < bCo)
            {
                CreateBeat(int.Parse(sps[i]), cSample);
                sps[i] = "0";
                iLimit = i + 1;
            }
            if (s - cSample > bCo) { break; }
        }
    }

    void Miss()
    {
        missed = true;
        chain = 0;

        string s = "~" + chain.ToString() + "~ " + SimpleLocalization.Translate("COMBO") + " x" + multiplier.ToString();
        chainField.text = s;
        chainFieldOverlay.text = s;

        miss++;
        gmCounter++;
    }

    void CalculateMultiplier()
    {
        int pMultiplier = multiplier;
        switch (chain)
        {
            case 0: multiplier = 1; break;
            case 10: multiplier = 2; break;
            case 20: multiplier = 3; break;
            case 35: multiplier = 4; break;
            case 50: multiplier = 5; break;
            case 65: multiplier = 5; break;
            case 80: multiplier = 7; break;
            case 100: multiplier = 8; break;
            case 120: multiplier = 9; break;
            case 150: multiplier = 10; break;
        }

        if (maxchain < chain) maxchain = chain;
        if (maxx < multiplier) maxx = multiplier;

        if (pMultiplier < multiplier)
        {
            ShowUp(multiplier);
        }

        if (pMultiplier > multiplier)
        {
            string text = "";
            Color32 color1 = new Color();
            Color32 color2 = new Color();

            text = SimpleLocalization.Translate("WASTED");
            color1 = ColorUtils.hexToColorAlpha("FF416BD3");
            color2 = ColorUtils.hexToColorAlpha("FF595965");

            color1.a = 211;
            color2.a = 101;

            Text[] t = upMessage.GetComponentsInChildren<Text>();
            t[0].text = text;
            t[1].text = text;
            t[0].color = color1;
            t[1].color = color2;

            upActivated = true;
            if (!BadSound.isPlaying)
            {
                BadSound.time = 0.2f;
                BadSound.Play();
            }
        }

    }

    void ShowUp(int m)
    {
        string text = "";
        Color32 color1 = new Color();
        Color32 color2 = new Color();

        switch (m)
        {
            case 2:
                text = SimpleLocalization.Translate("NICE");
                color1 = ColorUtils.hexToColorAlpha("05FFE8D3");
                color2 = ColorUtils.hexToColorAlpha("05FFE865");
                break;
            case 3:
                text = SimpleLocalization.Translate("COOL");
                color1 = ColorUtils.hexToColorAlpha("65FF70D3");
                color2 = ColorUtils.hexToColorAlpha("65FF7065");
                break;
            case 4:
                text = SimpleLocalization.Translate("+ALPHA");
                color1 = ColorUtils.hexToColorAlpha("FF69E0D3");
                color2 = ColorUtils.hexToColorAlpha("FF69E065");
                break;
            case 5:
                text = SimpleLocalization.Translate("SPECTAC");
                color1 = ColorUtils.hexToColorAlpha("7094FFD3");
                color2 = ColorUtils.hexToColorAlpha("7094FF65 ");
                break;
            case 6:
                text = SimpleLocalization.Translate("CAPACITY");
                color1 = ColorUtils.hexToColorAlpha("21A7FFD3");
                color2 = ColorUtils.hexToColorAlpha("21A7FF65");
                break;
            case 7:
                text = SimpleLocalization.Translate("SUPERIOR");
                color1 = ColorUtils.hexToColorAlpha("FFF1ACD3");
                color2 = ColorUtils.hexToColorAlpha("21A7FF65");
                break;
            case 8:
                text = SimpleLocalization.Translate("OVERHEAT");
                color1 = ColorUtils.hexToColorAlpha("FCFF47D3");
                color2 = ColorUtils.hexToColorAlpha("FCFF4765");
                break;
            case 9:
                text = SimpleLocalization.Translate("FEVER");
                color1 = ColorUtils.hexToColorAlpha("50FFD8D3");
                color2 = ColorUtils.hexToColorAlpha("FF292965");
                break;
            case 10:
                text = SimpleLocalization.Translate("PERFECT");
                color1 = ColorUtils.hexToColorAlpha("05FFE8D3");
                color2 = ColorUtils.hexToColorAlpha("05FFE865");
                break;
        }

        color1.a = 211;
        color2.a = 101;

        Text[] t = upMessage.GetComponentsInChildren<Text>();
        t[0].text = text;
        t[1].text = text;
        t[0].color = color1;
        t[1].color = color2;

        upActivated = true;
        if (!ChainSound.isPlaying)
        {
            ChainSound.time = 0.2f;
            ChainSound.Play();
        }
    }

    void CheckBeats()
    {
        var beats = canvaska.GetComponentsInChildren<Beat>();

        foreach (var beat in beats)
        {
            if (Vector2.Distance(beat.rect.anchoredPosition,
                new Vector2(0, 0)) < 0.01f)
            {
                beat.lifeTime--;
                if (beat.lifeTime <= 0)
                {
                    DestroyObject(beat.gameObject);
                    Miss();
                }
            }
        }

        if ((Input.GetMouseButtonDown(0) || Input.anyKeyDown) && !Input.GetKeyDown(KeyCode.Escape))
        {
            mus.brightness = 0;

            innerDisc.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            outerDisc.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            innerDisc.color = ColorUtils.hexToColor("A5F7FFFF");
            outerDisc.color = ColorUtils.hexToColor("A5F7FFFF");

            if (beats.Length > 0 && beats[0].distanceSamples < 10000)
            {
                tapped = true;
                mus.color = beats[0].color;
                chain++;

                int distance = beats[0].distanceSamples;
                int scorePortion = 0;
                if (distance <= 1000)
                {
                    scorePortion = 100;
                    excellent++;
                }
                if (distance > 1000)
                {
                    scorePortion = 50;
                    great++;
                }
                if (distance > 2000)
                {
                    scorePortion = 20;
                    good++;
                }
                if (distance > 5000 || distance <= 0)
                {
                    scorePortion = 5;
                    weak++;
                }
       
                score += scorePortion * multiplier;
                int zeros = 6 - score.ToString().Length;
                string s = "";

                for (int i = 0; i < zeros; i++)
                    s += "0";
                scoreField.text = s + score.ToString();

                s = "~" + chain.ToString() + "~" + SimpleLocalization.Translate("COMBO") + "x" + multiplier.ToString();
                chainField.text = s;
                chainFieldOverlay.text = s;

                DestroyObject(beats[0].gameObject);
                gmCounter = 0;
            }
            else
            {
                Miss();
            }
        }

        if (!tapped & !missed)
        {
            mus.brightness = Mathf.Lerp(mus.brightness, 0f, Time.deltaTime * 20.0f);

            innerDisc.rectTransform.localScale = Vector3.Lerp(innerDisc.rectTransform.localScale,
                new Vector3(1.0f, 1.0f, 1.0f), Time.deltaTime * 10.0f);
            outerDisc.rectTransform.localScale = Vector3.Lerp(outerDisc.rectTransform.localScale,
                new Vector3(1.0f, 1.0f, 1.0f), Time.deltaTime * 10.0f);

            innerDisc.color = Color.Lerp(innerDisc.color, ColorUtils.hexToColor("A5F7FFFF"),
                Time.deltaTime * 10.0f);
            outerDisc.color = Color.Lerp(innerDisc.color, ColorUtils.hexToColor("A5F7FFFF"),
                Time.deltaTime * 10.0f);
        }
        else if (tapped)
        {
            mus.brightness = Mathf.Lerp(mus.brightness, 0.40f, Time.deltaTime * 20.0f);

            innerDisc.rectTransform.localScale = Vector3.Lerp(innerDisc.rectTransform.localScale,
                new Vector3(1.1f, 1.1f, 1.0f), Time.deltaTime * 10.0f);
            outerDisc.rectTransform.localScale = Vector3.Lerp(outerDisc.rectTransform.localScale,
                new Vector3(1.1f, 1.1f, 1.0f), Time.deltaTime * 10.0f);

            innerDisc.color = Color.Lerp(innerDisc.color, ColorUtils.hexToColor("21FF89FF"),
                Time.deltaTime * 10.0f);
            outerDisc.color = Color.Lerp(innerDisc.color, ColorUtils.hexToColor("21FF89FF"),
                Time.deltaTime * 10.0f);
        }
        else if (missed)
        {
            innerDisc.rectTransform.localScale = Vector3.Lerp(innerDisc.rectTransform.localScale,
                new Vector3(1.1f, 1.1f, 1.0f), Time.deltaTime * 10.0f);
            outerDisc.rectTransform.localScale = Vector3.Lerp(outerDisc.rectTransform.localScale,
                new Vector3(1.1f, 1.1f, 1.0f), Time.deltaTime * 10.0f);

            innerDisc.color = Color.Lerp(innerDisc.color, ColorUtils.hexToColor("FF0B56FF"),
                Time.deltaTime * 10.0f);
            outerDisc.color = Color.Lerp(innerDisc.color, ColorUtils.hexToColor("FF0B56FF"),
                Time.deltaTime * 10.0f);
        }

        if (innerDisc.rectTransform.localScale.x > 1.09f && outerDisc.rectTransform.localScale.x > 1.09f)
        {
            tapped = false;
            missed = false;
        }

        CalculateMultiplier();

        if (upActivated)
        {
            float a = upMessage.GetComponent<CanvasGroup>().alpha;
            float s = 8.0f / (float)upMessage.GetComponentInChildren<Text>().text.Length;
            upMessage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(a, 1.0f, Time.deltaTime * 7.0f);
            upMessage.GetComponent<RectTransform>().localScale =
                Vector3.Lerp(upMessage.GetComponent<RectTransform>().localScale,
                new Vector3(s, s, s), Time.deltaTime * 3.0f);

            if (a >= 0.95f)
            {
                upMessage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(a, 0.0f, Time.deltaTime * 10.0f);
            }

            if (upMessage.GetComponent<RectTransform>().localScale.x >= s - 0.01f)
                upActivated = false;
        }
        else
        {
            upMessage.GetComponent<CanvasGroup>().alpha = 0.0f;
            upMessage.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }

    private void GameOver()
    {
        externalUI.SetActive(false);
        inGameUI.SetActive(false);
        gmUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        GameOverSound.time = 0.2f;
        GameOverSound.Play();

        if (GameStatic.adCounter == 0 || GameStatic.adCounter >= 2)
        {
            GameStatic.adCounter = 0;
            interstitial = new InterstitialAd(interstitialId);
            AdRequest request = new AdRequest.Builder().Build();
            interstitial.LoadAd(request);
            interstitial.OnAdLoaded += Interstitial_OnAdLoaded;
        }

        GameStatic.adCounter++;
    }

    private void Interstitial_OnAdLoaded(object sender, System.EventArgs e)
    {
        interstitial.Show();
    }

    public void Pause()
    {
        inGameUI.SetActive(false);
        pauseUI.SetActive(true);
        externalUI.SetActive(false);
        Time.timeScale = 0;
        isPaused = true;
        ActionSound.time = 0.1f;
        ActionSound.Play();
    }

    public void Continue()
    {
        inGameUI.SetActive(true);
        pauseUI.SetActive(false);
        externalUI.SetActive(true);
        Time.timeScale = 1;
        isPaused = false;

        ActionSound.time = 0.1f;
        ActionSound.Play();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        ActionSound.time = 0.1f;
        ActionSound.Play();
        StartCoroutine("CRestart");
    }

    public void Exit()
    {
        Time.timeScale = 1;
        ActionSound.time = 0.1f;
        ActionSound.Play();
        StartCoroutine("CExit");
    }

    IEnumerator CRestart()
    {
        yield return new WaitForSeconds(1);
        
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    IEnumerator CExit()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("menu");
    }
}
