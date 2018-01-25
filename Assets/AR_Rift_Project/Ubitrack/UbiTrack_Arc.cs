using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FAR
{
    public class UbiTrack_Arc : MonoBehaviour
    {

        //Save the following components in our scene
        public bool simpleFacade = false;
        public bool ubiTrackComponent = false;
        public bool arc = false;
        public bool arrift = false;

        // Use this for initialization
        void Start()
        {
            saveUbiTrackItems();
        }

        // Update is called once per frame
        void Update()
        {
			if(Input.GetKeyDown(KeyCode.F1)){
				SceneManager.LoadScene(0);
				return;
			}

			if(Input.GetKeyDown(KeyCode.F2)){
				SceneManager.LoadScene(1);
				return;
			}
			if(Input.GetKeyDown(KeyCode.F3)){
				SceneManager.LoadScene(2);
				return;
			}
			if(Input.GetKeyDown(KeyCode.F4)){
				SceneManager.LoadScene(3);
				return;
			}
			if(Input.GetKeyDown(KeyCode.F5)){
				SceneManager.LoadScene(4);
				return;
			}
        }



        void saveUbiTrackItems()
        {

            if (this.arc)
            {
                DontDestroyOnLoad(this.gameObject);
            }

            if (this.simpleFacade)
            {

                UTFacade[] allSimpleFacades = GameObject.FindObjectsOfType(typeof(UTFacade)) as UTFacade[];

                foreach (UTFacade sf in allSimpleFacades)
                {
                    DontDestroyOnLoad(sf.gameObject);
                }
            }

            if (this.ubiTrackComponent)
            {
                UbiTrackComponent[] allUbiTrackComponents = GameObject.FindObjectsOfType(typeof(UbiTrackComponent)) as UbiTrackComponent[];

                foreach (UbiTrackComponent uc in allUbiTrackComponents)
                {
                    DontDestroyOnLoad(uc.gameObject);
                }
            }

            if (this.arrift)
            {
                GameObject ARRift = GameObject.Find("ARRIFT");
                if (ARRift != null)
                {
                    DontDestroyOnLoad(ARRift);
                }
                else
                {
                    Debug.LogError("ARRIFT not found");
                }
            }
        }
    }
}
    
