using UnityEngine;
using System.Collections;

public class Rotor : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * 5.0f);
	}
}
