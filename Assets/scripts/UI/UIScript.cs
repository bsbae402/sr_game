using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {

    // We'll implement this later
    UIHealthScript health;

	// Use this for initialization
	void Start() {
        health = GetComponentInChildren<UIHealthScript>();
	}

    void Update() {
        
    }
}
