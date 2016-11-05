using UnityEngine;
using System.Collections;

public class levelListButtonHandler : MonoBehaviour {
    // This is full of functions for the different buttons of the level list
    // Each menuAvenue tab has its own handler

    // We'll keep track of the list container for calling ease
    public GameObject levelList;

    // Pressing a level on the level list will update the description box
    // (and eventually the picture preview)
	public void levelView(int selected) {
        levelList.GetComponent<levelListScript>().requestUpdate(selected);
    }

    // If and when we have more than five stages, we can scroll through them
    public void scrollUp() {
        levelList.GetComponent<levelListScript>().scrollnum(-1);
        levelList.GetComponent<levelListScript>().requestUpdate();
    }
    public void scrollDown() {
        levelList.GetComponent<levelListScript>().scrollnum(1);
        levelList.GetComponent<levelListScript>().requestUpdate();
    }

    // Pressing start when we have a level selected will use our transitioner to load the level discretely
    public void startLevel() {
        if (levelList.GetComponent<levelListScript>().levelSelected()) {
            GameObject level = levelList.GetComponent<levelListScript>().getLevel();
            DontDestroyOnLoad(level);
            GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                loadAppear(level.GetComponent<levelInitScript>().sceneName);
        }
    }

}
