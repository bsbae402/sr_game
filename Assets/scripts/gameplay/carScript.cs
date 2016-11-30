using UnityEngine;
using System.Collections;

public class carScript : MonoBehaviour {

	IEnumerator die() {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    void Start() {
        StartCoroutine(die());
    }

	// Update is called once per frame
	void FixedUpdate () {
        transform.position += transform.right * -0.6f;
        if (Vector3.Distance(transform.position, playerScript.instance.transform.position) < 1f)
            GetComponent<Animator>().SetBool("Wrecked", true);
	}
}
