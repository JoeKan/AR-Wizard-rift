using UnityEngine;
using System.Collections;
using FAR;

public class Position3DListSink : UbiTrackComponent {

	public UbitrackEventType ubitrackEvent = UbitrackEventType.Push;
	
	public Transform[] ListPoints;

	protected SimplePositionList3DReceiver m_listReceiver = null;

	// Use this for initialization    
	public override void utInit(SimpleFacade simpleFacade)
	{
		base.utInit(simpleFacade);
		
		switch (ubitrackEvent)
		{
		case UbitrackEventType.Pull:
		{
		
			break;
		}
		case UbitrackEventType.Push:
		{
			m_listReceiver = simpleFacade.getPushSourcePositionList3D(patternID);
				
			break;
		}
		default:
			break;
		}
	}
	

	
	void FixedUpdate()
	{
		switch (ubitrackEvent)
		{
		case UbitrackEventType.Pull:
		{
		
			break;
		}
		case UbitrackEventType.Push:
		{

			break;
		}
		default:
			break;
		}
	}

	public void sendData() {
		SimplePositionList3D ubi3DList = new SimplePositionList3D();

		ubi3DList.timestamp = UbiMeasurementUtils.getUbitrackTimeStamp();

		SimplePosition3DValue_Vector list = new SimplePosition3DValue_Vector(ListPoints.Length);

		for(int i=0;i<ListPoints.Length;i++) {

			Vector3 ubiPos = new Vector3();
			UbiMeasurementUtils.coordsysemChange(ListPoints[i].position, ref ubiPos);

			SimplePosition3DValue value = new SimplePosition3DValue();
			value.x = ubiPos.x / UbiMeasurementUtils.UbiToUnityScaleFactor;
			value.y = ubiPos.y / UbiMeasurementUtils.UbiToUnityScaleFactor;
			value.z = ubiPos.z / UbiMeasurementUtils.UbiToUnityScaleFactor;
			list.Add(value);

		}

		ubi3DList.values = list;


		m_listReceiver.receivePositionList3D(ubi3DList);

	}
}
