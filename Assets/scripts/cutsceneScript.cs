using UnityEngine;
using System.Collections;

public class cutsceneScript : MonoBehaviour {

    public GameObject[] slides;

	void Start () {
        StartCoroutine(slideshow());
	}
    IEnumerator slideshow() {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < slides.Length; i++) {
            resetSlides();
            slides[i].GetComponent<CanvasGroup>().alpha = 1;
            yield return new WaitForSeconds(4f);
        }
        if (GameObject.FindGameObjectWithTag("levelInit") != null)
            GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                loadAppear(GameObject.FindGameObjectWithTag("levelInit").GetComponent<levelInitScript>().sceneName);
        else
            GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                loadAppear("Level");
    }

    public void resetSlides() {
        for (int i = 0; i < slides.Length; i++) {
            slides[i].GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    void Update() { 
        if (Input.anyKeyDown) {
            StopCoroutine(slideshow());
            if (GameObject.FindGameObjectWithTag("levelInit") != null)
                GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                    loadAppear(GameObject.FindGameObjectWithTag("levelInit").GetComponent<levelInitScript>().sceneName);
            else
                GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                    loadAppear("Level");
        }
    }

}
