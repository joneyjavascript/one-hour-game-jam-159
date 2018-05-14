using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTarget : MonoBehaviour {

    public GameObject target;
    public float distance = -1;
    public float seekFactor = .8f;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate() {

        if (target == null)
        {
            return;
        }

        Vector3 targetPosition = target.transform.position;
        targetPosition.z = distance;
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, seekFactor);
	}
}
