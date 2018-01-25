using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FAR
{

    [RequireComponent(typeof(LineRenderer))]
    public class LaserPointer : InteractionMethod
    {

        public float laserRange = 5.0f;
        public float width = 1.0f;
        public LayerMask interactableObjects;
        public direction Direction;
        public bool stopRayAtTarget = true;
        public Color defaultColor;
        public Color interactColor;
        public GameObject CursorPrefab;

        private GameObject Cursor;

        public enum direction
        {
            FORWARD,
            BACKWARD,
            RIGHT,
            LEFT,
            UP,
            DOWN
        };

        private LineRenderer laser;

        // Use this for initialization
        override public void Start()
        {
            base.Start();

            if (defaultColor.a == 0 || interactColor.a == 0) 
            {
                Debug.LogWarning("Attention: One or multiple of your LaserPointer Colors have a ZERO value in their alpha channel. That makes them entirely invisible");
            }

            if (CursorPrefab != null)
            {
                Cursor = Instantiate(CursorPrefab, this.transform.position, Quaternion.identity) as GameObject;

                List<Collider> pointerColliders = new List<Collider>(Cursor.GetComponents<Collider>());
                pointerColliders.AddRange(Cursor.GetComponentsInChildren<Collider>());

                for (int i = 0; i < pointerColliders.Count; i++)
                {
                    Destroy(pointerColliders[i]);
                }

                pointerColliders.Clear();

            }

            laser = this.GetComponent<LineRenderer>();
			laser.positionCount = 2; //SetVertexCount(2);
            laser.material = new Material(Shader.Find("Particles/Additive"));
			laser.startWidth = laser.endWidth = width; //SetWidth(width, width);
			laser.startColor = laser.endColor = defaultColor; //SetColors(defaultColor, defaultColor);
            updateLaser();
        }

        // Update is called once per frame
		override public void Update()
		{
			base.Update();
            updateLaser();
        }

        void updateLaser()
        {
            laser.SetPosition(0, this.transform.position);
            
			//irrelevant, with no references to the width that change its value 
			//(except possibly inspector tweaking in the editor for testing)
			//laser.SetWidth(width, width);

            Vector3 dir = new Vector3(0, 0, 0);
            switch (Direction)
            {
                case (direction.FORWARD):
                    dir = this.transform.forward;
                    break;
                case (direction.BACKWARD):
                    dir = -this.transform.forward;
                    break;
                case (direction.RIGHT):
                    dir = this.transform.right;
                    break;
                case (direction.LEFT):
                    dir = -this.transform.right;
                    break;
                case (direction.UP):
                    dir = this.transform.up;
                    break;
                case (direction.DOWN):
                    dir = -this.transform.up;
                    break;
            }

            Ray ray = new Ray(this.transform.position, dir);
            if (!rayCast(ray))
            {
                laser.SetPosition(1, ray.GetPoint(laserRange));
                if (Cursor != null)
                {
                    Cursor.transform.position = ray.GetPoint(laserRange);
                }
				laser.startColor = laser.endColor = defaultColor;
				//laser.SetColors(defaultColor, defaultColor);
            }
            else
            {
				laser.startColor = laser.endColor = interactColor;
                //laser.SetColors(interactColor, interactColor);
            }
        }

        bool rayCast(Ray ray)
        {
            RaycastHit info;
            if (Physics.Raycast(this.transform.position, ray.direction, out info, laserRange, interactableObjects))
            {


                if (stopRayAtTarget)
                {
                    laser.SetPosition(1, info.point);
                    if (Cursor != null)
                    {
                        Cursor.transform.position = info.point;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse0)) 
                {
                    Debug.Log("Selected " + info.collider.name);
                    SelectionEvent sevt = new SelectionEvent(info.transform.gameObject,this, null);
                    fireEvent(sevt);
                    return true;
                }

                HoverEvent hevt = new HoverEvent(info.transform.gameObject, info.point,this, null);
                fireEvent(hevt);
                return true;
            }
            return false;
        }

    }

}
