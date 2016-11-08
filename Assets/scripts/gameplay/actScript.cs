using UnityEngine;
using System.Collections;

public class actScript : MonoBehaviour {

    // Each act has information about its minigame type, its tile length,
    // the time limit of the minigame, its first node, and the ambiguous
    // gameData that's completely situational depending on what the 
    // minigame is
    /*******ACT TYPES*******
    0 = Alleyway Navigation
    1 = Beat-Em-Up Obstacle
    99 = Stage Complete
    ***********************/
    public int actType;
    public int actLength;
    public int timeLimit;
    public GameObject firstNode;
    public int[] gameData;
    public GameObject[] interactiveObstacles;

    // Saves some rendering time by destroying acts that we've completed
    public void passAway() {
        StartCoroutine(die());
    }
    IEnumerator die() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void interactWithObstacle(int obstacle) {
        if (interactiveObstacles[obstacle] != null)
            interactiveObstacles[obstacle].GetComponent<obstacleScript>().interact();
    }
    public void removeObstacle(int obstacle) {
        if (interactiveObstacles[obstacle] != null)
            interactiveObstacles[obstacle].GetComponent<obstacleScript>().remove();
    }

}
