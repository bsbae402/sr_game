using UnityEngine;
using System.Collections;

public class levelConstructionScript : MonoBehaviour {

    public static levelConstructionScript instance = null;
    
    // Numberings for generating the stage; this allows us to use just one scene
    public Transform[] tileBase;
    // Possible values passed on by the level init obstacles, as well as the corresponding act tiles
    // Add more as we make more minigames
    int[] validTiles =  {       -1,  0, 1, 2, 3, 4, 5, 6, 7, 8, 9,  10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
    int[] CorrespondingTile = { -1, -1, 0, 1, 2, 3, 0, 1, 2, 3, 0,  -1,  4,  4,  4,  4,  4,  4,  4,  4,  4 };

    // Where the tiles themselves are held
    [HideInInspector]
    public Transform[] tiles;
    // Replicated from the level init obstacles
    int[] tileData;

    // The level init object preserved from the MenuAvenue scene; the keystone of each level
    GameObject levelData;
    // Keeps track of where the next tile is placed; not every tile is the same size
    float lastTilePosition = 0f;

    // Determines whether the stage should start moving
    // We want to make sure everything is set up first
    bool started = false;

    // The speed in which the stage moves
    // It should also be used to change how fast arrows move
    [HideInInspector]
    public float speed = 0.4f;

    void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

	// Use for initialization
	void Start () {
        audioManagerScript.instance.stopMusic();
        Time.timeScale = 1;
        // Grabs level data to generate level
        levelData = GameObject.FindGameObjectWithTag("levelInit");
        if (levelData == null) {
            GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
                loadAppear("MenuAvenue");
            Debug.Log("Error loading level data.");
            return;
        }
        tileData = levelData.GetComponent<levelInitScript>().obstacles;
        tiles = new Transform[tileData.Length + 1];
        constructLevel();
	}
	
    // Places tiles onto the scene
    void constructLevel() { 
        for(int i = 0; i < tileData.Length; i++) {
            tiles[i] = newTile(tileData[i]);
            tiles[i].parent = this.transform;
            tiles[i].transform.position += new Vector3(0, 0, lastTilePosition);
            lastTilePosition += tiles[i].GetComponent<actScript>().actLength;
        }
        var finishTile = Instantiate(tileBase[tileBase.Length - 1]) as Transform;
        tiles[tiles.Length - 1] = finishTile;
        tiles[tiles.Length - 1].parent = this.transform;
        tiles[tiles.Length - 1].transform.position += new Vector3(0, 0, lastTilePosition); 
        StartCoroutine(levelStart());
    }

    // Finally starts the level with the specified game speed
    IEnumerator levelStart() {
        yield return new WaitForSeconds(1f);
        Time.timeScale = levelData.GetComponent<levelInitScript>().stageSpeed;
        started = true;
        yield return new WaitForSeconds(1f);
        // Don't forget to destroy all preserved but used game objects to prevent overuse of memory
        Destroy(levelData);
    }

    // Used during constructLevel to properly choose a tile to place
    Transform newTile(int t) {
        // Checks which version of the act we're using
        // i.e. When doing alley navigation, alleys have different paths so we have multiple instances
        int variant = t % 10;

        // Checks for complete randomization or partial randomization
        // t == -1 : Choose any tile at all from any act variant
        // t % 10 == 0 : Choose any variant from the act
        // We can't use ArrayUtility for searching because UnityEditor is a dumb library
        bool found = false;
        for (int i = 0; i < validTiles.Length; i++)
            if (validTiles[i] == t)
                found = true;
        if (!found)
            t = -1;
        if (t == -1) {
            t = validTiles[Random.Range(1, validTiles.Length)];
            variant = t % 10;
        }
        if (variant == 0)
            t += (int)(Random.Range(1, 9));

        // Creates and sends the new tile
        // We can't use ArrayUtility for searching because UnityEditor is a dumb library
        int index = 0;
        for (int i = 0; i < validTiles.Length; i++)
            if (validTiles[i] == t)
                index = i;
        var levelTile = Instantiate(tileBase[CorrespondingTile[index]]) as Transform;
        return levelTile;
    }

    IEnumerator finishStage() {
        Time.timeScale = 1;
        yield return new WaitForSeconds(3f);
        if(playerStats.instance != null)
            playerStats.instance.updateNeeded = true;
        GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
            loadAppear("MenuAvenue");
    }
    IEnumerator failStage() { 
        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.FindGameObjectWithTag("perf"));
        GameObject.FindGameObjectWithTag("loader").GetComponent<menuTransitionScript>().
            loadAppear("MenuAvenue");
    }

	// Again, using FixedUpdate to update as often as the game speed
	void FixedUpdate () {
        // Don't update anything until we've started
        if (!started)
            return;
        // Don't update anything if the stage is over
        if (speed < -100f)
            return;

        transform.position -= Vector3.Normalize(playerScript.instance.angle) * speed;
        if (playerScript.instance.stop == 0) {
            if (speed < 0.4f)
                speed += 0.005f;
            if (speed > 0.4f)
                speed = 0.4f;
        } else if (playerScript.instance.stop == 99) {
            speed = -1000f;
            StartCoroutine(finishStage());
            return;
        } else if (playerScript.instance.stop == -99) {
            speed = -1000f;
            StartCoroutine(failStage());
            return;
        } else {
            speed = 0f;
        }
        
        // GameData is completely arbitrary in the way that it'll pass data to levelConstruction
        // I doubt I'll even remember what it does.
        // Code {1xxxx} - The player wishes to interact with the data%10000th element of the act's interactable obstacles
        // Code {1xxxx, 1} - The player wishes to remove the data%10000th element of the act's interactable obstacles
        // Code {1xxxx, 2} - Same as previous, but the player failed the stage
        if (playerScript.instance.gameData[0] / 10000 == 1) {
            tiles[playerScript.instance.gameData[0] % 10000].GetComponent<actScript>().
                interactWithObstacle(0);
            if (playerScript.instance.gameData[1] >= 1) {
                tiles[playerScript.instance.gameData[0] % 10000].GetComponent<actScript>().
                    removeObstacle(0);
                System.Array.Clear(playerScript.instance.gameData, 0, 10);
            }
        }

        // The player has been hit, so they will slow down
        if(playerScript.instance.hit) {
            speed = 0f;
            playerScript.instance.hit = false;
        }
        playerScript.instance.movementSpeed = speed;

        // We don't want to check every single act to see if the player has reached the first node
        // So we only check the proceeding one
        // This code bit is a mess
        int act = playerScript.instance.currentAct;
        if (act + 1 < tiles.Length) {
            if (Vector3.Distance(tiles[act + 1].GetComponent<actScript>().firstNode.transform.position,
                playerScript.instance.getPosition()) < speed ) {
                playerScript.instance.finishAct();
                playerScript.instance.setPosition(tiles[act + 1].GetComponent<actScript>().firstNode.transform.position);
                if (act >= 0)
                    tiles[act].GetComponent<actScript>().passAway();
                playerScript.instance.failedAct = false;
                playerScript.instance.UI.decreaseTime = true;
                playerScript.instance.currentAct = act + 1;
                playerScript.instance.actType = tiles[act + 1].GetComponent<actScript>().actType;
                playerScript.instance.currentNode = tiles[act + 1].GetComponent<actScript>().firstNode;
                playerScript.instance.nextNode = tiles[act + 1].GetComponent<actScript>().
                    firstNode.GetComponent<nodeScript>().nextNode;
                System.Array.Clear(playerScript.instance.gameData, 0, 10);
                minigameOverheadScript.instance.
                    newAct(tiles[act + 1].GetComponent<actScript>().actType, 
                    tiles[act + 1].GetComponent<actScript>().gameData);
                playerScript.instance.UI.timeLeft = tiles[act + 1].GetComponent<actScript>().timeLimit;
            }
        }
	}
}
