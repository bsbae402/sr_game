using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHealthScript : MonoBehaviour {

    public int health = 100;

    int rotateCounter;

    void Start() {
        rotateCounter = 0;
    }

    // PURPOSELY USED Update()
    // It ignores the game speed
	void Update () {
        if (rotateCounter % 60 == 29) {
            GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 30));
        } else if(rotateCounter % 60 == 59) {
            GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -30));
            rotateCounter = 0;
        }
        rotateCounter++;

        if( health >= 75 )
            GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadOkay");
        else if( health >= 50)
            GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadMeh");
        else if (health >= 25)
            GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadErr");
        else
            GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadOuch");

    }
}
