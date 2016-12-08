using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour {
    
    UIHealthScript health;
    public GameObject TimeText;
    public GameObject ScoreText;
    public GameObject Overhead;
    completionImageScript Completion;
    powerupImageScript Powerup;

    [HideInInspector]
    public float timeLeft;
    public bool decreaseTime;

    // All this just for aesthetic purposes
    int currentScore;
    int scoreincrement;
    int score;
    bool scoreUpdate;

	// Use this for initialization
	void Start() {
        currentScore = 0;
        scoreincrement = 0;
        score = 0;
        scoreUpdate = false;
        decreaseTime = true;
        health = GetComponentInChildren<UIHealthScript>();
        Completion = GetComponentInChildren<completionImageScript>();
        Powerup = GetComponentInChildren<powerupImageScript>();
    }

    public void hit(int damage) {
        health.hit(damage);
    }

    public int getHealth() {
        return health.health;
    }

    public void requestCompletionImage(int actType) {
        if (actType < 0)
            Completion.spawnCompletion("VelvetMinigameFailed");
        else {
            Completion.spawnCompletion("VelvetMinigameComplete-" + actType);
        }
    }

    public void requestPowerupImage(int power) { 
        Powerup.spawnPowerup("Powerup-" + power);
    }

    // This is called by the player to update the text for the score
    // A lot more went into this than needed because of the slow score increase
    public void updateScore(int score) {
        if (scoreUpdate)
            return;
        this.score = score;
        scoreincrement = (int)((this.score - currentScore) * 0.02);
        if (scoreincrement == 0)
            scoreincrement = 1;
        scoreUpdate = true;
    }

    void FixedUpdate() {
        if (decreaseTime) {
            if (timeLeft > 0)
                timeLeft -= Time.fixedDeltaTime;
            else
                timeLeft = 0;
        }
        TimeText.GetComponent<Text>().text = "" + timeLeft.ToString("n2");
        // If the score needs updating, it will set our score slowly to where it needs to be
        // This is so much code just for updating the score..
        if (scoreUpdate) {
            if (currentScore > score) {
                currentScore = score;
                scoreUpdate = false;
            }
            if (currentScore < score && score - currentScore > 10) {
                currentScore += scoreincrement;
            } else if( score - currentScore <= 10) {
                currentScore = score;
                scoreUpdate = false;
            }
            ScoreText.GetComponent<Text>().text = "" + currentScore;
        }
    }
}
