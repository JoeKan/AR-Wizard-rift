using VDS.RDF;
using VDS.RDF.Parsing;
using System;
using System.Globalization;
using UnityEngine;

public class ArvidaUnitConverter : MonoBehaviour
{
	static IGraph arvidaVomGraph;
	public string VOMVocabUri = "http://vocab.arvida.de/2014/03/vom/vocab.ttl";

	public static float ConvertToMeter(float value, Uri unitUri)
	{
		INode unitNode = arvidaVomGraph.CreateUriNode(unitUri);
		

		float offset = RdfSerializer.GetLiteralByPredicateName<float>(unitNode,":conversionOffset");
		float multiplier = RdfSerializer.GetLiteralByPredicateName<float>(unitNode,":conversionMultiplier");


		return offset + (value * multiplier);
	}

	void Start()
	{
		if (arvidaVomGraph == null)
		{
			arvidaVomGraph = new Graph();
			UriLoader.Load(arvidaVomGraph, new Uri(VOMVocabUri));
		}
	}
}


