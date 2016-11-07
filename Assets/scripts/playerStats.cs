using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerStats : MonoBehaviour {

    public int money;

    [HideInInspector]
    public bool updateNeeded = false;

	// Use this for initialization
	void Start () {
        money = 0;
        DontDestroyOnLoad(gameObject);
	}
	
    IEnumerator updateStats() {
        yield return new WaitForSeconds(6f);
        money += GameObject.FindGameObjectWithTag("perf").GetComponent<performanceScript>().score / 1000;
        GameObject.FindGameObjectWithTag("money").GetComponent<Text>().text = "" + money;
    }

	// Update is called once per frame
	void Update () {
	    if (updateNeeded) { 
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MenuAvenue") {
                StartCoroutine(updateStats());
                updateNeeded = false;
            }
        }
	}
}
