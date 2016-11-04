using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class levelListScript : MonoBehaviour {

    public GameObject[] levels;
    public GameObject[] levelListings;
    public GameObject description;
    int scroll, selected;

    bool needUpdate;
	// Use this for initialization
	void Start () {
        scroll = 0;
        selected = -1;
        needUpdate = true;
	}

    public void requestUpdate() {
        needUpdate = true;
    }
    public void requestUpdate(int selected) {
        this.selected = selected + scroll;
        needUpdate = true;
    }

    public void scrollnum(int n) {
        scroll += n;
        if (scroll > levels.Length - 5)
            scroll = levels.Length - 5;
        if (scroll < 0)
            scroll = 0;
    }

    public bool levelSelected() {
        return selected >= 0;
    }
    public GameObject getLevel() {
        if (selected == -1)
            return null;
        return levels[selected];
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (needUpdate) {
            if (selected != -1 && selected < levels.Length) { 
                description.GetComponentInChildren<Text>().text = 
                    levels[selected].GetComponent<levelInitScript>().description;
                GetComponentInChildren<CanvasGroup>().alpha = 1;
            }
	        for(int i = 0; i < levelListings.Length; i++) {
                if (i + scroll < levels.Length) {
                    levelListings[i].GetComponentInChildren<Text>().text = 
                        levels[i + scroll].GetComponent<levelInitScript>().levelName;
                } else {
                    break;
                }
            }
            needUpdate = false;
        }
	}
}
