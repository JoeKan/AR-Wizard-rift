using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDS.RDF;


class TranslationSerializer : RdfSerializer
{
	const string rdfType =  "spatial:Translation3D";
	
	public static String SerializeToString(Vector3 translation, RdfSerializerContext serializerContext, RdfSerializer.NotationType notation)
	{
		var graph = CreateArvidaGraph(serializerContext);
		
		SerializeToNewNode(translation, serializerContext, graph);
		
		return SerializeGraphToString(graph,notation);
	}
	
	public static void SerializeToNode(Vector3 translation, INode node)
	{
		IGraph graph = node.Graph;
		
		AddRdfType(node, "spatial:Translation3D");
		
		IBlankNode valueNode = graph.CreateBlankNode();
		graph.Assert(node, graph.CreateUriNode("vom:quantityValue"), valueNode);
		graph.Assert(valueNode, graph.CreateUriNode("vom:unit"), graph.CreateUriNode("vom:Meter"));

		Vector3Serializer.SerializeToNode(translation,valueNode);
	}

	public static void DeserializeFromNode(INode node, out Vector3 value)
	{
		AssertRdfType (node, "spatial:Translation3D");

		INode valueNode = GetObjectByPredicateName(node,"vom:quantityValue");
		
		INode unitNode = GetObjectByPredicateName(valueNode,"vom:unit");
		IUriNode unitUriNode = (IUriNode) unitNode;
		
		
		Vector3Serializer.DeserializeFromNode(valueNode,out value);
		
		value.x = ArvidaUnitConverter.ConvertToMeter(value.x,unitUriNode.Uri);
		value.y = ArvidaUnitConverter.ConvertToMeter(value.y,unitUriNode.Uri);
		value.z = ArvidaUnitConverter.ConvertToMeter(value.z,unitUriNode.Uri);
	}
	
	public static IUriNode SerializeToNewNode(Vector3 translation, RdfSerializerContext serializerContext, IGraph graph)
	{ 
		IUriNode baseNode = graph.CreateUriNode(serializerContext.RelativeUri);
		
		SerializeToNode(translation, baseNode);
		
		return baseNode;
	}


}
