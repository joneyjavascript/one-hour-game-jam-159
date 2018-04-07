using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    private void Start()
    {
        playMusic();
    }

    public void playMusic()
    {
        AudioManager.instance.Play("menu-music");
    }

    public void StartGame()
    {
        GotoScene("GameScene");
    }

    public void SeeCredits()
    {
        GotoScene("CreditsScene");
    }
    
    public void GotoScene(string nestSceneName) {
        AudioManager.instance.Play("ui-click");
        SceneManager.LoadScene(nestSceneName);
    }

}
