using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour {

    // Current nodes and next nodes of movement
    // Used for camera rotation
    public GameObject nextNode;
    public GameObject currentNode;

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

    // Use this for initialization
    void Start () {
        eyes = GetComponentInChildren<Camera>();
        currentAct = -1;
        actType = -1;
        angle = GetComponentInChildren<Camera>().transform.forward;
        gameData = new int[10];
	}
	
    // FixedUpdate moves with the game speed
	void FixedUpdate () {
        // We'll detect collisions with nodes within the same act in player
        // Collisions with nodes in different acts are handled in levelConstruction
        // Updates desired angle, and the current and next nodes for the player
        if (nextNode.GetComponent<nodeScript>().nodeType != 1) {
	        if (Vector3.Distance(nextNode.transform.position, transform.position) < 0.1) {
                //Debug.Log("From player: " + nextNode.name);
                currentNode = nextNode;
                nextNode = nextNode.GetComponent<nodeScript>().nextNode;
                angle = currentNode.transform.forward;
            }
        }
        // Makes sure the player is facing the right way during movement
        if(currentNode != null)
            transform.rotation = Quaternion.Slerp(
                eyes.transform.rotation,
                currentNode.transform.rotation,
                Time.deltaTime * 10);
    }
}
