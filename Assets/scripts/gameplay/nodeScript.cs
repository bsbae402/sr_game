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
    6 = Act-Type-3 Stop
    7 = Act-Type-3 Get-Hit
    8 = Act-Type-4 People Group
    9 = Act-Type-5 Lever Turn
    10 = Act-Type-6 Norman Door Push or Pull
    99 = Stage Complete
    ***********************/
    public int nodeType;
    public GameObject nextNode;
	
	void FixedUpdate () {
	
	}
}
