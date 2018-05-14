using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { plataform2D, FlyGhost }

public class PlayerMovement : MonoBehaviour {

    public MovementType currentMovementType = MovementType.plataform2D;

    public float speed = 300;
    public float jumptHeight = 50;

    public float flyInpulse = .25f;
    public float flyForce = 2f;
    
    Rigidbody2D body;

    HitAreaFlags hitAreaFlags;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        hitAreaFlags = GetComponent<HitAreaFlags>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        body.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, body.velocity.y);

        if (currentMovementType == MovementType.FlyGhost)
        {
            AddVerticalInpulse(flyInpulse);

            if (Input.GetButtonDown("Jump"))
            {
                AudioManager.instance.Play("FlyGhost");
                AddVerticalInpulse(flyForce);
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && hitAreaFlags.CheckHit("InFloor"))
            {
                AudioManager.instance.Play("Jump");
                SetVerticalInpulse(jumptHeight);
            }
        }
    }

    public void SetVerticalInpulse(float inpulse) {
        body.velocity = new Vector2(body.velocity.x, inpulse);
    }

    public void SetTorque(float torque)
    {
        body.angularVelocity = torque;
    }

    
    public void AddVerticalInpulse(float inpulse)
    {
        body.velocity = new Vector2(body.velocity.x, body.velocity.y + inpulse);
    }

    public void SetMovementType(MovementType newMovementType) {
        this.currentMovementType = newMovementType;
    }

}
