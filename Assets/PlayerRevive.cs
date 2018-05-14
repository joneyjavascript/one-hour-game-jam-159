using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine.Events;

public enum ReviveState { none, reviveProcess, complete }

public class PlayerRevive : MonoBehaviour {

    public FloatReference reviveArea;
    public FloatReference reviveForce;
    public FloatReference reviveeMaxSpeed;
    public FloatReference reviveeMinSpeed;

    private GameObject MyBody;
    private ReviveState state = ReviveState.none;

    public UnityEvent onComplete;
    public Collider2D collider;

    public GameObject lastTarget;

    public void Revive() {
        if (MyBody == null)
        {
            MyBody = GameObject.FindGameObjectWithTag("PlayerClone");
            return;
        }

        Debug.Log("Revive Start");        
        state = ReviveState.reviveProcess;
        lastTarget = Camera.main.GetComponent<SeekTarget>().target;
        Camera.main.GetComponent<SeekTarget>().target = MyBody;
    }
   
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (MyBody == null) {           
            MyBody = GameObject.FindGameObjectWithTag("PlayerClone");            
            return;
        }


        if (state == ReviveState.reviveProcess)
        {
            // move body to soul
            Rigidbody2D body = MyBody.GetComponent<Rigidbody2D>();
            Vector3 myDirection = transform.position - MyBody.transform.position;
            myDirection.Normalize();
            float speed = Vector3.Distance(MyBody.transform.position, transform.position);
            speed = speed * reviveForce;
            speed = Mathf.Clamp(speed, reviveeMinSpeed.Value, reviveeMaxSpeed.Value);
            body.velocity = myDirection * speed;
            body.gravityScale = 0;
            MyBody.GetComponent<BoxCollider2D>().enabled = false;

            // check if body is on soul
            collider = Physics2D.OverlapCircle(MyBody.transform.position, reviveArea.Value, LayerMask.GetMask(new string[1] { "Player" }));
            if (collider != null)
            {              
                state = ReviveState.complete;
            }
        }

        if (state == ReviveState.complete)
        {
            Camera.main.GetComponent<SeekTarget>().target = lastTarget;
            if (onComplete != null)
            {
                onComplete.Invoke();
            }

            state = ReviveState.none;
        }
    }

    private void OnDrawGizmos()
    {        
        if (MyBody == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(MyBody.transform.position, reviveArea.Value);
    }
}
