using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class tutorialScript : MonoBehaviour {

    public int actType;

    int counter;

	// Use this for initialization
	void Start () {
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<CanvasGroup>().alpha < 1)
            return;
        if (actType < 0)
            return;
	    if (counter % 30 == 14) {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Tutorial-" + (actType + 1) + "-1");
        } else if (counter % 30 == 29) {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Tutorial-" + (actType + 1) + "-2");
            counter = 0;
        }
        counter++;
	}
}
