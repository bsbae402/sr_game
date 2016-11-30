using UnityEngine;
using System.Collections;

public class powerListButtonScript : MonoBehaviour {

	public void powerView(int selected) { 
        powerListScript.instance.requestUpdate(selected);
        audioManagerScript.instance.playfxSound(8);
    }

    public void purchase() {
        if (playerStats.instance == null)
            return;
        if (powerListScript.instance.powerSelected()) {
            if (playerStats.instance.money < powerListScript.instance.getPower().GetComponent<powerScript>().cost)
                return;
            playerStats.instance.forceMoney(-powerListScript.instance.getPower().GetComponent<powerScript>().cost);
            for (int i = 0; i < playerStats.instance.powers.Length; i++)
                playerStats.instance.powers[i] = false;
            playerStats.instance.powers[powerListScript.instance.selected] = true;
            playerStats.instance.firstTimePower = true;
        }
    }
}
