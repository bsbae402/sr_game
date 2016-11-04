using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class levelInitScript : MonoBehaviour {

    public string levelName;
    public string sceneName;
    public string description;
    public int[] obstacles;
    public float stageSpeed = 1;

	// Use this for initialization
	void Start () {
        //Time.timeScale = stageSpeed;
	}

    public void EndGame(int status) {
        switch (status) {
            case 0:
                GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                    loadAppear("MenuAvenue");
                break;
            default: // Will process score later on
                //SceneManager.LoadScene("MainMenu");
                break;
        }
    }

}
