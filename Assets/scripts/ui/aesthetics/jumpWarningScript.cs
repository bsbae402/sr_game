using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class jumpWarningScript : MonoBehaviour {

    int animate;
    bool warned;

	void Start () {
        animate = 0;
        warned = false;
	}
	
	void FixedUpdate () {
	    if (animate % 15 == 7) {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("JumpWarningHurdle-2");
        } else if (animate % 15 == 14) {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("JumpWarningHurdle-1");
            animate = 0;
        }
        animate++;
        if (GetComponent<CanvasGroup>().alpha == 1 && !warned) {
            GetComponent<AudioSource>().Play();
            warned = true;
        } else if (GetComponent<CanvasGroup>().alpha == 0) { 
            GetComponent<AudioSource>().Stop();
            warned = false;
        }
	}
}
