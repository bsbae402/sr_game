using UnityEngine;
using System.Collections;

public class nodeScript : MonoBehaviour {
    /******NODE TYPES*******
    0 = Act Entrance
    1 = Act Exit
    2 = Act-Type-0 Turn Right
    3 = Act-Type-0 Turn Left
    99 = Stage Complete
    ***********************/
    public int nodeType;
    public GameObject nextNode;
	
	void FixedUpdate () {
	
	}
}
