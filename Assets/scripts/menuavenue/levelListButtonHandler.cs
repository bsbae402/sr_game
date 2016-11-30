using UnityEngine;
using System.Collections;

public class levelListButtonHandler : MonoBehaviour {
    // This is full of functions for the different buttons of the level list
    // Each menuAvenue tab has its own handler

    // Pressing a level on the level list will update the description box
    // (and eventually the picture preview)
	public void levelView(int selected) {
        levelListScript.instance.requestUpdate(selected);
        audioManagerScript.instance.playfxSound(8);
    }

    // If and when we have more than five stages, we can scroll through them
    public void scrollUp() {
        levelListScript.instance.scrollnum(-1);
        levelListScript.instance.requestUpdate();
        audioManagerScript.instance.playfxSound(8);
    }
    public void scrollDown() {
        levelListScript.instance.scrollnum(1);
        levelListScript.instance.requestUpdate();
        audioManagerScript.instance.playfxSound(8);
    }

    // Pressing start when we have a level selected will use our transitioner to load the level discretely
    public void startLevel() {
        if (levelListScript.instance.levelSelected()) {
            GameObject level = levelListScript.instance.getLevel();
            DontDestroyOnLoad(level);
            /*GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                loadAppear(level.GetComponent<levelInitScript>().sceneName);*/
            GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                loadAppear("Cutscene");
            audioManagerScript.instance.playfxSound(8);
        }
    }

}
