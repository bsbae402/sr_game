using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHealthScript : MonoBehaviour {
    
    // Default health for Velvet
    public int health = 100;

    // A variable made for aesthetic purposes
    int rotateCounter;

    void Start() {
        rotateCounter = 0;
    }

    public void hit(int damage) {
        if (health - damage <= 0 && playerScript.instance.powerActive[2]) {
            health = 50;
            playerScript.instance.powerActive[2] = false;
            playerScript.instance.UI.requestPowerupImage(2);
            audioManagerScript.instance.playfxSound(15);
            return;
        }
        health -= damage;
        if (health < 0)
            health = 0;
    }

    // PURPOSELY USED Update()
    // It ignores the game speed
	void Update () {
        // Aesthetic rotating
        if (rotateCounter % 60 == 29) {
            GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 30));
        } else if(rotateCounter % 60 == 59) {
            GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -30));
            rotateCounter = 0;
        }
        rotateCounter++;

        if (playerScript.instance.powerActive[1]) {
            if (playerScript.instance.actType == 1 || playerScript.instance.actType == 2 || playerScript.instance.actType == 4) {
                GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadOni");
                return;
            }
        }

        // When Velvet loses health her appearance will change
        if( health >= 1000 )
            GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadYay");
        else if( health >= 75 )
            GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadOkay");
        else if( health >= 50)
            GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadMeh");
        else if (health >= 25)
            GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadErr");
        else
            GetComponent<Image>().sprite = Resources.Load<Sprite>("VelvetUIHeadOuch");

    }
}
