using UnityEngine;
using System.Collections;

public class menuPersistentButtonHandler : MonoBehaviour {

    public GameObject lists;

    Vector3 listPosition;
    bool update = false;

    // When the Levels button is pressed
    public void levelList() {
        audioManagerScript.instance.playfxSound(8);
        listPosition = new Vector3(0, 0, 0);
        update = true;
    }

    // When the Items button is pressed
    public void itemList() {
        audioManagerScript.instance.playfxSound(8);
        listPosition = new Vector3(-800, 0, 0);
        update = true;
    }

    // When the Power ups button is pressed
    public void powerList() {
        audioManagerScript.instance.playfxSound(8);
        listPosition = new Vector3(-1600, 0, 0);
        update = true;
    }

    void Update() {
        if (!update)
            return;
        lists.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(
            lists.GetComponent<RectTransform>().localPosition, listPosition, Time.deltaTime * 1500);
        if (lists.GetComponent<RectTransform>().localPosition == listPosition)
            update = false;
    }

}
