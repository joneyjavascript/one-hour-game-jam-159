using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour {

    private void Start()
    {
        playMusic();
    }

    public void playMusic() {
        AudioManager.instance.Play("credits-music");
    }

    public void BackToMenu()
    {        
        GotoScene("MenuScene");
    }
    
    public void GotoScene(string nestSceneName) {
        AudioManager.instance.Play("ui-back");
        SceneManager.LoadScene(nestSceneName);
    }

}
