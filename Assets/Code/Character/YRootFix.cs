using UnityEngine;
using System.Collections;

public class YRootFix : MonoBehaviour {

    private Vector3 inPos;
    private float inY;
	// Use this for initialization
	void Start () {
        inPos = transform.position;
        inY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, inY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, inPos, Time.deltaTime * 0.2f);
	}
}
