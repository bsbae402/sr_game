using UnityEngine;
using System.Collections;

public class UIButtonScript : MonoBehaviour {

	public void EndGame(int status) {
        switch (status) {
            case 0:
                playerScript.instance.getHit(1000);
                break;
            default: // Will process score later on
                //SceneManager.LoadScene("MainMenu");
                break;
        }
    }

}
