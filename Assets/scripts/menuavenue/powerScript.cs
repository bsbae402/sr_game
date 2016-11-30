using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class powerScript : MonoBehaviour {

    public string powerName;
    public string description;
    public int cost;
    
    void Start() {
        GetComponentInChildren<Text>().text = powerName;
    }
}
