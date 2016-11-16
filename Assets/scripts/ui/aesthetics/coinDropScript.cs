using UnityEngine;
using System.Collections;

public class coinDropScript : MonoBehaviour {

    float[] speed;

	// Use this for initialization
	void Start () {
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        GetComponent<RectTransform>().localScale = Vector3.one;
        speed = new float[3];
        speed[0] = Random.Range(-2, 2);
        speed[1] = Random.Range(-4, 1);
        speed[2] = Random.Range(-24, 24);
        StartCoroutine(expire());
	}

    IEnumerator expire() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
	
	void FixedUpdate () {
        GetComponent<RectTransform>().Rotate(0, 0, speed[2]);
        GetComponent<RectTransform>().localPosition += new Vector3(speed[0], speed[1]);
        speed[1] -= 0.09f;
	}
}
