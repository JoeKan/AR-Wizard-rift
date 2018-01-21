using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsEnemyScript : MonoBehaviour {

    //reference to script attached to projectile
    GesturesMovementScript scriptReference = new GesturesMovementScript();

    //hit counter int variable coutning up with each hit
    int hitCountEnemy;

    // Use this for initialization
    void Start () {
        hitCountEnemy = 0;
	}

    private void OnTriggerEnter(Collider other)
    {
        scriptReference.DestroyProjectile();
        
        hitCountEnemy++;
        Debug.Log("HitCounter: " + hitCountEnemy);
        if (hitCountEnemy >= 5)
        {
            Debug.Log("WIN");
            this.gameObject.SetActive(false);
        }
    }
}
