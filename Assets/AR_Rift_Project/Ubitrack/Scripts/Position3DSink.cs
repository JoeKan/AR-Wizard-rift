using UnityEngine;
using System;
using System.Collections;

namespace FAR
{

    public class Position3DSink : UbiTrackComponent
    {
        public UbitrackEventType ubitrackEvent = UbitrackEventType.Push;
        public UbitrackRelativeToUnity relative = UbitrackRelativeToUnity.World;


        public bool once = true;

        public Vector3 sendPos;

        public bool sending = true;

        protected SimplePosition3DReceiver m_poseReciever = null;
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

                        m_poseReciever = simpleFacade.getPushSourcePosition3D(patternID);

                        if (m_poseReciever == null)
                        {
                            throw new Exception("SimplePosition3DReceiver could not be set for POS3D ID:" + patternID);
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
                            SimplePosition3D simplePos3D = new SimplePosition3D();
                            Vector3 output = Vector3.zero;
                            UbiMeasurementUtils.coordsysemChange(sendPos, ref output);
                            simplePos3D.x = output.x;
                            simplePos3D.y = output.y;
                            simplePos3D.z = output.z;
                            simplePos3D.timestamp = UbiMeasurementUtils.getUbitrackTimeStamp();
                            m_poseReciever.receivePosition3D(simplePos3D);
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
            SimplePosition3D simplePos3D = new SimplePosition3D();
            Vector3 output = Vector3.zero;
            UbiMeasurementUtils.coordsysemChange(sendPos, ref output);
            simplePos3D.x = output.x;
            simplePos3D.y = output.y;
            simplePos3D.z = output.z;
            simplePos3D.timestamp = timestamp;
            m_poseReciever.receivePosition3D(simplePos3D);
            if (once) this.enabled = false;
        }


    }

}
