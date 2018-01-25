using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FAR
{
    public class MouseObjectSelector : InteractionMethod, Action {
    
		public Camera RaycastCamera = null;
        public bool virtualCursor = false;

    	MouseEvent LastMouseEvent;
        GameObject CurrDragged;

        bool clickHold = false;

        private float holdingTimeUntilGrab = 1.0f;
        private float runningholdingTimer;
        private GameObject cursor;


    	public void doEvent(InteractionEvent evt)
    	{
    		if(evt.GetType() == typeof(MouseEvent))
            {

                MouseEvent mevt = (MouseEvent) evt;

                if (mevt.mouseButton == MouseButton.Left && mevt.UpDown == MouseState.Down) 
                {
                    clickHold = true;
                }

                if (clickHold && (mevt.mouseButton == MouseButton.Left && mevt.UpDown == MouseState.Up)) 
                {
                    clickHold = false;
                    runningholdingTimer = holdingTimeUntilGrab;
                }

                Vector3 hit = new Vector3();
                GameObject clickedOn = DidIClickOnSomething(mevt, ref hit);


                //Haben wir etwas angeklickt senden wir es an alle EventListeners
                if(clickedOn != null && mevt.UpDown == MouseState.Down)
                {
                    if (clickHold && runningholdingTimer <= 0) 
                    {
                        GrabEvent gevt = new GrabEvent(clickedOn,null,this,mevt);
                        if (virtualCursor) 
                        {
                            gevt.parentTo = cursor.transform;
                        }
                        fireEvent(gevt);
                    }

                    SelectionEvent sevt = new SelectionEvent(clickedOn, this, mevt);
                    fireEvent(sevt);
                    if (mevt.mouseButton == MouseButton.Left)
                    {
                        SelectionLmbEvent slevt = new SelectionLmbEvent(clickedOn, hit, this.GetComponent<InteractionMethod>(), mevt);
                        fireEvent(slevt);
                    }
                    else if (mevt.mouseButton == MouseButton.Right)
                    {
                        SelectionRmbEvent srevt = new SelectionRmbEvent(clickedOn, hit, this.GetComponent<InteractionMethod>(),mevt);
                        fireEvent(srevt);
                    }
                }
                //Hovern wir über ein Objekt muss dies als eigenes Event gesendet werden
                else if(clickedOn != null && mevt.UpDown == MouseState.NONE)
                {
                	HoverEvent hevt = new HoverEvent(clickedOn,hit,this.GetComponent<InteractionMethod>(),mevt);
                	fireEvent(hevt);
                }
            }
    	}
    	
        override public void Start()
        {
            base.Start();
            runningholdingTimer = holdingTimeUntilGrab;
        }
    	
		override public void Update()
		{
			base.Update();
            if (clickHold) 
            {
                runningholdingTimer -= Time.deltaTime;
            }
    	}
    	
    	/// <summary>
    	/// Checks for CursorSelection with any Collider-Objekt and returns the gameobject.
    	/// Requires CAMERA.MAIN to work
    	/// </summary>
    	/// <returns>The I click on something.</returns>
    	/// <param name="evt">Evt.</param>
    	public GameObject DidIClickOnSomething (MouseEvent evt, ref Vector3 hit)
    	{
    		//REM
			Camera MainCam;
			if(RaycastCamera == null)
            {
                MainCam = Camera.main;
            }
            else
            {
                MainCam = RaycastCamera;
            }
    		
    		Vector3 AimingAt = MainCam.ScreenToWorldPoint (new Vector3 (evt.xPos, evt.yPos, 100));
    		
    		Ray ray = new Ray (MainCam.transform.position, AimingAt - MainCam.transform.position);
    		
    		RaycastHit Hitinfo = new RaycastHit ();
    	

    		if (Physics.Raycast (ray,out Hitinfo,10000f))
    		{
                hit = Hitinfo.point;

                if (virtualCursor)
                {
                    if (cursor == null)
                    {
                        cursor = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
                        cursor.GetComponent<Collider>().enabled = false;
                        cursor.transform.localScale = new Vector3(0.1f, 0.1f, 0.01f);
                    }

                    cursor.transform.position = Hitinfo.point;
                }

    			return Hitinfo.transform.gameObject;
    		}
    		else
    		{
                hit = Vector3.zero;
    			return null;
    		}
    	}


    }


}
