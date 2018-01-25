using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;


public class RDFTermTest : MonoBehaviour {

    public TextAsset rdfText=null;


    void doTest () {
        ARVIDAGraph graph = new ARVIDAGraph("http://localhost:10000/");

        if (rdfText != null)
            graph.readRDFText(rdfText.text, "http://localhost:10000/");


        RDFTerm root1 = graph.getUriTerm(new Uri("http://localhost:10000/sr1"));

        root1.addObject("rdf:type").setURI("spatial:SpatialRelationship");
        root1["spatial:translation"].makeBlankNode()["vom:qualityValue"].makeBlankNode()["maths:x"].setLiteralValue<double>(10);

        string turtle = graph.serializeToTurtle();
        Debug.Log(turtle);

        Debug.Log("All SpatialRelations -------------------------------------");
        List<RDFTerm> srs = graph.getSubjects("rdf:type", "maths:RightHandedCartesianCoordinateSystem3D");
        foreach (var item in srs)
        {
            Debug.Log(item.ToString());
            Debug.Log(item["maths:axesUnit"].ToString());
        }
        Debug.Log("-------------------------------------");
        IRDF2UTQL converter = new RDF2UTQL_v1();


        //RDFTerm cs1;
        //RDFTerm cs2;

        //cs1.addObject("owl:sameAs").setURI(cs2.getURI());



        string utql = converter.rdf2utql(graph.getGraph());
        Debug.Log(utql);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            doTest();
	}
}
