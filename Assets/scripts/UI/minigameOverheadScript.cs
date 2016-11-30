using UnityEngine;
using System.Collections;

public class minigameOverheadScript : MonoBehaviour {

    public static minigameOverheadScript instance = null;

    // The overhead pieces used for different minigames
    // Unfortunately there's no optimized and generalized algorithm for sorting them
    // We'll have to hardcode their usage, plus with additional minigames
    public GameObject[] components;
    Quaternion previousLeverRotation;

    // Another hard coded piece for a single minigame..
    // There's no other way to do this unfortunately
    // Holds the base prefabs for arrows
    public RectTransform[] arrows;
    // In order to prevent key presses to count for several notes, we have to keep track of
    // the arrows we make.
    arrowScript[] activeArrows;
    // Hard coded piece for the beat-em-up minigame
    float obstaclehealth;

    // UI components used to show tutorials 
    public GameObject[] tutorials;
    public GameObject[] powerTutorials;

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

    void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start() {
        previousLeverRotation = components[6].transform.rotation;
    }

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
        if (score.gameObject == null)
            return;
        // Arrows provide feedback for the alleyway navigation minigame
        if (currentAct == 0) {
            if (feedbackData[0] >= 4) {
                playerScript.instance.getHit(feedbackData[1]);
                return;
            }
            score.GetComponent<performanceScript>().score += feedbackData[1];
            // If the player gets a boo, they crash into the wall
        }
        else if (currentAct == 1) {
            components[2].transform.localPosition += new Vector3(-800 / obstaclehealth, 0, 0);
            if (playerScript.instance.powerActive[1])
                components[2].transform.localPosition += new Vector3(-800 / obstaclehealth, 0, 0);
            if (feedbackData.Length == 2) {
                components[2].transform.localPosition += new Vector3(-800, 0, 0);
            }
            actPerformance += feedbackData[0];
            hitMark();
            if (components[2].transform.localPosition.x <= -800) {
                score.GetComponent<performanceScript>().score += actPerformance;
                actPerformance = 0;
                playerScript.instance.gameData[1] = 1;
                playerScript.instance.stop = 0;
                playerScript.instance.UI.decreaseTime = false;
            }
        }
        else if (currentAct == 2) {
            if (feedbackData[0] == 0) {
                components[5].GetComponent<CanvasGroup>().alpha = 1;
            } else if (feedbackData[0] == 1) {
                score.GetComponent<performanceScript>().score += 100;
                components[5].GetComponent<CanvasGroup>().alpha = 0;
            } else if (feedbackData[0] == 2) { 
                components[5].GetComponent<CanvasGroup>().alpha = 0;
            }
        }
        else if (currentAct == 3) {
            score.GetComponent<performanceScript>().score += 800;
        }
        else if (currentAct == 4) {
            score.GetComponent<performanceScript>().score += 100;
        }
        else if (currentAct == 5) {
            score.GetComponent<performanceScript>().score += feedbackData[0];
        }
        else if (currentAct == 6) {
            score.GetComponent<performanceScript>().score += 100;
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
    IEnumerator finishPuffs() {
        float rand = 0;
        for (int i = 0; i < 15; i++) {
            rand = Random.Range(0, 100);
            var puff = Instantiate(aesthetics[rand > 50 ? 2 : 3]) as RectTransform;
            puff.SetParent(GetComponent<RectTransform>());
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 15; i++) {
            rand = Random.Range(0, 100);
            var puff = Instantiate(aesthetics[rand > 50 ? 2 : 3]) as RectTransform;
            puff.SetParent(GetComponent<RectTransform>());
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 15; i++) {
            rand = Random.Range(0, 100);
            var puff = Instantiate(aesthetics[rand > 50 ? 2 : 3]) as RectTransform;
            puff.SetParent(GetComponent<RectTransform>());
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 15; i++) {
            rand = Random.Range(0, 100);
            var puff = Instantiate(aesthetics[rand > 50 ? 2 : 3]) as RectTransform;
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
        switch (actType) {
            // Alleyway-Navigation
            case 0:
                components[0].GetComponent<CanvasGroup>().alpha = 1;
                int dist = 0;
                int type = 0;
                float x = 0;
                activeArrows = new arrowScript[gameData.Length];
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
                    activeArrows[i] = arrow.GetComponent<arrowScript>();
                    arrow.SetParent(GetComponent<RectTransform>());
                    arrow.localScale = new Vector3(1, 1, 1);
                    arrow.localPosition = Vector3.zero;
                    // I swear to god this is the worst
                    arrow.localPosition += new Vector3(x, -60 * dist + 120);
                }
            break;
            // Obstacle Beatup
            case 1:
                // Show the health bar
                components[1].GetComponent<CanvasGroup>().alpha = 1;
                components[2].GetComponent<CanvasGroup>().alpha = 1;
                // Set obstaclehealth as total health of the obstacle
                obstaclehealth = gameData[0];
            break;
            // Hurdle Jump
            case 2:

            break;
            // Silent Crossing
            case 3:
                StartCoroutine(playerScript.instance.stopPlayer(gameData[0]));
            break;
            // Look Out
            case 4:
                playerScript.instance.useGameData(gameData);
            break;
            // Drawbridge
            case 5:
                obstaclehealth = gameData[0];
                actPerformance = gameData[0];
                components[6].GetComponent<CanvasGroup>().alpha = 1;
                components[7].GetComponent<CanvasGroup>().alpha = 1;
                break;
            case 6:
                //// player.gameData[] : [0~3] are unusable, but we can use from 4 to 19
                //// use them for the door types (push=0, pull=1)
                playerScript.instance.useGameData(gameData);
            break;
            // Ending scene
            // We need a lot of congratulatory effects
            case 1000:
                StartCoroutine(finishPuffs());
                components[3].GetComponent<CanvasGroup>().alpha = 1;
                components[3].GetComponent<CanvasGroup>().blocksRaycasts = true;
                components[4].GetComponent<CanvasGroup>().alpha = 1;
                components[4].GetComponent<CanvasGroup>().blocksRaycasts = true;
            break;
        }
    }
    public void showPowerTutorial() {
        audioManagerScript.instance.playfxSound(13);
        Time.timeScale = 0.1f;
        powerTutorials[0].GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(stopPowerTutorial());
    }
    IEnumerator stopPowerTutorial() {
        yield return new WaitForSecondsRealtime(4f);
        Time.timeScale = levelConstructionScript.instance.levelSpeed;
        powerTutorials[0].GetComponent<CanvasGroup>().alpha = 0;
    }

    public void showTutorial(int act) {
        audioManagerScript.instance.playfxSound(13);
        Time.timeScale = 0.1f;
        tutorials[act].GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(stopTutorial(act));
    }
    IEnumerator stopTutorial(int act) {
        yield return new WaitForSecondsRealtime(4f);
        Time.timeScale = levelConstructionScript.instance.levelSpeed;
        tutorials[act].GetComponent<CanvasGroup>().alpha = 0;
    }

    public int increaseScore(int scoring) {
        score.GetComponent<performanceScript>().score += scoring;
        return score.GetComponent<performanceScript>().score;
    }
    public int getScore() {
        return score.GetComponent<performanceScript>().score;
    }
    
    void Update() {
        // Unfortunately the calculation if an arrow is hit must happen here.
        // This is because a single arrow press may count as hitting multiple arrows if the handling is left to arrows.
        // This means we have to do the detection ourselves.
        // Needs a lot of overhead I feel; I hope the player doesn't spam the keys.
        if (currentAct == 0) {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                for (int i = 0; i < activeArrows.Length; i++) { 
                    if(activeArrows[i].arrowType == 0 && 
                        !activeArrows[i].hit) {
                        activeArrows[i].hitArrow();
                        return;
                    }
                }
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                for (int i = 0; i < activeArrows.Length; i++) { 
                    if(activeArrows[i].arrowType == 1 && 
                        !activeArrows[i].hit) {
                        activeArrows[i].hitArrow();
                        return;
                    }
                }
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                for (int i = 0; i < activeArrows.Length; i++) { 
                    if(activeArrows[i].arrowType == 2 && 
                        !activeArrows[i].hit) {
                        activeArrows[i].hitArrow();
                        return;
                    }
                }
            }
        } else if (currentAct == 5) {
            if (5 - 4.5f * (1 - obstaclehealth / actPerformance) <= 0.5 && playerScript.instance.stop != 0) {
                components[6].transform.localScale = new Vector3(
                    5 - 4.5f * (1 - obstaclehealth / actPerformance), 5 - 4.5f * (1 - obstaclehealth / actPerformance), 0);
                playerScript.instance.stop = 0;
                playerScript.instance.gameData[0] = 20001;
                playerScript.instance.gameData[1] = actPerformance / 3;
            } else if(playerScript.instance.stop != 0) {
                components[6].transform.localScale = new Vector3(
                    5 - 4.5f * (1 - obstaclehealth / actPerformance), 5 - 4.5f * (1 - obstaclehealth / actPerformance), 0);
                components[6].transform.localEulerAngles = new Vector3(0, 0,
                    Mathf.Atan2(((Input.mousePosition.y - Screen.height / 2f) - components[6].transform.localPosition.y),
                    ((Input.mousePosition.x - Screen.width / 2f) - components[6].transform.position.x)) * Mathf.Rad2Deg - 90);
                obstaclehealth -= (int)Quaternion.Angle(components[6].transform.rotation, previousLeverRotation);
                previousLeverRotation = components[6].transform.rotation;
            }
        }
    }

}
