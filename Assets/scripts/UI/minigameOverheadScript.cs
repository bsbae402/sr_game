using UnityEngine;
using System.Collections;

public class minigameOverheadScript : MonoBehaviour {

    // The overhead pieces used for different minigames
    // Unfortunately there's no optimized and generalized algorithm for sorting them
    // We'll have to hardcode their usage, plus with additional minigames
    public GameObject[] components;

    // Another hard coded piece for a single minigame..
    // There's no other way to do this unfortunately
    // Holds the base prefabs for arrows
    public RectTransform[] arrows;
    // Hard coded piece for the beat-em-up minigame
    int obstaclehealth;

    // Not that important
    // Transforms used for decoration of the level
    /****AESTHETIC TYPES****
    0 = Overhead minigame appearance puffs
    ***********************/
    public RectTransform[] aesthetics;

    // We keep a copy of the current type of minigame we're in
    // So we can handle feedback in a single function
    int currentAct;
    // This is how well the player accomplishes each minigame
    public int actPerformance = 0;
    // This is how well the player does as a whole for the level
    public GameObject score;

    // Sets all the components invisible before the selection of a new minigame
    void resetComponents() {
        for (int i = 0; i < components.Length; i++)
            components[i].GetComponent<CanvasGroup>().alpha = 0;
        components[2].transform.localPosition = Vector3.zero;
        components[2].transform.localPosition += new Vector3(0, 220);
    }

    // Whenever a minigame element updates, it will call this function with
    // yet another arbitrary set of data
    // It will increase the act performance as well as handle cute effects
    public void miniFeedback(int[] feedbackData) {
        // Arrows provide feedback for the alleyway navigation minigame
        if (currentAct == 0) {
            if (feedbackData[0] == 5) {
                transform.parent.parent.gameObject.GetComponent<playerScript>().getHit(feedbackData[1]);
                return;
            }
            score.GetComponent<performanceScript>().score += feedbackData[1];
            // If the player gets a boo, they crash into the wall
        }
        else if (currentAct == 1) {
            components[2].transform.localPosition += new Vector3(-800 / obstaclehealth, 0, 0);
            if (feedbackData.Length == 2) {
                components[2].transform.localPosition += new Vector3(-800, 0, 0);
            }
            actPerformance += feedbackData[0];
            hitMark();
            if (components[2].transform.localPosition.x <= -800) {
                score.GetComponent<performanceScript>().score += actPerformance;
                actPerformance = 0;
                transform.parent.parent.gameObject.GetComponent<playerScript>().stop = 0;
                transform.parent.parent.gameObject.GetComponent<playerScript>().gameData[1] = 1;
                transform.parent.GetComponent<UIScript>().decreaseTime = false;
            }
        }
    }

    void hitMark() {
        var hitMarker = Instantiate(aesthetics[1]) as RectTransform;
        hitMarker.SetParent(GetComponent<RectTransform>());
    }

    void puffOverhead() {
        for (int i = 0; i < 15; i++) {
            var puff = Instantiate(aesthetics[0]) as RectTransform;
            puff.SetParent(GetComponent<RectTransform>());
        }
    }

    // Called by levelConstruction, we'll switch the displayed minigame
    // gameData's contents are completely ambiguous, depending on the minigame
    // Completely situational and hardcoded behavior
    // This is the worst
	public void newAct(int actType, int[] gameData) {
        currentAct = actType;
        actPerformance = 0;
        resetComponents();
        puffOverhead();
        // Alleyway-Navigation
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
                arrow.localPosition += new Vector3(x, -60 * dist + 120);
            }
        }
        // Obstacle Beat-Em-Up
        else if (actType == 1) {
            // Show the health bar
            components[1].GetComponent<CanvasGroup>().alpha = 1;
            components[2].GetComponent<CanvasGroup>().alpha = 1;
            // Set obstaclehealth as total health of the obstacle
            obstaclehealth = gameData[0];
        }
    }

    public int increaseScore(int scoring) {
        score.GetComponent<performanceScript>().score += scoring;
        return score.GetComponent<performanceScript>().score;
    }
    public int getScore() {
        return score.GetComponent<performanceScript>().score;
    }

    void FixedUpdate() {

    }

}
