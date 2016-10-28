using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageInitScript : MonoBehaviour {
    
    public int[] obstacles;
    public float stageSpeed = 1;

	// Use this for initialization
	void Start () {
        Time.timeScale = stageSpeed;
	}

    public void EndGame(int status) {
        switch (status) {
            case 0:
                SceneManager.LoadScene("MainMenu");
                break;
            default: // Will process score later on
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

}
