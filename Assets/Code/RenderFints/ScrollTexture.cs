using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour {

    public float scrollSpeed = 2f;
    private float offset;
 
    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed);
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, offset));

    }
}
