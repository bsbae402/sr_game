using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour {

    public static playerScript instance = null;

    // Current nodes and next nodes of movement
    // Used for camera rotation
    public GameObject nextNode;
    public GameObject currentNode;
    int nodes;

    [HideInInspector]
    public UIScript UI;

    // Used to send information between the player and the level
    [HideInInspector]
    public int[] gameData;
    // The current act (as in the tiles[] index) the player is in
    [HideInInspector]
    public int currentAct;
    // The current act (as in the type of minigame) the player is in
    [HideInInspector]
    public int actType;
    // The desired angle the player wants to end up facing depending on the current node
    [HideInInspector]
    public Vector3 angle;

    [HideInInspector]
    public bool[] powerActive;
    public int[] powerCooldown;

    [HideInInspector]
    // levelConstruction's record of the movement speed will slow down
    // when it detects that the player has been hit
    public bool hit = false;
    bool invincible = false;
    [HideInInspector]
    // Unfortunately we need to keep this in order for node detection to work
    // levelConstruction will have to continously update this value
    public float movementSpeed;
    [HideInInspector]
    // Detects whether the movement should stop or not
    // It's used in minigames such as Beat-Up
    public int stop = 0;
    [HideInInspector]
    // Used to process a failed stage inside update, so that update won't call it a million times
    public bool failedAct = false;
    [HideInInspector]
    // Used in Hurdle Jump to detect whether the player is jumping
    public bool jumping = false;
    float jumpheight = 0.2f;
    bool warned = false;
    [HideInInspector]
    // Used in Look Out to detect if player hits a person
    public int lane = 0;

    //# used in Norman Doors. Set if plyr pressed correct button before contact the door
    [HideInInspector]
    public bool doorOpening = false;
    //# used in Norman Doors. Set if plyr pressed wrong button
    [HideInInspector]
    public bool wrongInput = false;

    void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        UI = GetComponentInChildren<UIScript>();
        nodes = 1;
        currentAct = -1;
        actType = -1;
        angle = GetComponentInChildren<Camera>().transform.forward;
        gameData = new int[20];
        
        if (playerStats.instance != null) {
            powerActive = new bool[playerStats.instance.powers.Length];
            powerCooldown = new int[playerStats.instance.powers.Length];
            for(int i = 1; i < powerActive.Length; i++) {
                if (playerStats.instance.powers[i]) {
                    powerActive[i] = true;
                    powerCooldown[i] = 10000000;
                    break;
                }   
            }
        } else {
            powerActive = new bool[20];
            powerCooldown = new int[20];
        }
    }

    public Vector3 getPosition() {
        return transform.position;
    }
    public void setPosition(Vector3 pos) {
        transform.position = pos;
    }

    public void getHit(int damage, float invincibleTime, bool strong) {
        if (invincible)
            return;
        if (powerActive[3])
            damage /= 2;
        hit = strong;
        invincible = true;
        StartCoroutine(invincibility(invincibleTime));
        UI.hit(damage);
    }
    public void getHit(int damage, float invincibleTime) {
        getHit(damage, invincibleTime, true);
    }
    public void getHit(int damage) {
        getHit(damage, 0f);
    }
    IEnumerator invincibility(float time) {
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    public void powerCool() {
        for (int i = 0; i < powerActive.Length; i++) {
            powerCooldown[i]--;
            if (powerCooldown[i] <= 0) {
                powerCooldown[i] = 0;
                powerActive[i] = false;
            }
        }
        if (powerCooldown[0] == 4 && powerActive[0]) {
            getHit(-100, 0f, false);
            UI.requestPowerupImage(0);
            audioManagerScript.instance.playfxSound(15);
        }
    }

    // Used to stop the jumping mechanic after a small amount of time
    IEnumerator jump() {
        yield return new WaitForSeconds(0.5f);
        jumpheight = 0.2f;
        cameraScript.instance.transform.localPosition = new Vector3(0, 2.5f, 0);
        jumping = false;
    }

    // Stop the player for a certain amount of time 
    // ONLY FOR SILENT CROSSING
    public IEnumerator stopPlayer(float time) {
        yield return new WaitForSeconds(time);
        if (actType == 3)
            stop = 0;
    }

    // Uses an unused portion of the gameData to record information for the player
    // It is a prerequisite that data[] be of size 16 or less
    public void useGameData(int[] data) { 
        for(int i = 0; i < data.Length; i++) {
            gameData[4 + i] = data[i];
        }
    }

    // Called by level construction when we finish a minigame
    // It's the only way to get it to call ONCE and only ONCE
    public void finishAct() {
        if (actType < 0)
            return;
        if (UI.timeLeft <= 0 || failedAct) {
            UI.requestCompletionImage(-1);
            powerActive[0] = false;
            UI.hit(20);
            audioManagerScript.instance.playfxSound(11);
        } else {
            UI.requestCompletionImage(actType);
            audioManagerScript.instance.playfxSound(5);
        }
        UI.updateScore(minigameOverheadScript.instance.increaseScore((int)UI.timeLeft * 20));
        powerCool();
        nodes = 1;
    }

    // FixedUpdate moves with the game speed
	void FixedUpdate () {
        if (stop == 99 || stop == -99)
            return;
        // The stage has ended at this point
        if (UI.getHealth() <= 0)
            stop = -99;

        // Node Type 99 is the level completely finished
        if (nextNode.GetComponent<nodeScript>().nodeType == 99) { 
            if (Vector3.Distance(nextNode.transform.position, transform.position) < movementSpeed) {
                transform.position = nextNode.transform.position;
                UI.hit(-10000);
                stop = 99;
            }
        }
        // We'll detect collisions with nodes within the same act in player
        // Collisions with nodes in different acts are handled in levelConstruction
        // Updates desired angle, and the current and next nodes for the player
        else if (nextNode.GetComponent<nodeScript>().nodeType != 1) {
	        if (Vector3.Distance(nextNode.transform.position, transform.position) < movementSpeed) {
                nodes++;
                warned = false;
                transform.position = nextNode.transform.position;
                currentNode = nextNode;
                nextNode = nextNode.GetComponent<nodeScript>().nextNode;
                angle = currentNode.transform.forward;
                
                // If we hit a specific node, there may be some functions to run
                int nodeType = currentNode.GetComponent<nodeScript>().nodeType;
                switch (nodeType) {
                    case 2: audioManagerScript.instance.playfxSound(7); break;
                    case 4: stop = 1; break;
                    case 5:
                        if (cameraScript.instance.transform.localPosition.y < 3.2f) {
                            if (!powerActive[1])
                                getHit(10);
                            else
                                audioManagerScript.instance.playfxSound(14);
                            // Code 10000, 0 : Wreck current act's interactable obstacle
                            gameData[0] = 10000 + nodes - 2;
                            gameData[1] = 0;
                            int[] feedback = { 2 };
                            minigameOverheadScript.instance.miniFeedback(feedback);
                        } else {
                            int[] feedback = { 1 };
                            minigameOverheadScript.instance.miniFeedback(feedback);
                        }
                    break;
                    case 6: stop = -1; break;
                    case 7:
                        if (failedAct) {
                            getHit(30);
                        } else { 
                            int[] feedback = { 0 };
                            minigameOverheadScript.instance.miniFeedback(feedback);
                        }
                        StopCoroutine("stopPlayer");
                    break;
                    case 8:
                        if (gameData[nodes + 2] / 100 == 1 && lane == -1 || 
                            (gameData[nodes + 2] % 100) / 10 == 1 && lane == 0 ||
                            gameData[nodes + 2] % 10 == 1 && lane == 1) {
                            if (!powerActive[1])
                                getHit(10);
                            else
                                audioManagerScript.instance.playfxSound(14);
                            // Code 10000, 0 : Shake current act's interactable obstacle
                            gameData[0] = 10000 + nodes - 2;
                            gameData[1] = 0;
                        } else { 
                            int[] feedback = { 0 };
                            minigameOverheadScript.instance.miniFeedback(feedback);
                        }
                    break;
                    case 9:
                        stop = 9;
                    break;
                    case 10:
                        if(!doorOpening)
                            stop = 10;
                        else {
                            warned = false;
                            doorOpening = false;
                        }
                    break;
                }
                if (nodeType != 8)
                    lane = 0;
            } else if (nextNode.GetComponent<nodeScript>().nodeType == 5) {
                if (!warned) {
                    if (Vector3.Distance(nextNode.transform.position, transform.position) < 10.0f) {
                        int[] feedback = { 0 };
                        minigameOverheadScript.instance.miniFeedback(feedback);
                        warned = true;
                    }
                }
            } else if (nextNode.GetComponent<nodeScript>().nodeType == 8) {
                cameraScript.instance.transform.localPosition = Vector3.MoveTowards(cameraScript.instance.transform.localPosition,
                    new Vector3(lane * 6, 2.5f, 0), Time.deltaTime * 20f);
            }
        } else { 
            UI.decreaseTime = false;
        }

        // Makes sure the player is facing the right way during movement
        if (currentNode != null) {
            transform.rotation = Quaternion.Slerp(
                cameraScript.instance.transform.rotation,
                currentNode.transform.rotation,
                Time.deltaTime * 10);
            // When encountering this node we want to return the camera to its original position
            if (currentNode.GetComponent<nodeScript>().nodeType == 3) {
                cameraScript.instance.transform.localPosition = Vector3.MoveTowards(cameraScript.instance.transform.localPosition,
                    new Vector3(0, 2.5f, 0), Time.deltaTime * 40f);
                cameraScript.instance.transform.rotation = Quaternion.Slerp(
                    cameraScript.instance.transform.rotation,
                    Quaternion.Euler(0, 0, 0),
                    Time.deltaTime * 10);
            }
        }
        if (stop == 9)
            cameraScript.instance.transform.rotation = Quaternion.Slerp(
                cameraScript.instance.transform.rotation,
                Quaternion.Euler(0, -90, 0),
                Time.deltaTime * 10);

        // Moves the camera parabolically when jumping
        if (jumping) {
            cameraScript.instance.transform.position += new Vector3(0, jumpheight, 0);
            jumpheight -= 0.016f;
        }

        // If an act runs out of time, they are supposed to unconditionally fail
        // Some minigames, however, require more player interaction than others and
        // therefore, the game needs to detect this
        // We have problems with it running more than once, however..
        if (UI.timeLeft == 0) {
            if (!failedAct) {
                failedAct = true;
                if (actType == 1) {
                    int[] feedback = { -200, 0 };
                    // Code 10000 : Shake current act's interactable obstacle
                    gameData[0] = 10000;
                    gameData[1] = 2;
                    minigameOverheadScript.instance.miniFeedback(feedback);
                } else if (actType == 5) {
                    stop = 0;
                    gameData[0] = 10001;
                    gameData[1] = 0;
                } else if (actType == 6) {
                    wrongInput = false;
                    doorOpening = false;
                    warned = false;
                    int numOfOpened;
                    if (stop == 10) 
                        numOfOpened = nodes - 2;
                    else 
                        numOfOpened = nodes - 1;
                    minigameOverheadScript.instance.score.GetComponent<performanceScript>().score -= numOfOpened * 100;
                    actScript normDrActScr = levelConstructionScript.instance.tiles[currentAct].GetComponent<actScript>();
                    int numOfDoors = normDrActScr.interactiveObstacles.Length;
                    for(int i = numOfOpened; i < numOfDoors; i++) {
                        obstacleScript doorObsScr = normDrActScr.interactiveObstacles[i].GetComponent<obstacleScript>();
                        doorObsScr.interact();
                    }
                    GameObject actExitNode = currentNode;
                    while(actExitNode.GetComponent<nodeScript>().nodeType != 1)
                        actExitNode = actExitNode.GetComponent<nodeScript>().nextNode;
                    nextNode = actExitNode;
                    stop = 0;
                }
            }
        }
    }

    // We need to do key input in Update() because it updates every frame
    void Update() { 

        if (Input.GetKeyDown(KeyCode.Tab) && powerCooldown[0] == 0) {
            powerActive[0] = true;
            powerCooldown[0] = 8;
            UI.requestPowerupImage(0);
            audioManagerScript.instance.playfxSound(15);
        }

        // Controls for Beat-Up
        if (actType == 1) { 
            if (currentNode.GetComponent<nodeScript>().nodeType == 4) { 
                if (Input.anyKeyDown) {
                    int[] feedback = { 10 };
                    // Code 10000, 0 : Shake current act's interactable obstacle
                    gameData[0] = 10000;
                    gameData[1] = 0;
                    minigameOverheadScript.instance.miniFeedback(feedback);
                    audioManagerScript.instance.playfxSound(Random.Range(0,4));
                }
            }
        }
        // Controls for Hurdle Jump
        else if (actType == 2) { 
            if (!jumping) {
                if (Input.GetKeyDown("space")) {
                    jumping = true;
                    StartCoroutine(jump());
                }
            }
        }
        // Controls for Silent Crossing
        else if (actType == 3) { 
            if (Input.anyKeyDown) {
                if (currentNode.GetComponent<nodeScript>().nodeType == 6) { 
                    stop = 0;
                    failedAct = true;
                    StopCoroutine("stopPlayer");
                }
            }
        }
        // Controls for Look Out
        else if (actType == 4) {
            if (Input.GetKeyDown("a")) {
                audioManagerScript.instance.playfxSound(7);
                if (nextNode.GetComponent<nodeScript>().nodeType == 8) {
                    lane -= lane > -1 ? 1 : 0;
                }
            } else if (Input.GetKeyDown("d")) {
                audioManagerScript.instance.playfxSound(7);
                if (nextNode.GetComponent<nodeScript>().nodeType == 8) {
                    lane += lane < 1 ? 1 : 0;
                }
            }
        }

        //# controls for Norman Doors
        else if (actType == 6) {
            if (!wrongInput) {
                //# if: plyr stopped in front of the current door node
                if (stop == 10) {
                    int currentDoorIth = nodes - 2;
                    if (Input.GetKeyDown("left shift")) {
                        if (gameData[4 + currentDoorIth] == 0) 
                            stop = 0;
                        else {
                            gameData[0] = 10000 + currentDoorIth;
                            gameData[1] = 0;
                            wrongInput = true;
                            audioManagerScript.instance.playfxSound(12);
                        }
                    } else if (Input.GetKeyDown("right shift")) {
                        if (gameData[4 + currentDoorIth] == 1) 
                            stop = 0;
                        else {  //# pressed wrong key
                                //# the interaction will be the shaking of the door.
                            gameData[0] = 10000 + currentDoorIth;
                            gameData[1] = 0;
                            wrongInput = true;
                            audioManagerScript.instance.playfxSound(12);
                        }
                    }
                    //// use Code {10002, 0} to interact with 2nd element of the act
                    //// <int>nodes is ++ when each act passes.
                    //// nodes count starts from 1. And the initial currentNode is the 1st node(type 0).
                    //// After conatact of the 2nd node (== first normand door node) , it will be 2.
                    //// before plr reach first norman door node(type 10), it is still 1.
                    if (stop == 0) {
                        warned = false;
                        doorOpening = false;
                        gameData[0] = 10000 + currentDoorIth;
                        gameData[1] = 0;

                        //# for scoring. feedback data is meaningless.
                        int[] feedback = { 0 };
                        minigameOverheadScript.instance.miniFeedback(feedback);
                    }
                } else {
                    if (doorOpening == false) {
                        if (warned == false) {
                            if (nextNode.GetComponent<nodeScript>().nodeType == 10)
                                if (Vector3.Distance(nextNode.transform.position, transform.position) < 7.0f)
                                    warned = true;
                        } else {
                            int currentDoorIth = nodes - 1;
                            if (Input.GetKeyDown("left shift")) {
                                if (gameData[4 + currentDoorIth] == 0) 
                                    doorOpening = true;
                                else {
                                    gameData[0] = 10000 + currentDoorIth;
                                    gameData[1] = 0;
                                    wrongInput = true;
                                    audioManagerScript.instance.playfxSound(12);
                                }
                            }
                            else if (Input.GetKeyDown("right shift")) {
                                if (gameData[4 + currentDoorIth] == 1)
                                    doorOpening = true;
                                else {
                                    gameData[0] = 10000 + currentDoorIth;
                                    gameData[1] = 0;
                                    wrongInput = true;
                                    audioManagerScript.instance.playfxSound(12);
                                }
                            }
                            if (doorOpening == true) {
                                // Code {1xxxx, 0} - The player wishes to interact with the data%10000th element of the act's interactable obstacles
                                gameData[0] = 10000 + currentDoorIth;
                                gameData[1] = 0;
                                int[] feedback = { 0 };
                                minigameOverheadScript.instance.miniFeedback(feedback);
                            }
                        }
                    }
                }
            }
            
        }
    }

}
