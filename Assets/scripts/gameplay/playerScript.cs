using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour {

    // Current nodes and next nodes of movement
    // Used for camera rotation
    public GameObject nextNode;
    public GameObject currentNode;

    UIScript UI;
    // The super-messy minigameOverhead is a child to the player
    // Again, a lot of consistent objects are used as public fields because it won't matter anyway
    public GameObject minigameOverhead;

    // I can't remember why I have this
    // The current act may have to pass information to the player?
    [HideInInspector]
    public int[] gameData;
    // The current act (as in the tiles[] index) the player is in
    [HideInInspector]
    public int currentAct;
    // The current act (as in the type of minigame) the playe ris in
    [HideInInspector]
    public int actType;
    // The desired angle the player wants to end up facing depending on the current node
    [HideInInspector]
    public Vector3 angle;
    
    // Our camera kept record of for calling ease
    Camera eyes;

    [HideInInspector]
    // levelConstruction's record of the movement speed will slow down
    // when it detects that the player has been hit
    public bool hit = false;
    [HideInInspector]
    // Unfortunately we need to keep this in order for node detection to work
    // levelConstruction will have to continously update this value
    public float movementSpeed;
    [HideInInspector]
    // Detects whether the movement should stop or not
    // It's used in minigames such as Beat-em-up
    public int stop = 1;

    // Use this for initialization
    void Start () {
        UI = GetComponentInChildren<UIScript>();
        eyes = GetComponentInChildren<Camera>();
        currentAct = -1;
        actType = -1;
        angle = GetComponentInChildren<Camera>().transform.forward;
        gameData = new int[10];
	}

    public void getHit(int damage) {
        hit = true;
        UI.hit(damage);
    }
	
    // FixedUpdate moves with the game speed
	void FixedUpdate () {
        if (stop == 99 || stop == -99)
            return;
        // The stage has ended at this point
        if (UI.getHealth() <= 0)
            stop = -99;
        if (nextNode.GetComponent<nodeScript>().nodeType == 99) { 
            if (Vector3.Distance(nextNode.transform.position, transform.position) < movementSpeed) {
                transform.position = nextNode.transform.position;
                stop = 99;
            }
        }
        // We'll detect collisions with nodes within the same act in player
        // Collisions with nodes in different acts are handled in levelConstruction
        // Updates desired angle, and the current and next nodes for the player
        else if (nextNode.GetComponent<nodeScript>().nodeType != 1) {
	        if (Vector3.Distance(nextNode.transform.position, transform.position) < movementSpeed) {
                //Debug.Log("From player: " + nextNode.name);
                transform.position = nextNode.transform.position;
                currentNode = nextNode;
                nextNode = nextNode.GetComponent<nodeScript>().nextNode;
                angle = currentNode.transform.forward;
                // If we encounter the beat-em-up node identifier
                if (currentNode.GetComponent<nodeScript>().nodeType == 4)
                    stop = 1;
            }
        } else {
            UI.updateScore(minigameOverhead.GetComponent<minigameOverheadScript>().score.GetComponent<performanceScript>().score);
        }
        // Makes sure the player is facing the right way during movement
        if(currentNode != null)
            transform.rotation = Quaternion.Slerp(
                eyes.transform.rotation,
                currentNode.transform.rotation,
                Time.deltaTime * 10);
    }

    // We need to do key input in Update() because it updates every frame
    void Update() { 
        if (actType == 1) { 
            if (currentNode.GetComponent<nodeScript>().nodeType == 4) { 
                if (Input.anyKeyDown) {
                    int[] feedback = { 10 };
                    minigameOverhead.GetComponent<minigameOverheadScript>().miniFeedback(feedback);
                }
            }
        }
    }
}
