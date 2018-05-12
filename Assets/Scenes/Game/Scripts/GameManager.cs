using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private void Start()
    {        
        playMusic();
    }

    public void playMusic()
    {
        AudioManager.instance.Play("game-music");
    }

    private float _oldTimeScale;
    private bool paused = false;
    
    public void PauseGame()
    {
        if (paused)
        {
            throw new UnityException("Game is paused, not pause again.");
        }

        _oldTimeScale = Time.timeScale;
        Time.timeScale = 0;
        paused = true;        
    }

    public void ResumeGame()
    {
        if (!paused)
        {
            throw new UnityException("Game is not paused, can not resume it.");
        }

        Time.timeScale = _oldTimeScale;
        paused = false;
       
    }

    public void BackToMenu()
    {
        GotoScene("MenuScene");
    }
    
    public void GotoScene(string nestSceneName) {
        AudioManager.instance.Play("ui-back");
        SceneManager.LoadScene(nestSceneName);
    }
    
    public void PlayerInteract(PlayerInteraction interaction) {
        Debug.Log(interaction.type);

        if (interaction.type == "Coin")
        {
            Debug.Log("Coin Collect");
        }


        if (interaction.type == "NextScene")
        {
            GotoScene(interaction.value);
        }

        if (interaction.type == "Enemy")
        {
            GotoScene("MenuScene");
        }

        if (interaction.type == "playerHit") {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerManager manager = player.GetComponent<PlayerManager>();
            manager.Hit(int.Parse(interaction.value));
        }
    }



}
