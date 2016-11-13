using UnityEngine;
using System.Collections;

public class audioSourceScript : MonoBehaviour {

    public void startPlayingfx(AudioClip a) {
        GetComponent<AudioSource>().clip = a;
        GetComponent<AudioSource>().priority = 255;
        StartCoroutine(play());
    }
    public void startPlayingMusic(AudioClip a) {
        GetComponent<AudioSource>().clip = a;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().volume = 0.04f;
        GetComponent<AudioSource>().priority = 0;
        GetComponent<AudioSource>().Play();
    }

    IEnumerator play() {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        Destroy(gameObject);
    }

}
