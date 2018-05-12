using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerInteractEvent : UnityEvent<PlayerInteraction> {    

}

public class PlayerInteraction : MonoBehaviour {

    public string type;
    public string value;
    public string interactionSound;
    public bool interactOnTrigger = false;
    public PlayerInteractEvent OnInteract;

    Collider2D overlap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        overlap = collision;
    }    

    private void OnTriggerExit2D(Collider2D other)
    {
        overlap = null;
    }

    private void Update()
    {
        if (overlap != null){
            UpdateInteraction(overlap);
        }
    }

    private void UpdateInteraction(Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetButtonDown("Fire1") || interactOnTrigger)
            {

                if (AudioManager.instance.HasSound(interactionSound))
                {
                    AudioManager.instance.Play(interactionSound);
                }

                if (OnInteract != null)
                {
                    OnInteract.Invoke(this);
                }
            }
        }
    }

}
