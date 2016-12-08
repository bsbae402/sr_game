using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class levelInitScript : MonoBehaviour {
    // The levelInit controls how an initialized Level scene will be like
    // It is what makes the scene seem like a completely different level
    
    // The name of the level and the description are used during the MenuAvenue display
    // We'll have images too later on
    // Obstacles contains integers that tell what minigame variation we'll have
    // Stage Speed can make the level slower or faster; 1 is already really slow
    public string levelName;
    public string sceneName; // Removing later; kept for testing
    public string description;
    public int musicNo;
    public int[] obstacles;
    public float stageSpeed = 1;

	// Use this for initialization
	void Start () {

	}

}
