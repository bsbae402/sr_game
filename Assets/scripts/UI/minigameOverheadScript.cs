using UnityEngine;
using System.Collections;

public class minigameOverheadScript : MonoBehaviour {

    // The overhead pieces used for different minigames
    // Unfortunately there's no optimized and generalized algorithm for sorting them
    // We'll have to hardcode their usage, plus with additional minigames
    public GameObject[] components;

    // Another hard coded piece for a single minigame..
    // There's no other way to do this unfortunately
    public RectTransform[] arrows;
    
    // Sets all the components invisible before the selection of a new minigame
    void resetComponents() {
        for (int i = 0; i < components.Length; i++)
            components[0].GetComponent<CanvasGroup>().alpha = 0;
    }

    // Called by levelConstruction, we'll switch the displayed minigame
    // gameData's contents are completely ambiguous, depending on the minigame
    // Completely situational and hardcoded behavior
    // This is the worst
	public void newAct(int actType, int[] gameData) {
        resetComponents();
        // Minigame is Alleyway-Navigation
        if (actType == 0) {
            components[0].GetComponent<CanvasGroup>().alpha = 1;
            int dist = 0;
            int type = 0;
            float x = 0;
            for (int i = 0; i < gameData.Length; i++) {
                dist = gameData[i] % 100;
                type = gameData[i] / 100;
                if (type == 0)
                    x = -247;
                else if (type == 1)
                    x = -157f;
                else if (type == 2)
                    x = -335f;
                var arrow = Instantiate(arrows[type]) as RectTransform;
                arrow.SetParent(GetComponent<RectTransform>());
                arrow.localScale = new Vector3(1, 1, 1);
                arrow.localPosition = Vector3.zero;
                // I swear to god this is the worst
                arrow.Translate(new Vector3(x, -60 * dist + 60) / 474.68f);
            }
        }
    }

    void FixedUpdate() {

    }

}
