using UnityEngine;
using System.Collections;

public class menuTransitionBouncerScript : MonoBehaviour {
    
    public Vector2 bouncer = new Vector2(0, 0);

    Vector3 initScale;

    void Start()
    {
        initScale = transform.localScale;
    }

    void FixedUpdate() {
        transform.localScale = new Vector3(initScale.x + bouncer.x * (float)System.Math.Sin(Time.time),
            initScale.y + bouncer.y * (float)System.Math.Sin(Time.time));

    }
}
