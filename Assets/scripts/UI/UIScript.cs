using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour {

    // We'll implement this later
    UIHealthScript health;
    public GameObject TimeText;
    public GameObject ScoreText;

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
        health = GetComponentInChildren<UIHealthScript>();
	}

    public void hit(int damage) {
        health.hit(damage);
    }

    public int getHealth() {
        return health.health;
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
