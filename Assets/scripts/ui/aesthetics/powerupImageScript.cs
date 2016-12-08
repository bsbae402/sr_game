using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class powerupImageScript : MonoBehaviour {

    int rotateCounter;

    bool spawned;

    void Start() {
        rotateCounter = 0;
        spawned = false;
        GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -15));
    }

	public void spawnPowerup(string image) {
        if (spawned)
            return;
        GetComponent<Image>().sprite = Resources.Load<Sprite>(image);
        GetComponent<CanvasGroup>().alpha = 1;
        spawned = true;
        StartCoroutine(complete());
    }

    IEnumerator complete() {
        yield return new WaitForSeconds(1.5f);
        GetComponent<CanvasGroup>().alpha = 0;
        spawned = false;
    }
    void Update () {
        // Aesthetic rotating
        if (rotateCounter % 30 == 14) {
            GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 30));
        } else if(rotateCounter % 30 == 29) {
            GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -30));
            rotateCounter = 0;
        }
        rotateCounter++;

    }
}
