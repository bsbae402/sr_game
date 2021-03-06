﻿using UnityEngine;
using System.Collections;

public class menuBackgroundScript : MonoBehaviour {
    // I'm not sure why this has a complex name; this is just for moving the background texture
    
    public Vector2 speed = new Vector2(0, 0);
    public Vector2 bouncer = new Vector2(0, 0);

    Vector3 initScale;

    void Start() {
        initScale = transform.localScale;
    }
	
	void FixedUpdate () {
        GetComponent<MeshRenderer>().material.mainTextureOffset
            = new Vector2(Time.time * speed.x, Time.time * speed.y);
        transform.localScale = new Vector3(initScale.x + bouncer.x * (float)System.Math.Sin(Time.time), 
            initScale.y + bouncer.x * (float)System.Math.Sin(Time.time));

    }
}
