using UnityEngine;
using System.Collections;

public class levelListButtonHandler : MonoBehaviour {

    public GameObject levelList;

    void Start() {
    }

	public void levelView(int selected) {
        levelList.GetComponent<levelListScript>().requestUpdate(selected);
    }

    public void scrollUp() {
        levelList.GetComponent<levelListScript>().scrollnum(-1);
        levelList.GetComponent<levelListScript>().requestUpdate();
    }

    public void scrollDown() {
        levelList.GetComponent<levelListScript>().scrollnum(1);
        levelList.GetComponent<levelListScript>().requestUpdate();
    }

    public void startLevel() {
        if (levelList.GetComponent<levelListScript>().levelSelected()) {
            GameObject level = levelList.GetComponent<levelListScript>().getLevel();
            DontDestroyOnLoad(level);
            GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                loadAppear(level.GetComponent<levelInitScript>().sceneName);
        }
    }

}
