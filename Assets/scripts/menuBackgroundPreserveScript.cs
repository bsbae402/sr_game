using UnityEngine;
using System.Collections;

public class menuBackgroundPreserveScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
}
