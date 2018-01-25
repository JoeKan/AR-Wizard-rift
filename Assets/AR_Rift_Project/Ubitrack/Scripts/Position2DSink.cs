using UnityEngine;
using System;
using System.Collections;

namespace FAR
{

    public class Position2DSink : UbiTrackComponent
    {
        public UbitrackEventType ubitrackEvent = UbitrackEventType.Push;
        public UbitrackRelativeToUnity relative = UbitrackRelativeToUnity.World;


        public bool once = true;

        public Vector2 sendPos2D;

        public bool sending = false;

        protected SimplePosition2DReceiver m_poseReciever = null;
        // Use this for initialization
        public override void utInit(SimpleFacade simpleFacade)
        {
            base.utInit(simpleFacade);



            switch (ubitrackEvent)
            {
                case UbitrackEventType.Pull:
                    {
                        throw new Exception("Pull not supported yet");
                    }
                case UbitrackEventType.Push:
                    {

                        m_poseReciever = simpleFacade.getPushSourcePosition2D(patternID);

                        if (m_poseReciever == null)
                        {
                            throw new Exception("SimplePosition2DReceiver could not be set for POS2D ID:" + patternID);
                        } 
                        
                        


                        break;
                    }
                default:
                    break;
            }
        }

        void Update()
        {
            if (sending)
            {
                switch (ubitrackEvent)
                {
                    case UbitrackEventType.Push:
                        {
                            SimplePosition2D simplePos2D = new SimplePosition2D();
                            simplePos2D.x = sendPos2D.x;
                            simplePos2D.y = sendPos2D.y;
                            simplePos2D.timestamp = UbiMeasurementUtils.getUbitrackTimeStamp();
                            m_poseReciever.receivePosition2D(simplePos2D);
                            if (once) this.enabled = false;
                            break;
                        }
                    case UbitrackEventType.Pull:
                    default:
                        break;

                }
            }
        }

        public void send(ulong timestamp) 
        {
            SimplePosition2D simplePos2D = new SimplePosition2D();
            simplePos2D.x = sendPos2D.x;
            simplePos2D.y = sendPos2D.y;
            simplePos2D.timestamp = timestamp;
            m_poseReciever.receivePosition2D(simplePos2D);
            if (once) this.enabled = false;
        }


    }

}
