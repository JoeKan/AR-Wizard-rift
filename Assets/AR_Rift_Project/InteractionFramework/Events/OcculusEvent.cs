using UnityEngine;
using System.Collections;

namespace FAR{
	
	public class OcculusEvent : InteractionEvent {
		
		
		Vector3 m_CurrentOCPos;
		Quaternion m_CurrentOCRot;
		Vector3 m_OCAngularVelocity;
		Vector3 m_OCAcceleration;
		
		bool m_TrackerEnabled;
		bool m_TrackingHMD;
		
		public Vector3 CurrentOCPos
		{
			get { return m_CurrentOCPos; }
		}
		
		public Quaternion CurrentOCRot
		{
			get { return m_CurrentOCRot; }
		}
		
		public Vector3 OCAngularVelocity
		{
			get { return m_OCAngularVelocity; }
		}
		
		public Vector3 OCAcceleration
		{
			get { return m_OCAcceleration; }
		}
		
		public bool TrackerEnabled
		{
			get { return m_TrackerEnabled; }
		}
		
		public bool TrackingHMD
		{
			get { return m_TrackingHMD; }
		}

        public OcculusEvent(Vector3 CurrentOCPos, Quaternion CurrentOCRot, Vector3 OCAngularVelocity, Vector3 OCAcceleration, bool TrackerEnabled, bool TrackingHMD, InteractionMethod sender = null, InteractionEvent base_Evt = null)
            : base(sender, base_Evt)
		{
			this.m_CurrentOCPos = CurrentOCPos;
			this.m_CurrentOCRot = CurrentOCRot;
			this.m_OCAngularVelocity = OCAngularVelocity;
			this.m_OCAcceleration = OCAcceleration;
			this.m_TrackerEnabled = TrackerEnabled;
			this.m_TrackingHMD = TrackingHMD;
		}
		
		
	}
	
}
