using UnityEngine;
using System.Collections;

namespace FAR {

	public class UnityErrorPositionReceiver : SimpleErrorPosition3DReceiver
	{
		protected Measurement<ErrorVector3D> m_data = null;
		
		private System.Object thisLock = new System.Object();
		
		public Measurement<ErrorVector3D> getData()
		{
			lock (thisLock)
			{
				Measurement<ErrorVector3D> tmp = m_data;
				m_data = null;
				return tmp;
			}
			
		}
		
		public override void receiveErrorPosition3D (SimpleErrorPosition3D position3d)
		{
			lock (thisLock)
			{
				m_data = UbiMeasurementUtils.ubitrackToUnity(position3d);
			}
		}
	}

}

