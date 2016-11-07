using UnityEngine;
using System.Collections;

public class performanceScript : MonoBehaviour {

    [HideInInspector]
    public int score = 0;

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

}
