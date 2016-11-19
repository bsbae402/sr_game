using UnityEngine;
using System.Collections;

public class nodeScript : MonoBehaviour {
    /******NODE TYPES*******
    0 = Act Entrance
    1 = Act Exit
    2 = Act-Type-0 Turn
    3 = Empty Movement
    4 = Act-Type-1 Stop-Until-Obstacle-Destroyed
    5 = Act-Type-2 Jump
    99 = Stage Complete
    ***********************/
    public int nodeType;
    public GameObject nextNode;
	
	void FixedUpdate () {
	
	}
}
