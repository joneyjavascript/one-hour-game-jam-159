using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RoboRyanTron.Unite2017.Variables;

public class MenuManager : MonoBehaviour {

    public StringVariable CurrentLevel; 

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
        CurrentLevel.Value = "Level 1";
        GotoScene("Level 1");
    }

    public void ContinueGame()
    {
        GotoScene(CurrentLevel.Value);
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
