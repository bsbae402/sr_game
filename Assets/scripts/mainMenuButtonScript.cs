using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class mainMenuButtonScript : MonoBehaviour {

    bool loading = false;

    public void StartGame() {
        if (!loading) {
            GameObject.FindGameObjectWithTag("loader").
                GetComponent<menuTransitionScript>().loadAppear("MenuAvenue");
            loading = true;
        }
    }

}
