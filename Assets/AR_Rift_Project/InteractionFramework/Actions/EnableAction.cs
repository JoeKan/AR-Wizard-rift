using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FAR {

	public class EnableAction : MonoBehaviour, Action  {

		public void doEvent(InteractionEvent evt)
		{
			if(evt is ObjectStateEvent) {
				ObjectStateEvent enevt = (ObjectStateEvent) evt;
				Debug.Log("got event:" +enevt.objectState);
				switch (enevt.objectState) {
				default:
				case ObjectState.Show:
					gameObject.SetActive(true);
				break;

				case ObjectState.Hide:
					gameObject.SetActive(false);
				break;

				}

			}
		}
}

}
