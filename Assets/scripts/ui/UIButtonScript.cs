using UnityEngine;
using System.Collections;

public class UIButtonScript : MonoBehaviour {

	public void EndGame(int status) {
        switch (status) {
            case 0:
                Time.timeScale = 1;
                Destroy(GameObject.FindGameObjectWithTag("perf"));
                GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                    loadAppear("MenuAvenue");
                break;
            default: // Will process score later on
                //SceneManager.LoadScene("MainMenu");
                break;
        }
    }

}
