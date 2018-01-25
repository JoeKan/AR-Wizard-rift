using UnityEngine;
using System.Collections;
using System;
using FAR;

public class Send2DPosition : MonoBehaviour {
    private HTTPClientHelper httpClient;


    public void send2DData(Vector2 position2D, string URL)
    {
        string rdfTemplate =    "@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> . \n" +
                                "@prefix core: <http://vocab.arvida.de/2015/06/core/vocab#> .\n" +
                                "@prefix m: <http://vocab.arvida.de/2015/06/maths/vocab#> .\n" +
                                "@prefix opencv: <http://vocab.arvida.de/2015/06/opencv/vocab#> .\n" +
                                "@prefix service: <http://vocab.arvida.de/2015/06/service/vocab#> .\n" +
                                "@prefix spatial: <http://vocab.arvida.de/2015/06/spatial/vocab#> .\n" +
                                "@prefix streaming: <http://vocab.arvida.de/2015/06/streaming/vocab#> .\n" +
                                "@prefix tracking: <http://vocab.arvida.de/2015/06/tracking/vocab#> .\n" +
                                "@prefix ubitrack: <http://vocab.arvida.de/2015/06/ubitrack/vocab#> .\n" +
                                "@prefix vom: <http://vocab.arvida.de/2015/06/vom/vocab#> .\n" +
                                "@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> .\n" +
                                "@prefix xsd: <http://www.w3.org/2001/XMLSchema#> .\n" +
                                "@prefix owl: <http://www.w3.org/2002/07/owl#> .\n" +
                                "@prefix ldp: <http://www.w3.org/ns/ldp#> .\n" +

                                "<>\n" +
                                "    spatial:timestamp [\n" +
                                "        vom:quantityValue [\n" +
                                "            vom:numericalValue \"{0}\"^^xsd:unsignedLong ;\n" +
                                "            vom:unit vom:NanoSecond ;\n" +
                                "            a vom:QuantityValue\n" +
                                "        ] ;\n" +
                                "        a tracking:UnixTimestamp\n" +
                                "    ] ;\n" +
                                "    spatial:translation [\n" +
                                "        vom:quantityValue [\n" +
                                "            m:x {1} ;\n" +
                                "            m:y {2} ;\n" +
                                "            vom:unit vom:meter ;\n" +
                                "            a m:Vector2D\n" +
                                "        ] ;\n" +
                                "        a spatial:Translation2D\n" +
                                "    ] ;\n" +
                                "    streaming:subscription <Position2DSink/subscriptions> ;\n" +
                                "    a core:Output, core:Pull .\n";

        String data = string.Format(rdfTemplate, UbiMeasurementUtils.getUbitrackTimeStamp(), position2D.x, position2D.y);

        httpClient = new HTTPClientHelper(URL, "PUT", "text/turtle", data);
        httpClient.Start();
    }
	
	// Update is called once per frame
	void Update () {
  
        if (httpClient != null)
        {
            if (httpClient.IsDone)
            {
                Debug.Log("done");
                Debug.Log(httpClient.GetResponseBody());
                httpClient = null;
            }
        }
	}
}
