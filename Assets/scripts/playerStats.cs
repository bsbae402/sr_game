using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerStats : MonoBehaviour {

    public static playerStats instance = null;

    public int money;

    [HideInInspector]
    public bool updateNeeded = false;

    void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
        money = 0;
        DontDestroyOnLoad(gameObject);
	}
	
    IEnumerator updateStats() {
        yield return new WaitForSeconds(6f);
        if (GameObject.FindGameObjectWithTag("perf") == null)
            yield break;
        money += GameObject.FindGameObjectWithTag("perf").GetComponent<performanceScript>().score / 1000;
        GameObject.FindGameObjectWithTag("money").GetComponent<Text>().text = "" + money;
        audioManagerScript.instance.playfxSound(10);
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.FindGameObjectWithTag("perf"));
    }

	// Update is called once per frame
	void Update () {
	    if (updateNeeded) { 
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MenuAvenue") {
                audioManagerScript.instance.playMusic(0);
                GameObject.FindGameObjectWithTag("money").GetComponent<Text>().text = "" + money;
                GameObject.FindGameObjectWithTag("moneyEffect").GetComponent<moneyDisplayScript>().start();
                StartCoroutine(updateStats());
                updateNeeded = false;
            }
        }
	}
}
