using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfiniteSlider : MonoBehaviour {

    public RectTransform container;
    public RectTransform[] slides;
    public RectTransform center;

    public float[] distance;
    public float[] distReposition;

    public int slidesLength;

    public int startSlide = 1;
    public int currentSlide = 1;

    public float velocity;
    public float minVelocity = 3.0f;

    public float lerpValue = 3.0f;
    public bool isZoom = false;
    public float exWith = 300.0f;
    public float exMultiplier = 2.0f;

    private bool dragging = false;
    private int slideDistance;
    private int minSlideNum;
    
    private Vector3 lastPos;

    // Use this for initialization
    void Start () {
        slidesLength = slides.Length;
        distance = new float[slidesLength];
        distReposition = new float[slidesLength];

        slideDistance = (int)Mathf.Abs(slides[1].GetComponent<RectTransform>().anchoredPosition.x 
            - slides[0].GetComponent<RectTransform>().anchoredPosition.x);
        container.anchoredPosition = new Vector2((startSlide - 1) * -300, 0f);

        lastPos = container.anchoredPosition;
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < slides.Length; i++)
        {
            distReposition[i] = center.GetComponent<RectTransform>().position.x - slides[i].GetComponent<RectTransform>().position.x;
            distance[i] = Mathf.Abs(distReposition[i]);

            if (distReposition[i] > slides[0].rect.width * exMultiplier)
            {
                float curX = slides[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = slides[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPOs = new Vector2(curX + (slidesLength * slideDistance), curY);
                slides[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPOs;
            }

            if (distReposition[i] < -slides[0].rect.width * exMultiplier)
            {
                float curX = slides[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = slides[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPOs = new Vector2(curX - (slidesLength * slideDistance), curY);
                slides[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPOs;
            }
        }

        float minDistance = Mathf.Min(distance);

        for (int i = 0; i < slides.Length; i++)
        {
            if (minDistance == distance[i])
            {
                minSlideNum = i;
                currentSlide = minSlideNum;
            }
        }

        if (!dragging)
        {
            velocity = Mathf.Abs(container.anchoredPosition.x - lastPos.x);
            if (velocity <= minVelocity)
                LerpToSlide(-slides[minSlideNum].GetComponent<RectTransform>().anchoredPosition.x);
        }

        lastPos = container.anchoredPosition;

        if (isZoom)
        {
            for (int i = 0; i < slidesLength; i++)
            {
                if (i == currentSlide)
                    slides[i].localScale = Vector2.Lerp(slides[i].localScale, new Vector2(1.0f, 1.0f), Time.deltaTime * 10.0f);
                else
                    slides[i].localScale = Vector2.Lerp(slides[i].localScale, new Vector2(0.8f, 0.8f), Time.deltaTime * 10.0f);
            }
        }
    }

    void LerpToSlide(float position)
    {
        float newX = Mathf.Lerp(container.anchoredPosition.x, position, Time.deltaTime * lerpValue);
        Vector2 newPosition = new Vector2(newX, container.anchoredPosition.y);

        container.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    public void GoToSlide(int index)
    {
        minSlideNum = index - 1;
    }
}
