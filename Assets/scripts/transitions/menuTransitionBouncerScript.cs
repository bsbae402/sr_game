using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuTransitionBouncerScript : MonoBehaviour {
    // This class is also trivial.  It's the same as that background bouncer, but for canvas sprites
    
    public Vector2 bouncer = new Vector2(0, 0);
    public string picture;
    public int anim;

    int animCount;

    Vector3 initScale;

    void Start() {
        animCount = 0;
        initScale = transform.localScale;
    }

    public void changeAnim(int a) {
        anim = a;
        GetComponent<Image>().sprite = Resources.Load<Sprite>(picture + "-" + anim + "-1");
    }

    void FixedUpdate() {
        transform.localScale = new Vector3(initScale.x + bouncer.x * (float)System.Math.Sin(Time.time),
            initScale.y + bouncer.y * (float)System.Math.Sin(Time.time));
        if (anim < 1)
            return;
        if (animCount % 60 == 29)
            GetComponent<Image>().sprite = Resources.Load<Sprite>(picture + "-" + anim + "-1");
        else if (animCount % 60 == 59) {
            GetComponent<Image>().sprite = Resources.Load<Sprite>(picture + "-" + anim + "-2");
            animCount = 0;
        }
        animCount++;
    }
}
