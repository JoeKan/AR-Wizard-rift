using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FAR{

    public class CameraAction : MonoBehaviour , Action {
    
    	/*
    	
    	Anmerkung:
    	
    	Dieses Skript ermöglicht einfache Kameradrehungen um das Geschehen. Eine Art Globus-Betrachtung.
    	Um dies zu ermöglichen muss ein Empty Gameobject (Hier Rotation Center in der StartMethode) erstellt und
    	als Mittelpunkt der Drehung genutzt werden.
    	Sie können natürlich ein eigenes Objekt als Parent der Kamera nutzen und die hießige StartMethode einfach entfernen.
    	
    	(Vergessen sie nicht, dass die Kameradrehung auf Events basiert. Setzen sie die MainCamera mit diesem Skript
    	als Listener der gewünschten Interaktionsmethode ein)
    	
    	*/
    
        public float Camera_Rotation_Speed = 5.0f;
        
        public bool MovementBlocked = false;
        
		public GameObject ReturnObj()
		{
			return this.gameObject;
		}
        
        
        void Start()
        {
        	GameObject RotationCenter = new GameObject("CameraRotationCenter");
        	RotationCenter.transform.position = new Vector3(0,0,0);
        	this.gameObject.transform.parent = RotationCenter.transform;
        }
        
    
        public void doEvent(InteractionEvent evt)
        {
    
        
            if(evt.GetType() == typeof(KeyboardEvent) && MovementBlocked == false)
            {
                KeyboardEvent k_evt = (KeyboardEvent) evt;
                if(k_evt.key=='a')
                {
                    this.transform.parent.Rotate(Vector3.up,-(Camera_Rotation_Speed)*Time.deltaTime);
                }
                if(k_evt.key=='d')
                {
                    this.transform.parent.Rotate(Vector3.up,+(Camera_Rotation_Speed)*Time.deltaTime);
                }
                if(k_evt.key=='w')
                {
                    Vector3 OldPos = this.transform.parent.transform.position;
                    this.transform.parent.transform.position = new Vector3(OldPos.x,OldPos.y+5.0f*Time.deltaTime,OldPos.z);
                }
                if(k_evt.key=='s')
                {
                    Vector3 OldPos = this.transform.parent.transform.position;
                    this.transform.parent.transform.position = new Vector3(OldPos.x,OldPos.y-5.0f*Time.deltaTime,OldPos.z);
                }
            }
        }
    }

}
