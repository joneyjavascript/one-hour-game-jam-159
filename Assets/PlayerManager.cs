using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour {

    PlayerMovement movement;

    public UnityEvent secondChance;

    bool isDead = false;
    int health = 1;
    float dieTorque = 360f;

    public float invisbleTime = 2f;
    private bool isInvisible = false;
    private float invisibleTimePassed = 0;

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
        }
        else {
            gameObject.transform.Find("live").gameObject.SetActive(true);
            gameObject.transform.Find("die").gameObject.SetActive(false);
        }
    }

    public void Hit(int hitPoint) {
        if (!isInvisible)
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

    public void Die() {
        Debug.Log("DIE");
        if (isDead)
        {
            Debug.Log("Second Chance");
            secondChance.Invoke();
        }
        else
        {
            health = 1;
            isDead = true;
        }
    }


}
