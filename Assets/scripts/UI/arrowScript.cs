using UnityEngine;
using System.Collections;

public class arrowScript : MonoBehaviour {
    // This is specifically for minigame 0 - Alleyway Navigation
    // DO NOT USE ARROWS ANYWHERE ELSE

    // What type of arrow is it?
    // 0 - Up Arrow
    // 1 - Right Arrow
    // 2 - Left Arrow
    public int arrowType;

    // Records whether the arrow was hit or not
    // If not, we'll send feedback for a Boo to the overhead
    [HideInInspector]
    public bool hit = false;

    // Records if the arrow is "dead"
    // Which means scheduled for deletion
    // Otherwise the arrow will just keep updating
    bool dead = false;

    // When the arrow is out of range we want to get rid of it to save some space
    IEnumerator die() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // Calculate the score of the arrow press timing, and send the information back to the overhead
    void calculateScore(float dist) {
        if (Mathf.Abs(200 - transform.localPosition.y) > 75)
            return;
        // 0 - The actual rating of the press i.e. Perfect, Great, Good, Almost, Boo
        // 1 - The score to add to the act performance in minigameOverhead
        int[] feedback = new int[2];
        // Marvelous
        if (dist < 10) {
            feedback[0] = 0;
            feedback[1] = 100;
            Destroy(gameObject);
            audioManagerScript.instance.playfxSound(9);
        }
        // Perfect
        else if (dist < 20) { 
            feedback[0] = 1;
            feedback[1] = 66;
            Destroy(gameObject);
            audioManagerScript.instance.playfxSound(9);
        }
        // Great
        else if (dist < 35) { 
            feedback[0] = 2;
            feedback[1] = 33;
            Destroy(gameObject);
            audioManagerScript.instance.playfxSound(9);
        }
        // Good
        else if (dist < 50) { 
            feedback[0] = 3;
            feedback[1] = 10;
            audioManagerScript.instance.playfxSound(9);
        }
        // Almost
        else if (dist < 70) {
            feedback[0] = 4; // 4
            feedback[1] = 10; // 0
            audioManagerScript.instance.playfxSound(12);
        }
        hit = true;
        minigameOverheadScript.instance.miniFeedback(feedback);
        if (dist < 50) {
        }
    }

    // Instead of testing through update, the conditions are handled in minigameOverhead.
    // We don't need to check any criteria twice was a result.
    public void hitArrow() {
        calculateScore(Mathf.Abs(200 - transform.localPosition.y));
    }

    // Input has to be checked in regular time in comparison to the rest of the data
    // This allows it to be registered 100% of the time
    void Update() { 
        // Detect how close the player clicked the arrow
        /*if (arrowType == 0) { 
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                calculateScore(Mathf.Abs(200 - transform.localPosition.y));
            }
        } else if (arrowType == 1) { 
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                calculateScore(Mathf.Abs(200 - transform.localPosition.y));
            }
        } else if (arrowType == 2) { 
            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                calculateScore(Mathf.Abs(200 - transform.localPosition.y));
            }
        }*/
    }

	// Called proportionate to gamespeed
    // We have to detect how close the player pressed the corresponding button in the arrow itself
	void FixedUpdate () {
        // The arrow rises
        GetComponent<RectTransform>().localPosition += 
            new Vector3(0, levelConstructionScript.instance.speed * 10, 0);

        // Don't update the rest of the arrow if the arrow is dead
        if (dead)
            return;

        // Fades into existance for a slick interface
        if (GetComponent<CanvasGroup>().alpha < 1)
            GetComponent<CanvasGroup>().alpha += 0.05f;

        // If the note has passed the point of pressing, it will eventually disappear
        // It will also send a boo for feedback
        if (200 - transform.localPosition.y < -60) {
            if (hit) {
                dead = true;
                StartCoroutine(die());
                return;
            }
            // 5 - BOO, 15 - 15 Damage
            int[] feedback = { 5, 15 };
            minigameOverheadScript.instance.miniFeedback(feedback);
            dead = true;
            hit = true;
            StartCoroutine(die());
        }
    }

}
