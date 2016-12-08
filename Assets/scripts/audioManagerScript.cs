using UnityEngine;
using System.Collections;

public class audioManagerScript : MonoBehaviour {

    public static audioManagerScript instance = null;

    public AudioClip[] audiofx;
    public AudioClip[] audiomusic;

    public Transform audioTransform;

    Transform music;
    int musicNo;
    bool stopping = false;

    void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
    public void playfxSound(int index) {
        var source = Instantiate(audioTransform) as Transform;
        source.SetParent(transform);
        source.GetComponent<audioSourceScript>().startPlayingfx(audiofx[index]);
    }

    public void playMusic(int index) {
        if (music != null) {
            if (music.gameObject != null && musicNo == index) {
                return;
            } else if (music.gameObject != null) {
                Destroy(music.gameObject);
            }
        }
        music = Instantiate(audioTransform) as Transform;
        music.SetParent(transform);
        musicNo = index;
        music.GetComponent<audioSourceScript>().startPlayingMusic(audiomusic[index]);
    }

    public void stopMusic() {
        if (music != null) {
            if (music.gameObject != null) {
                StartCoroutine(stoppingMusic());
                musicNo = -1;
            }
        }
    }
    IEnumerator stoppingMusic() {
        stopping = true;
        yield return new WaitForSecondsRealtime(1f);
        stopping = false;
        Destroy(music.gameObject);
    }

	// Update is called once per frame
	void Update () {
        if (music == null)
            return;
        if (music.gameObject == null)
            return;
        if (stopping)
            music.gameObject.GetComponent<AudioSource>().volume -= Time.fixedDeltaTime * 0.04f;
        else
            music.gameObject.GetComponent<AudioSource>().volume = 0.06f;

    }
}
