using UnityEngine;
using System.Collections;

public class actScript : MonoBehaviour {

    // Each act has information about its minigame type, its tile length,
    // the time limit of the minigame, its first node, and the ambiguous
    // gameData that's completely situational depending on what the 
    // minigame is
    public int actType;
    public int actLength;
    public int timeLimit;
    public GameObject firstNode;
    public int[] gameData;

    // Saves some rendering time by destroying acts that we've completed
    public void passAway() {
        StartCoroutine(die());
    }
    IEnumerator die() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
