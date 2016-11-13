using UnityEngine;
using System.Collections;

public class overheadPuffScript : MonoBehaviour {

    float[] speed;

	// Use this for initialization
	void Start () {
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        GetComponent<RectTransform>().localScale = Vector3.one;
        GetComponent<RectTransform>().localPosition += new Vector3(Random.Range(-400, 400), Random.Range(180, 250));
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
        GetComponent<RectTransform>().localPosition += new Vector3(speed[0], speed[1]);
        transform.Rotate(new Vector3(0, 0, speed[2]));
        speed[1] -= 0.09f;
	}
}
