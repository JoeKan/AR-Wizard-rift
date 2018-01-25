using UnityEngine;
using System;
using System.Collections;

namespace FAR{

	//Vorher PoseSink jetzt auf UnityPerspektive geändert.
	public class PoseSource : UbitrackSourceComponent<Pose> {   
		public UbitrackEventType ubitrackEvent = UbitrackEventType.Push;
		public UbitrackRelativeToUnity relative = UbitrackRelativeToUnity.World;

		
	    
	    protected SimpleApplicationPullSinkPose m_posePull = null;
	    protected SimplePose m_simplePose = null;	
	
		protected UnityPoseReceiver m_poseReceiver = null;	
		protected Measurement<Pose> m_pose = null;		


		protected float m_meanPosVelocity = 0;

		
		// Use this for initialization    
	    public override void utInit(SimpleFacade simpleFacade)
	    {
	        base.utInit(simpleFacade);

            
	        		
			switch(ubitrackEvent)
			{
			case UbitrackEventType.Pull:{
	            m_posePull = simpleFacade.getSimplePullSinkPose(patternID);
	            m_simplePose = new SimplePose();	

				 	if (m_posePull == null)
				    {
	                    throw new Exception("SimpleApplicationPullSinkPose not found for poseID:" + patternID);
				    }
					break;
				}
			case UbitrackEventType.Push:{
	            m_poseReceiver = new UnityPoseReceiver();
            
	
	            if (!simpleFacade.setPoseCallback(patternID, m_poseReceiver))
					{
	                    throw new Exception("SimplePoseReceiver could not be set for poseID:" + patternID);
					}
	              
					break;
				}
			default:
			break;
			}    		
		}
		
	    void FixedUpdate()
	    {
            
			if(ubitrackEvent == UbitrackEventType.Pull && UseTriggerToPull)
				return;

            

	        m_pose = null;
		
			switch(ubitrackEvent)
			{
			case UbitrackEventType.Pull:{				
					ulong lastTimestamp =  UbiMeasurementUtils.getUbitrackTimeStamp();
					if(m_posePull.getPose(m_simplePose, lastTimestamp))
					{					
	                    m_pose = UbiMeasurementUtils.ubitrackToUnity(m_simplePose);    
					}	
					break;
				}
			case UbitrackEventType.Push:{
            
	            m_pose = m_poseReceiver.getData();
					break;
				}
			default:
			break;
			}

            


			//Debug.Log ("m_simplePose: " + m_simplePose); 
			processData();
	        	
	    } 

		public override void pullNow(ulong lastTimestamp) {
		
			if(m_posePull.getPose(m_simplePose, lastTimestamp))
			{					
				m_pose = UbiMeasurementUtils.ubitrackToUnity(m_simplePose);    
			}	
			processData();
		}
		
		protected void processData() {
			if (m_pose != null)
			{
				UbiUnityUtils.setGameObjectPose(relative, gameObject, m_pose.data());
				
				if(m_lastData != null) {
					Vector3 posDiff = m_pose.data().pos - m_lastData.data().pos;
					ulong timeDiff = m_pose.time() - m_lastData.time();
					
					float timeDiffSeconds = (float)timeDiff * 1E-9f;
					
					m_meanPosVelocity =  posDiff.magnitude / timeDiffSeconds;
				}
				
				m_lastData = m_pose;
			}
			
			
			if(m_lastData != null) {		
				ulong timeDiff = UbiMeasurementUtils.getUbitrackTimeStamp() - m_lastData.time();
				float timeDiffMilliSeconds = (float)timeDiff * 1E-6f;
				m_timeout = timeDiffMilliSeconds > TimeoutInMilliSeconds;
				
				
			}
		}



		public float Velocity
		{
			get { return m_meanPosVelocity; }	
		}


	}



}
