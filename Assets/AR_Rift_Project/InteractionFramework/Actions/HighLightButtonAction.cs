using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FAR
{

    public class HighLightButtonAction : MonoBehaviour, Action
    {

        //Which level to load when clicked on
        public int levelToLoad;
        //Reference to the attached button
        UnityEngine.UI.Button myButton;
        public GameObject ReturnObj()
        {
            return this.gameObject;
        }

        public void doEvent(InteractionEvent evt)
        {
            if (evt.GetType() == typeof(HoverEvent))
            {
                HoverEvent hevt = (HoverEvent)evt;

                if (hevt.Obj == this.gameObject) 
                {
                    if (myButton != null) 
                    {
                        Debug.Log("Selecting button");
                        myButton.Select();
                    }
                }
            }

            if (evt.GetType() == typeof(SelectionEvent))
            {
                SelectionEvent sevt = (SelectionEvent)evt;

                if (sevt.Obj == this.gameObject)
                {
                    Debug.Log("Calling new level");
					UnityEngine.SceneManagement.SceneManager.LoadScene (levelToLoad);
					//Application.LoadLevel(levelToLoad);
                }

            }
        }

        // Use this for initialization
        void Start()
        {
            myButton = this.GetComponent<UnityEngine.UI.Button>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
