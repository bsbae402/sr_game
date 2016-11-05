using UnityEngine;
using System.Collections;

public class arrowScript : MonoBehaviour {
    
	// Called proportionate to gamespeed
	void FixedUpdate () {
        GetComponent<RectTransform>().localPosition += new Vector3(0, 4, 0);
        if (GetComponent<CanvasGroup>().alpha < 1)
            GetComponent<CanvasGroup>().alpha += 0.05f;
    }

}
