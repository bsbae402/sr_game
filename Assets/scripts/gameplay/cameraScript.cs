using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {

    public static cameraScript instance = null;

    void Awake() {
        instance = this;
    }

}
