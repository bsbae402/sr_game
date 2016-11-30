using UnityEngine;
using System.Collections;

public class carSpawnerScript : MonoBehaviour {

    [HideInInspector]
    public bool active;
    [HideInInspector]
    public bool spawning;

    public bool flip;

    public Transform[] cars;

    public void spawn() {
        spawning = true;
        var car = Instantiate(cars[Random.Range(0, cars.Length)]) as Transform;
        car.position = transform.position;
        if (flip)
            car.RotateAround(car.position, Vector3.up, 180);
        car.SetParent(transform);
        StartCoroutine(finishSpawning());
    }
    IEnumerator finishSpawning() {
        yield return new WaitForSeconds(1f + Random.Range(0, 3));
        spawning = false;
    }

	// Use this for initialization
	void Start () {
        spawning = false;
        active = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!active)
            return;
        if (spawning)
            return;
        spawn();
	}
}
