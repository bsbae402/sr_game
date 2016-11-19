using UnityEngine;
using System.Collections;

public class obstacleScript : MonoBehaviour {

    public int actType;
    public int obstacleType;

    float shake;

    bool removing;

    Vector3 originalPosition;

	void Start () {
        shake = 0;
        removing = false;
        originalPosition = transform.localPosition;
	}

    public void interact() {
        if (actType == 1)
            shake += 0.4f;
        else if (actType == 2) {
            GetComponent<Animator>().SetBool("Destroyed", true);
            removing = true;
            StartCoroutine(die());
        }
    }
    public void remove() {
        removing = true;
        StartCoroutine(die());
    }
    IEnumerator die() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        if (actType == 1) {
            if (removing) {
                transform.localPosition += new Vector3(0, 0.02f, 0.5f);
                transform.Rotate(new Vector3(0, 0, 20f));
                return;
            }
            if (shake > 10)
                shake = 10;
            if (shake > 0) {
                transform.localPosition = originalPosition + Random.insideUnitSphere * shake * 0.1f;
                shake -= 0.02f;
            } else {
                shake = 0f;
            }
        }
	}
}
