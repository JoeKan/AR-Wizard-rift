using UnityEngine;
using System.Collections;

public class UbiTrack_ConnectComponents : MonoBehaviour {

    public string[] Names;
    public GameObject[] Objects;

	// Use this for initialization
	void Start () {
        /*for(int i=0;i<Names.Length;i++){
            GameObject go = GameObject.Find(Names[i]);
            Objects[i].transform.parent = go.transform;

        }*/
	    
	}

     void OnDestroy() {
         /*Debug.Log("foo");
         for (int i = 0; i < Names.Length; i++)
         {          
             Objects[i].transform.parent = null;

         }*/
     }

	// Update is called once per frame
	void Update () {
	
	}
}
