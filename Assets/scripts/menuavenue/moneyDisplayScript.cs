using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class moneyDisplayScript : MonoBehaviour {

    public GameObject score;
    public GameObject sign;
    public GameObject coins;

    public RectTransform coin;

    public void start() {
        StartCoroutine(display());
    }

    IEnumerator display() {
        if (GameObject.FindGameObjectWithTag("perf") == null)
            yield break;
        int s = GameObject.FindGameObjectWithTag("perf").GetComponent<performanceScript>().score;
        yield return new WaitForSeconds(2f);
        score.GetComponent<Text>().text = "" + s;
        score.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(0.5f);
        sign.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(0.5f);
        coins.GetComponent<Text>().text = "" + s / 1000;
        coins.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < s / 1000; i++) {
            var obj = Instantiate(coin) as RectTransform;
            obj.SetParent(GetComponent<RectTransform>());
            audioManagerScript.instance.playfxSound(10);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        score.GetComponent<CanvasGroup>().alpha = 0;
        sign.GetComponent<CanvasGroup>().alpha = 0;
        coins.GetComponent<CanvasGroup>().alpha = 0;
    }



}
