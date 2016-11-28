using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class levelListScript : MonoBehaviour {
    // The big script for handling the entire level list
    public static levelListScript instance = null;
    
    // Records of almost all of its children for calling ease
    // Scroll keeps track of where the scrolling currently is in our level list
    // We keep selected in conjunction with scroll to update the description box
    public GameObject[] levels;
    public GameObject[] levelListings;
    public GameObject description;
    int scroll, selected;

    // We don't want to update every frame because that's costly
    // Instead, when a button is pressed we'll update once
    bool needUpdate;

    void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
	// Use this for initialization
	void Start () {
        scroll = 0;
        selected = -1;
        needUpdate = true;
	}

    // Functions that can be called by buttons to ask for an update
    public void requestUpdate() {
        needUpdate = true;
    }
    public void requestUpdate(int selected) {
        this.selected = selected + scroll;
        needUpdate = true;
    }

    // Handling of scrolling; makes sure it never goes out of range
    public void scrollnum(int n) {
        scroll += n;
        if (scroll > levels.Length - 5)
            scroll = levels.Length - 5;
        if (scroll < 0)
            scroll = 0;
    }

    // The level list starts off with nothing selected
    // For more ease we keep a retriever function
    public bool levelSelected() {
        return selected >= 0;
    }
    public GameObject getLevel() {
        if (selected == -1)
            return null;
        return levels[selected];
    }
    
	void Update () {
        // Only update if it was requested
        if (needUpdate) {
            // If there is a stage selected, make the start button appear
            if (selected != -1 && selected < levels.Length) { 
                description.GetComponentInChildren<Text>().text = 
                    levels[selected].GetComponent<levelInitScript>().description;
                GetComponentInChildren<CanvasGroup>().alpha = 1;
            }
            // When we press a scroll button, we have to update as well
	        for(int i = 0; i < levelListings.Length; i++) {
                if (i + scroll < levels.Length) {
                    levelListings[i].GetComponentInChildren<Text>().text = 
                        levels[i + scroll].GetComponent<levelInitScript>().levelName;
                } else {
                    break;
                }
            }
            // Only update once
            needUpdate = false;
        }
	}
}
