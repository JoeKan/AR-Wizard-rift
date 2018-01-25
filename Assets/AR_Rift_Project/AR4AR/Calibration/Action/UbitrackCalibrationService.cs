using UnityEngine;
using System.Collections;
using FAR;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class UbitrackCalibrationService : InteractionMethod, FAR.Action {
	public TextAsset DFG4Service;
	public string ServerURI = "http://localhost:10000";
	public string ServiceURI = "/ubitrack/xmlCheet";
	public string ServiceInstanceURI = "/ubitrack/instances/0";
	public string MimeType = "text/xml";


	protected bool m_isStartingJob = false;
	public bool ServiceRunning = false;

	private HTTPClientHelper m_httpJob = null;


    List<System.Type> m_ReceivableEvents = new List<System.Type>(new System.Type[] { typeof(CalibrationEvent)});

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


	public virtual void doEvent(InteractionEvent evt) {
		if(evt is CalibrationEvent) {
			CalibrationEvent cevt = (CalibrationEvent) evt;
			switch(cevt.calibrationType){
			case CalibrationEventType.Start: {
				Debug.Log("signal to start service");
				if(ServiceRunning) {
					Debug.Log("severice is already running");
					return;
				}
				startService();
				break;
			}
			case CalibrationEventType.Stop: {
				Debug.Log("signal to stop service");
				if(!ServiceRunning) {
					Debug.Log("severice is not running");
					return;
				}
				stopService();
				break;
			}
			case CalibrationEventType.Restart: {

				break;
			}

			}
		}
	}

	public virtual void ServiceStarted() {
	}

	public virtual void ServiceStopped() {
	}

	public override void Update() {
		if (m_httpJob != null)
		{
			if (m_httpJob.Update())
			{


				Debug.Log("Status: " + m_httpJob.Status);
				if(m_isStartingJob){
					ServiceInstanceURI = m_httpJob.getResponseHeader()["Location"];
					Debug.Log("response: "+ServiceInstanceURI);
					ServiceRunning = true;
					ServiceStarted();

				} else {
					ServiceRunning = false;
					ServiceStopped();
				}
				 
				m_httpJob = null;
			}
		}
			
	}


	public virtual void startService() {
		if(m_httpJob != null) {
			Debug.LogWarning("Http Job already running, wait for it to finish before starting the next");
			return;
		}
		m_isStartingJob = true;
		m_httpJob = new HTTPClientHelper(ServerURI + ServiceURI, "PUT", MimeType, DFG4Service.text);						
		m_httpJob.Start();



	}

	public virtual void stopService() {
		if(m_httpJob != null) {
			Debug.LogWarning("Http Job already running, wait for it to finish before starting the next");
			return;
		}
		m_isStartingJob = false;
		m_httpJob = new HTTPClientHelper(ServerURI + ServiceInstanceURI, "DELETE");
		m_httpJob.Start();

	}
}
