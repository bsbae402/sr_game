using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hitMarkerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int randImage = (int)(Random.Range(0, 3));
        string image = "FistMarker-1";
        switch (randImage) {
            case 0: image = "FistMarker-1"; break;
            case 1: image = "FistMarker-2"; break;
            case 2: image = "FootMarker-1"; break;
            case 3: image = "FootMarker-2"; break;
        }
        GetComponent<Image>().sprite = Resources.Load<Sprite>(image);
        GetComponent<RectTransform>().Rotate(new Vector3(0, 0, Random.Range(-90, 90)));
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        GetComponent<RectTransform>().localScale = Vector3.one;
        GetComponent<RectTransform>().localPosition += new Vector3(Random.Range(-250, 250), Random.Range(-200, 50));
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<CanvasGroup>().alpha -= 0.02f;
        if (GetComponent<CanvasGroup>().alpha <= 0)
            Destroy(gameObject);
	}
}
