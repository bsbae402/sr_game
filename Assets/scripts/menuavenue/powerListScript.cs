using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class powerListScript : MonoBehaviour {

    public static powerListScript instance = null;

    public GameObject[] powerListings;
    public GameObject description;
    public GameObject purchase;
    public GameObject cost;

    public int selected;

    bool needUpdate;

    void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start () {
        selected = -1;
        needUpdate = true;
	}

    public void requestUpdate() {
        needUpdate = true;
    }
    public void requestUpdate(int selected) {
        this.selected = selected;
        needUpdate = true;
    }

    public bool powerSelected() {
        return selected >= 0;
    }
    public GameObject getPower() {
        if (selected == -1)
            return null;
        return powerListings[selected];
    }
	
	void Update () {
        if (!needUpdate)
            return;
        if (selected != -1 && selected < powerListings.Length) {
            description.GetComponentInChildren<Text>().text =
                powerListings[selected].GetComponent<powerScript>().description;
            purchase.GetComponent<CanvasGroup>().alpha = 1;
            purchase.GetComponent<CanvasGroup>().interactable = true;
            purchase.GetComponent<CanvasGroup>().blocksRaycasts = true;
            cost.GetComponent<CanvasGroup>().alpha = 1;
            cost.GetComponentInChildren<Text>().text = powerListings[selected].GetComponent<powerScript>().cost + " COST";
        }
        needUpdate = false;
    }
}
