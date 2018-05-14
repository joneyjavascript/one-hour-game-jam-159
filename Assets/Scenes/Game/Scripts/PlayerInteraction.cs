using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerInteractEvent : UnityEvent<PlayerInteraction> {

    public GameObject interactWith;

}

public class PlayerInteraction : MonoBehaviour {

    [HeaderAttribute("Interact On")]
    public PlayerStates[] playerStates;
    public bool enter = false;
    public bool stay = true;
    public bool exit = false;

    [HeaderAttribute("Attributes")]
    public float frequencyInteraction = 0;
    public float frequencyTimePassed;
    public string type;
    public string value;
    public string interactionSound;
    public bool interactOnTrigger = false;
    public PlayerInteractEvent OnInteract;
    

    Collider2D overlap;

    private void Start()
    {
        OnInteract = new PlayerInteractEvent();
        OnInteract.AddListener(GameObject.Find("Game Manager").GetComponent<GameManager>().PlayerInteract);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        overlap = collision;
        if (enter)
        {
            UpdateInteraction(overlap);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (exit)
        {
            UpdateInteraction(overlap);
        }
        overlap = null;        
    }

    private void Update()
    {
        frequencyTimePassed += Time.deltaTime;

        if (overlap != null){
            if (stay)
            {
                UpdateInteraction(overlap);
            }
        }
    }

    private void UpdateInteraction(Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {            
            PlayerManager manager = collision.gameObject.GetComponent<PlayerManager>();

            bool containState = false;
            bool hasStates = (playerStates != null && playerStates.Length > 0);
            foreach (PlayerStates state in playerStates) {
                if (state.Equals(manager.state)){
                    containState = true;
                }
            }
            
            if (Input.GetButtonDown("Fire1") || interactOnTrigger && frequencyTimePassed >= frequencyInteraction && (!hasStates || containState))
            {
                if (AudioManager.instance.HasSound(interactionSound))
                {
                    AudioManager.instance.Play(interactionSound);
                }

                if (OnInteract != null)
                {
                    OnInteract.interactWith = gameObject;
                    OnInteract.Invoke(this);
                }

                frequencyTimePassed = 0;
            }
        }
    }

}
