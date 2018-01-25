using UnityEngine;
using System.Collections;

public class AO_UbitrackService : DefaultUbiCalibService {

	public Position3DListSink ReferencePositions;



	public override void ServiceStarted() {
		base.ServiceStarted();

		ReferencePositions.sendData();

	}
}
