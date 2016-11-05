using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class mainMenuButtonScript : MonoBehaviour {

    // We will prevent the button from triggering the load twice
    // Not sure if needed anymore
    bool loading = false;

    // Progress the game to the MenuAvenue
    public void StartGame() {
        if (!loading) {
            GameObject.FindGameObjectWithTag("loader").
                GetComponent<menuTransitionScript>().loadAppear("MenuAvenue");
            loading = true;
        }
    }

}
