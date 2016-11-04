using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class menuTransitionScript : MonoBehaviour {

    public GameObject[] slides;
    string currentScene;
    bool appeared;

    // Use this for initialization
    void Start () {
        appeared = false;
        DontDestroyOnLoad(transform.parent.gameObject);
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    void FixedUpdate() {
        if (appeared)
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != currentScene)
                StartCoroutine(disappear());
    }

    public void loadAppear(string scene) {
        if (!appeared) {
            StartCoroutine(appear(scene));
            appeared = true;
        }
    }

    IEnumerator appear(string scene) {
        for (int i = 0; i < slides.Length; i++) {
            if (i < 7)
                yield return new WaitForSeconds(0.2f);
            slides[i].GetComponent<CanvasGroup>().alpha = 1;
            slides[i].transform.SetAsLastSibling();
            //GetComponent<AudioSource>().Play();
        }
        yield return new WaitForSeconds(3f);
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone) {
            yield return null;
        }
    }

    IEnumerator disappear() {
        yield return new WaitForSeconds(1.0f);
        for (int i = slides.Length - 1; i >= 0; i--) {
            if (i < 7)
                yield return new WaitForSeconds(0.2f);
            if( slides[i] != null )
                slides[i].GetComponent<CanvasGroup>().alpha = 0;
            //GetComponent<AudioSource>().Play();
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < slides.Length; i++) {
            Destroy(slides[i]);
        }
        yield return new WaitForSeconds(1f);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }
}
