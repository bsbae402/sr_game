using UnityEngine;
using System.Collections;

public class creditsScript : MonoBehaviour {

    bool move;

	void Start () {
        if (audioManagerScript.instance != null)
            audioManagerScript.instance.playMusic(3);
        move = true;
        StartCoroutine(leave());
	}

    IEnumerator leave() {
        yield return new WaitForSecondsRealtime(83f);
        move = false;
        GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
            loadAppear("MenuAvenue");
    }
	
	void Update () {
        if (!move)
            return;
        if (GetComponent<RectTransform>().localPosition.y >= 3600)
            move = false;
        GetComponent<RectTransform>().localPosition += new Vector3(0, 0.8f, 0);
    }
}
