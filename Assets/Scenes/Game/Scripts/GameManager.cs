using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public StringVariable orbVariable;
    public StringVariable CurrentLevel;

    public GameObject Transition;

    private void Start()
    {
        orbVariable.Value = "0";
        if (Transition != null)
        {
            Transition.SetActive(false);
        }

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
        ClonePlayer();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        StartCoroutine(LoadYourAsyncScene(nestSceneName));
    }

    public GameObject ClonePlayer() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject clone = GameObject.Instantiate(player);
        clone.GetComponent<HitAreaFlags>().enabled = false;
        clone.GetComponent<PlayerMovement>().enabled = false;
        clone.GetComponent<PlayerManager>().enabled = false;
        clone.GetComponent<PlayerRevive>().enabled = false;
        clone.layer = LayerMask.NameToLayer("PlayerClone");
        clone.tag = "PlayerClone";
        return clone;
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        float alpha = 0;
        float secondsAlpha = 2f;

        if (Transition != null)
        {
            Transition.SetActive(true);

            while (alpha < secondsAlpha)
            {               
                Image render = Transition.GetComponent<Image>();
                Color color = new Color(render.color.r, render.color.g, render.color.b, alpha);
                render.color = color;
                alpha += Time.deltaTime;
                yield return null;
            }
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
               
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void PlayerInteract(PlayerInteraction interaction) {
        Debug.Log(interaction.type);

        if (interaction.type == "Coin")
        {
            Debug.Log("Coin Collect");
        }
        
        if (interaction.type == "NextScene")
        {
            CurrentLevel.Value = interaction.value;
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

        if (interaction.type == "CollectOrb")
        {            
            if (orbVariable == null) {
                Debug.Log("orbVariable undefined");
                return;
            }

            int orbValue = int.Parse(orbVariable.Value);
            orbValue += int.Parse(interaction.value);
            orbVariable.Value = orbValue.ToString();

            GameObject.Destroy(interaction.OnInteract.interactWith);
        }

        if (interaction.type == "CollectMaterialOrb")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerManager manager = player.GetComponent<PlayerManager>();
            manager.Revive();
        }

    }



}
