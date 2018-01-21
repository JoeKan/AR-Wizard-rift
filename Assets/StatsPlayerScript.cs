using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPlayerScript : MonoBehaviour {

    int hitCountPlayer;

	// Use this for initialization
	void Start () {
        hitCountPlayer = 0;
	}

    private void OnTriggerEnter(Collider other)
    {
        hitCountPlayer++;
        Debug.Log("Player Hit Count " + hitCountPlayer);
        if (hitCountPlayer >= 10)
        {
            Debug.Log("DEATH");
            Destroy(this.gameObject);
        }
    }
}
