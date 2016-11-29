using UnityEngine;
using System.Collections;

public class actScript : MonoBehaviour {

    // Each act has information about its minigame type, its tile length,
    // the time limit of the minigame, its first node, and the ambiguous
    // gameData that's completely situational depending on what the 
    // minigame is
    /*******ACT TYPES*******
    0 = Alley-Navigation
    1 = Obstacle Beat-Up
    2 = Hurdle Jump
    3 = Silent Crossing
    4 = Look Out
    5 = Drawbridge
    6 = Norman Doors
    1000 = Stage Complete
    ***********************/
    public int actType;
    public int actLength;
    public int timeLimit;
    public bool tutorial;
    public GameObject firstNode;
    public int[] gameData;
    public GameObject[] interactiveObstacles;

    // Saves some computation time by destroying acts that we've completed
    public void passAway() {
        StartCoroutine(die());
    }
    IEnumerator die() {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }

    public void interactWithObstacle(int obstacle) {
        if (obstacle >= interactiveObstacles.Length)
            return;
        if (interactiveObstacles[obstacle] != null)
            interactiveObstacles[obstacle].GetComponent<obstacleScript>().interact();
    }
    public void removeObstacle(int obstacle) {
        if (obstacle >= interactiveObstacles.Length)
            return;
        if (interactiveObstacles[obstacle] != null)
            interactiveObstacles[obstacle].GetComponent<obstacleScript>().remove();
    }

}
