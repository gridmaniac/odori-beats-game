using UnityEngine;
using System.Collections;

public class ImgRotR : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<RectTransform>().Rotate(Vector3.forward, 5.0f);
    }
}
