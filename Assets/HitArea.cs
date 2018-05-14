using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitAreaEvent : UnityEvent<Collider2D[]> {
    public Collider2D[] colliders;
}

public class HitArea : MonoBehaviour {        
    public float radius = 1f;
    public LayerMask layerMask;

    public HitAreaEvent OnHit;

    Collider2D[] collisionList;
  
    private void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate () {
        CheckHits();
        if (HasHits()) {
            if (OnHit != null)
            {
                OnHit.Invoke(GetHits());
            }
        }      
	}

    public bool HasHits() {
        return (collisionList != null && collisionList.Length > 0);
    }
    
    public void CheckHits()
    {
        collisionList = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
    }

    public Collider2D[] GetHits() {
        return collisionList;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
