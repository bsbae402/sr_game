using UnityEngine;
using System.Collections;
using UnityEditor;

public class levelConstructionScript : MonoBehaviour {

    // A lot of variables are recorded as fields because the Level scene is used as every stage

    public GameObject player;
    
    // Numberings for generating the stage; this allows us to use just one scene
    public Transform[] tileBase;
    // Possible values passed on by the level init obstacles, as well as the corresponding act tiles
    // Add more as we make more minigames
    int[] validTiles =  { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    int[] CorrespondingTile = { -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    [HideInInspector]
    // Where the tiles themselves are held
    public Transform[] tiles;
    // Replicated from the level init obstacles
    int[] tileData;

    // The level init object preserved from the MenuAvenue scene; the keystone of each level
    GameObject levelData;
    // Keeps track of where the next tile is placed; not every tile is the same size
    float lastTilePosition = 0f;

    // Controls movement speed through the stage
    // I'm not sure why this is placed here
    // It should probably be static
    float speed = 0.4f;

    // Determines whether the stage should start moving
    // We want to make sure everything is set up first
    bool started = false;

	// Use for initialization
	void Start () {
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
        tiles = new Transform[tileData.Length];
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
        if (!ArrayUtility.Contains(validTiles, t))
            t = -1;
        if (t == -1)
            t = Random.Range(0, 9);
        if (variant == 0)
            t += (int)(Random.Range(1, 9));

        // Creates and sends the new tile
        var levelTile = Instantiate(
            tileBase[CorrespondingTile[ArrayUtility.IndexOf<int>(validTiles, t)]]) as Transform;
        return levelTile;
    }

	// Again, using FixedUpdate to update as often as the game speed
	void FixedUpdate () {
        // Don't update anything until we've started
        if (!started)
            return;

        transform.position -= Vector3.Normalize(player.GetComponent<playerScript>().angle) * speed;
        if (speed < 0.4f)
            speed += 0.01f;
        if (speed > 0.4f)
            speed = 0.4f;

        // We don't want to check every single act to see if the player has reached the first node
        // So we only check the proceeding one
        // This code bit is a mess
        int act = player.GetComponent<playerScript>().currentAct;
        if (act + 1 < tiles.Length) {
            if (Vector3.Distance(tiles[act + 1].GetComponent<actScript>().firstNode.transform.position, 
                player.transform.position) < speed ) {
                if (act >= 0)
                    tiles[act].GetComponent<actScript>().passAway();
                player.GetComponent<playerScript>().currentAct = act + 1;
                player.GetComponent<playerScript>().actType = tiles[act + 1].GetComponent<actScript>().actType;
                player.GetComponent<playerScript>().nextNode =
                    tiles[act + 1].GetComponent<actScript>().firstNode.GetComponent<nodeScript>().nextNode;
                player.GetComponent<playerScript>().minigameOverhead.GetComponent<minigameOverheadScript>().
                    newAct(tiles[act + 1].GetComponent<actScript>().actType, 
                    tiles[act + 1].GetComponent<actScript>().gameData);
            }
        }
	}
}
