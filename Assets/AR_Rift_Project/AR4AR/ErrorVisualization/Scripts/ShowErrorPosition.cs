using UnityEngine;
using System.Collections;
using FAR;
using System.Collections.Generic;

[RequireComponent (typeof (ErrorPositionSource))]
public class ShowErrorPosition : MonoBehaviour, Action {

	public float ErrorScale = 100;
	public GameObject ErrorObjectPrefab;

	protected ErrorPositionSource m_localErrorSource;
	protected GameObject m_errorVisObject = null;

    List<System.Type> m_ReceivableEvents = new List<System.Type>(new System.Type[] { typeof(ObjectStateEvent)});

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
	void Start () {
		m_localErrorSource = GetComponent<ErrorPositionSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(m_errorVisObject == null) 
			return;

		Measurement<ErrorVector3D> localError = m_localErrorSource.getLastData();
		if(localError == null)
			return;

		double[,] covar = localError.data().covariance;
		float x = (float) covar[0,0];
		float y = (float) covar[1,1];
		float z = (float) covar[2,2];

		x = Mathf.Sqrt(x)*ErrorScale;
		y = Mathf.Sqrt(y)*ErrorScale;
		z = Mathf.Sqrt(z)*ErrorScale;


		m_errorVisObject.transform.localScale = new Vector3(x,y,z);

	}

	public void doEvent(InteractionEvent evt){
		if(evt is ObjectStateEvent){
			ObjectStateEvent oevt = (ObjectStateEvent) evt;

			switch(oevt.objectState) {
			case ObjectState.Show:{
				m_errorVisObject = (GameObject) Instantiate(ErrorObjectPrefab);
				m_errorVisObject.transform.parent = transform;
				m_errorVisObject.transform.localRotation = Quaternion.identity;
				m_errorVisObject.transform.localPosition = Vector3.zero;

				break;
			}
			case ObjectState.Hide:{
					DestroyObject(m_errorVisObject);
				break;
			}
			}

		}
	}
}
