using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FAR
{
    [RequireComponent(typeof(LineRenderer))]
    public class LeapMotionInteraction : InteractionMethod
    {
        public GameObject thumbTip;
        public GameObject indexTip;
        public GameObject middleTip;
        public GameObject ringTip;
        public GameObject littleTip;
        public GameObject palm;

        public GameObject indexRayCastReference;
        private LineRenderer myLineRay;
        private float rayLength = 10.0f;
        private float rayWidth = 0.01f;

        // Use this for initialization
        override public void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        override public void Update()
        {
            base.Update();
            if (checkForPointingGesture() || checkForSelectionGesture()) 
            {
                showPointingRay();
            }
            checkForGrabbingGesture();
        }

        bool checkForPointingGesture() 
        {
            Vector3 thumb = thumbTip.transform.localPosition;
            Vector3 index = indexTip.transform.localPosition;
            Vector3 middle = middleTip.transform.localPosition;

            if (Mathf.Abs(Mathf.Abs(middle.y) - Mathf.Abs(index.y)) >= 0.03)
            {
                //Index muss weiter vorne sein
                if ((index.z - thumb.z) >= 0.1f)
                {
                    //Daumen muss weiter links gestreckt sein, also muss sein X kleiner sein
                    if (Mathf.Abs(Mathf.Abs(index.x) - Mathf.Abs(thumb.x)) >= 0.05f)
                    {
                        //Beide Finger müssen auf einer Höhenebene sein
                        if (Mathf.Abs(Mathf.Abs(index.y) - Mathf.Abs(thumb.y)) <= 0.1)
                        {
                            Debug.Log("=========== POINTING ==========");
                            Ray ray = new Ray(indexTip.transform.position, (indexTip.transform.position - palm.transform.position).normalized);

                            RaycastHit hitinfo;
                            if (Physics.Raycast(ray, out hitinfo))
                            {
                                SelectionEvent sevt = new SelectionEvent(hitinfo.transform.gameObject);
                                fireEvent(sevt);
                            }
                            return true;
                        }
                        else
                        {
                            //Debug.Log("Height difference! I: " + index.y + " T: " + thumb.y);
                        }
                    }
                    else
                    {
                        //Debug.Log("LEFTRIGHT Not Fitting! I: " + index.x + " T: " + thumb.x);
                    }
                }
                else
                {
                    //Debug.Log("Index not far enough ahead on Z! I: " + index.z + " T: " + thumb.z);
                }
            }
            else 
            {
                //Debug.Log("MiddleFinger and Index not far enough away on Y-Axis! INDEX Y : " + index.y + " MIDDLE Y " + middleTip.transform.localPosition.y);
            }
            return false;
        }

        bool checkForGrabbingGesture() 
        {
            Vector3 thumb = thumbTip.transform.localPosition;
            Vector3 index = indexTip.transform.localPosition;
            Vector3 middle = middleTip.transform.localPosition;

            float Ydiff_Th_In = Mathf.Abs(Mathf.Abs(thumb.y) - Mathf.Abs(index.y));
            float Ydiff_In_Mid = Mathf.Abs(Mathf.Abs(index.y) - Mathf.Abs(middle.y));
            float Ydiff_Th_Mid = Mathf.Abs(Mathf.Abs(thumb.y) - Mathf.Abs(middle.y));

            //All on same height
            if (Ydiff_Th_In <= 0.018f && Ydiff_In_Mid <= 0.018f && Ydiff_Th_Mid <= 0.018f)
            {
                //0.02 Difference on X Axis
                float Xdiff_Th_In = Mathf.Abs(Mathf.Abs(thumb.x) - Mathf.Abs(index.x));
                float Xdiff_In_Mid = Mathf.Abs(Mathf.Abs(index.x) - Mathf.Abs(middle.x));
                float Xdiff_Th_Mid = Mathf.Abs(Mathf.Abs(thumb.x) - Mathf.Abs(middle.x));
                if (Xdiff_Th_In <= 0.02f && Xdiff_In_Mid <= 0.02f && Xdiff_Th_Mid <= 0.02f) 
                {
                    Debug.Log("=========== GRABBING ===========");
                    Ray ray = new Ray(indexTip.transform.position, (indexTip.transform.position - palm.transform.position).normalized);

                    RaycastHit hitinfo;
                    if (Physics.Raycast(ray, out hitinfo))
                    {
                        GrabEvent gevt = new GrabEvent(hitinfo.transform.gameObject, indexTip.transform);
                        fireEvent(gevt);
                    }
                    return true;
                }
            }
            else 
            {
                Debug.Log("No Grab. Height varies too much");
            }
            return false;
        }

        bool checkForSelectionGesture() 
        {
            Vector3 thumb = thumbTip.transform.localPosition;
            Vector3 index = indexTip.transform.localPosition;
            Vector3 middle = middleTip.transform.localPosition;

            if (Mathf.Abs(Mathf.Abs(middle.y) - Mathf.Abs(index.y)) <= 0.1)
            {
                //Index muss weiter vorne sein
                if ((index.z - thumb.z) >= 0.05f)
                {
                    //Daumen muss weiter links gestreckt sein, also muss sein X kleiner sein
                    if (Mathf.Abs(Mathf.Abs(index.x) - Mathf.Abs(thumb.x)) <= 0.04f)
                    {
                        //Beide Finger müssen auf einer Höhenebene sein
                        if (Mathf.Abs(Mathf.Abs(index.y) - Mathf.Abs(thumb.y)) <= 0.15)
                        {
                            Debug.Log("=========== SELECTING ==========");
                            Ray ray = new Ray(indexTip.transform.position, (indexTip.transform.position - palm.transform.position).normalized);

                            RaycastHit hitinfo;
                            if (Physics.Raycast(ray, out hitinfo))
                            {
                                HoverEvent hevt = new HoverEvent(hitinfo.transform.gameObject, hitinfo.point);
                                fireEvent(hevt);
                            }
                            return true;
                        }
                        else
                        {
                            //Debug.Log("Height difference! I: " + index.y + " T: " + thumb.y);
                        }
                    }
                    else
                    {
                        //Debug.Log("LEFTRIGHT Not Fitting! I: " + index.x + " T: " + thumb.x);
                    }
                }
                else
                {
                    //Debug.Log("Index not close enough on Z! I: " + index.z + " T: " + thumb.z);
                }
            }
            else
            {
                //Debug.Log("MiddleFinger and Index not close enough on Y-Axis! INDEX Y : " + index.y + " MIDDLE Y " + middleTip.transform.localPosition.y);
            }
            return false;
        }


        void showPointingRay() 
        {
            if (myLineRay == null) 
            {
                myLineRay = this.GetComponent<LineRenderer>();
            }
            //myLineRay.SetVertexCount(2);
			myLineRay.positionCount = 2;
            myLineRay.SetPosition(0, indexTip.transform.position);
            Ray ray = new Ray(indexTip.transform.position, (indexTip.transform.position - palm.transform.position).normalized);
            myLineRay.SetPosition(1, ray.GetPoint(rayLength));
            myLineRay.widthMultiplier = rayWidth;
        }
    }
}
