using UnityEngine;
using System.Collections;

public class prizeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(transform.position, Vector3.up, 1.4f);
	}
}
