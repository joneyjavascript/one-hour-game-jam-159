using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitFlag {

    public string name;
    public GameObject hitAreaGameObject;

    [HideInInspector]
    public bool hit { get; private set; }

    [HideInInspector]
    public Collider2D[] colliders { get; private set; }

    [HideInInspector]
    public HitArea hitArea { get; private set; }

    public void Start()
    {
        hitArea = hitAreaGameObject.GetComponent<HitArea>();
    }

    public void Update() {
        if (hitArea != null)
        {
            colliders = hitArea.GetHits();
            if (hitArea.HasHits())
            {
                hit = true;
                
            }
            else
            {
                hit = false;                
            }
        }
    }

}

public class HitAreaFlags : MonoBehaviour {

    public List<HitFlag> flags;

	// Use this for initialization
	void Start () {
        foreach (HitFlag flag in flags)
        {
            flag.Start();
        }
    }
	
	// Update is called once per frame
	void Update () {
        foreach (HitFlag flag in flags) {
            flag.Update();
        }
	}

    public bool CheckHit(string hitFlagName) {
        HitFlag flag = GetHitFlagByName(hitFlagName);

        if (flag == null) {
            return false;
        }
     
        return flag.hit;
    }

    public HitFlag GetHitFlagByName(string name)
    {
        HitFlag flag = flags.Find(x => x.name == name);
        if (flag == null)
        {
            Debug.LogWarning("Hit Flag with name '" + name + "' not found.");
        }
        return flag;
    }

}
