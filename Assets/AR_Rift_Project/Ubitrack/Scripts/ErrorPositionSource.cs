using UnityEngine;
using System;
using System.Collections;

namespace FAR {

	public class ErrorPositionSource : UbitrackSourceComponent<ErrorVector3D>{
    public UbitrackEventType ubitrackEvent = UbitrackEventType.Push;
    public UbitrackRelativeToUnity relative = UbitrackRelativeToUnity.World;

    protected SimpleApplicationPullSinkErrorPosition3D m_posePull = null;
    protected SimpleErrorPosition3D m_simplePose = null;

    protected UnityErrorPositionReceiver m_poseReceiver = null;
    protected Measurement<ErrorVector3D> m_pose;
	protected Measurement<ErrorVector3D> m_lastpose;

	public Measurement<ErrorVector3D> getLastData() {
			return m_lastpose;
	}

    // Use this for initialization
    public override void utInit(SimpleFacade simpleFacade)
    {
        base.utInit(simpleFacade);



        switch (ubitrackEvent)
        {
            case UbitrackEventType.Pull:
                {
                    m_posePull = simpleFacade.getPullSinkErrorPosition3D(patternID);
                    m_simplePose = new SimpleErrorPosition3D();
                    if (m_posePull == null)
                    {
                        throw new Exception("SimpleApplicationPullSinkErrorPose not found for poseID:" + patternID);
                    }
                    break;
                }
            case UbitrackEventType.Push:
                {
					m_poseReceiver = new UnityErrorPositionReceiver();


                    if (!simpleFacade.set3DErrorPositionCallback(patternID, m_poseReceiver))
                    {
                        throw new Exception("SimpleErrorPoseReceiver could not be set for poseID:" + patternID);
                    }
                    break;
                }
            default:
                break;
        }
    }

    void FixedUpdate()
    {		
        m_pose = null;

        switch (ubitrackEvent)
        {
            case UbitrackEventType.Pull:
                {
                    ulong lastTimestamp = UbiMeasurementUtils.getUbitrackTimeStamp();

                    if (m_posePull.getErrorPosition3D(m_simplePose, lastTimestamp))
                    {
                        m_pose = UbiMeasurementUtils.ubitrackToUnity(m_simplePose);
                    }
                    break;
                }
            case UbitrackEventType.Push:
                {
                    m_pose = m_poseReceiver.getData();
                    break;
                }
            default:
                break;
        }

        if (m_pose != null)
        {
			if(relative == UbitrackRelativeToUnity.Local) {
					gameObject.transform.localPosition = m_pose.data().position;
				} else {
					gameObject.transform.position = m_pose.data().position;
				}
			m_lastpose = m_pose;
        }

    }

}

}