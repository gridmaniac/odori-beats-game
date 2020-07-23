using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class MenuController : MonoBehaviour {

    public AudioSource actionSound;
    public GameObject mainScreen;
    public GameObject selectScreen;
    public GameObject loadingScreen;

    public InfiniteSlider sceneSlider;
    public InfiniteSlider trackSlider;
    public InfiniteSlider danceSlider;
    public InfiniteSlider charSlider;

    private bool isMainScreen = false;
    private bool isSelectScreen = false;
    private int scene = 0;

    private const string bannedId = "ca-app-pub-5649429931709993/1334157868";
    private const string interstitialId = "ca-app-pub-5649429931709993/2810891061";

    private BannerView banner;
    private InterstitialAd interstitial;

    void Awake()
    {
        SimpleLocalization.TranslateAll();

        trackSlider.startSlide = Random.Range(0, 15);
        danceSlider.startSlide = Random.Range(0, 7);
        charSlider.startSlide = Random.Range(0, 7);
        sceneSlider.startSlide = Random.Range(0, 4);
    }

    // Use this for initialization
    void Start () {
        if (GameStatic.isFirstLoad)
        {
            mainScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
            isMainScreen = true;
        }
        else
        {
            selectScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
            isSelectScreen = true;

            if (GameStatic.adCounter == 1)
            {
                interstitial = new InterstitialAd(interstitialId);
                AdRequest request = new AdRequest.Builder().Build();
                interstitial.LoadAd(request);
                interstitial.OnAdLoaded += Interstitial_OnAdLoaded;
            }

            if (GameStatic.adCounter >= 2)
            {
                GameStatic.adCounter = 0;
            }

            GameStatic.adCounter++;
        }

        banner = new BannerView(bannedId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request2 = new AdRequest.Builder().Build();
        banner.LoadAd(request2);
    }

    private void Interstitial_OnAdLoaded(object sender, System.EventArgs e)
    {
        interstitial.Show();
    }

    // Update is called once per frame
    void Update () {
	    if (isMainScreen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMainScreen = false;
                isSelectScreen = true;
                actionSound.time = 0.1f;
                actionSound.Play();
            }
        }

        if (!isSelectScreen && selectScreen.GetComponent<CanvasGroup>().alpha > 0.0f)
        {
            selectScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(
                selectScreen.GetComponent<CanvasGroup>().alpha, 0.0f, Time.deltaTime * 5.0f
            );
        }

        if (isSelectScreen && mainScreen.GetComponent<CanvasGroup>().alpha < 0.01f)
        {
            selectScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(
                selectScreen.GetComponent<CanvasGroup>().alpha, 1.0f, Time.deltaTime * 5.0f
            );
        }

        if (!isMainScreen && mainScreen.GetComponent<CanvasGroup>().alpha > 0.0f)
        {
            mainScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(
                mainScreen.GetComponent<CanvasGroup>().alpha, 0.0f, Time.deltaTime * 5.0f
            );
        }

        if (isMainScreen && mainScreen.GetComponent<CanvasGroup>().alpha < 0.01f)
        {
            mainScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(
                mainScreen.GetComponent<CanvasGroup>().alpha, 1.0f, Time.deltaTime * 5.0f
            );
        }
    }

    public void StartGame()
    {
        if (selectScreen.GetComponent<CanvasGroup>().alpha > 0.9f)
        {
            scene = sceneSlider.currentSlide + 1;
            GameStatic.Track = (Track)trackSlider.currentSlide;
            GameStatic.Dance = (Dance)danceSlider.currentSlide;
            GameStatic.Character = (Character)charSlider.currentSlide;
            GameStatic.SongId = trackSlider.slides[trackSlider.currentSlide].GetComponent<SongController>().id;
            GameStatic.Location = (Location)(scene - 1);
            actionSound.time = 0.1f;
            actionSound.Play();
            StartCoroutine("CStart");
        }
    }

    IEnumerator CStart()
    {
        selectScreen.SetActive(false);
        loadingScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }
}
