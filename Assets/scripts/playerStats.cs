﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerStats : MonoBehaviour {

    public static playerStats instance = null;

    public int money;

    /*****POWER TYPES*******
    0 = Four of a Kind
    1 = Missing Power
    2 = Determination
    3 = Easy Mode
    ***********************/
    [HideInInspector]
    public bool[] powers;

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
        powers = new bool[4];
        DontDestroyOnLoad(gameObject);
	}
	
    IEnumerator updateStats() {
        if (GameObject.FindGameObjectWithTag("perf") == null)
            yield break;
        yield return new WaitForSeconds(GameObject.FindGameObjectWithTag("perf").GetComponent<performanceScript>().score / 10000f + 3.8f);
        money += GameObject.FindGameObjectWithTag("perf").GetComponent<performanceScript>().score / 1000;
        GameObject.FindGameObjectWithTag("money").GetComponent<Text>().text = "" + money;
        audioManagerScript.instance.playfxSound(10);
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.FindGameObjectWithTag("perf"));
    }

    public void forceMoney(int amount) {
        money += amount;
        GameObject.FindGameObjectWithTag("money").GetComponent<Text>().text = "" + money;
        audioManagerScript.instance.playfxSound(10);
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
