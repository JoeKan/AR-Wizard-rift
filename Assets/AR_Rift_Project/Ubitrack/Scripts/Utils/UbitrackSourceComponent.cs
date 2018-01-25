using UnityEngine;
using System.Collections;

namespace FAR {

public class UbitrackSourceComponent<T> : UbiTrackComponent {

    public int TimeoutInMilliSeconds = 40;

	public bool UseTriggerToPull = false;

	public UbiTrackComponent[] TriggerToPull;

	protected Measurement<T> m_lastData = null;
	
	public Measurement<T> Data
	{
		get { return m_lastData; }	
	}



	protected void triggerPull(ulong timestamp) {
			foreach (UbiTrackComponent item in TriggerToPull) {
				item.pullNow(timestamp);
			}
		}
}
}