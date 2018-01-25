using UnityEngine;
using System.Collections;
using FAR;
using System.Collections.Generic;

public class HandEyeGuide : MonoBehaviour, FAR.Action
{

    public float minDistance = 0.1f;
    public Material AOMaterial;
    public Transform Arrow;
    public Transform Root;
    public Transform LookRoot;
    public Vector3[] CalibRotations;
    public float[] CalibDistance;

    public Color farColor = Color.red;
    public Color nearColor = Color.green;

   
    bool nextPosition = false;
    int currentPosition = 0;

    List<System.Type> m_ReceivableEvents = new List<System.Type>(new System.Type[] { typeof(SingleKeyboardEvent) });

    public List<System.Type> ReceivableEvents
    {
        get
        {
            return m_ReceivableEvents;
        }
        set
        {
            m_ReceivableEvents = value;
        }
    }

    // Use this for initialization
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       

       // AOMaterial.color = Color.Lerp(nearColor, farColor, localScale);

        if (nextPosition)
        {
            nextPosition = false;
            //Ray ray = Camera.main.ViewportPointToRay(Random.insideUnitSphere);
            //float newdistance = Random.Range(0.2f, 0.5f);
            //targetPosition = ray.GetPoint(newdistance);
            //Debug.Log("taget pos:"+targetPosition);
            currentPosition++;
            if (currentPosition >= CalibRotations.Length)
                currentPosition = 0;

           

        }

        //Arrow.localScale = new Vector3(1,1, localScale);

        //Arrow.LookAt(LookRoot.TransformPoint(targetPosition));

        Arrow.localRotation = Quaternion.Euler(CalibRotations[currentPosition]);

    }

    public void doEvent(InteractionEvent evt)
    {
        if (evt is SingleKeyboardEvent)
        {
            SingleKeyboardEvent k_evt = (SingleKeyboardEvent)evt;
            if (k_evt.key == 32)
            {
                nextPosition = true;
            }




        }
    }
}
