using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class mainMenuUIPreview : MonoBehaviour {
    
	public void StartGame() {
        SceneManager.LoadScene("UIPreview");
    }

}
