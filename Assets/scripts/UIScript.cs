using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {

    UIHealthScript health;

	// Use this for initialization
	void Start() {
        health = GetComponentInChildren<UIHealthScript>();
	}

    void Update() {
        if(Input.GetKeyDown("left shift")) {
            health.health -= 25;
            if (health.health < 0)
                health.health = 100;
        }
    }
}
