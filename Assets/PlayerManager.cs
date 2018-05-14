using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RoboRyanTron.Unite2017.Variables;

public enum PlayerStates { none, live, dead };

public class PlayerManager : MonoBehaviour {

    PlayerMovement movement;

    public UnityEvent secondChance;

    bool isDead = false;
    int health = 1;
    float dieTorque = 360f;

    public float invisbleTime = 2f;
    private bool isInvisible = false;
    private float invisibleTimePassed = 0;

    public PlayerStates state = PlayerStates.none;

    public GameObject myBody;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {      
        invisibleTimePassed += Time.deltaTime;
        if (invisibleTimePassed > invisbleTime) {
            isInvisible = false;
        }

        if (isDead)
        {
            movement.SetMovementType(MovementType.FlyGhost);
            gameObject.transform.Find("live").gameObject.SetActive(false);
            gameObject.transform.Find("die").gameObject.SetActive(true);
            movement.SetTorque(dieTorque);
            state = PlayerStates.dead;
        }
        else {
            movement.SetMovementType(MovementType.plataform2D);
            gameObject.transform.Find("live").gameObject.SetActive(true);
            gameObject.transform.Find("die").gameObject.SetActive(false);
            state = PlayerStates.live;
        }
    }

    public void Hit(int hitPoint) {
        if (!isInvisible)
        {
           
            if (movement != null)
            {
                Debug.Log("HIT");
                movement.SetVerticalInpulse(5f);
                health--;

                if (health <= 0)
                {
                    Die();
                }
                isInvisible = true;
                invisibleTimePassed = 0;
            }
        }
    }

    public void Die() {
        Debug.Log("DIE");

        // check if has orbs
        GameObject gameManagerObject = GameObject.Find("Game Manager");
        GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
        StringVariable orbVariable = gameManager.orbVariable;
        int orbVariableValue = int.Parse(orbVariable.Value);

        if (orbVariableValue <= 0)
        {
            secondChance.Invoke();
            return;
        }
        else {
            orbVariableValue--;
            orbVariable.Value = orbVariableValue.ToString();
        }
      
        if (!isDead)
        {
            myBody = gameManager.ClonePlayer();
            health = 1;
            isDead = true;
        }
    }

    public void Revive() {
        PlayerRevive revice = GetComponent<PlayerRevive>();      
        revice.onComplete.AddListener(ReviveImediataly);
        revice.Revive();      
    }

    public void ReviveImediataly() {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("PlayerClone"));
        health = 1;
        isDead = false;
    }


}
