using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDS.RDF;


class ScalingSerializer : RdfSerializer
{
	public static String SerializeToString(Vector3 translation, RdfSerializerContext serializerContext, RdfSerializer.NotationType notation)
	{
		var graph = CreateArvidaGraph(serializerContext);
		
		SerializeToNewNode(translation, serializerContext, graph);
		
		return SerializeGraphToString(graph,notation);
	}
	
	public static void SerializeToNode(Vector3 translation, INode node)
	{
		IGraph graph = node.Graph;
		
		AddRdfType(node, "spatial:NonUniformScaling3D");
		
		IBlankNode valueNode = graph.CreateBlankNode();
		graph.Assert(node, graph.CreateUriNode("vom:quantityValue"), valueNode);
		Vector3Serializer.SerializeToNode(translation,valueNode);
	}
	
	public static IUriNode SerializeToNewNode(Vector3 translation, RdfSerializerContext serializerContext, IGraph graph)
	{
		IUriNode baseNode = graph.CreateUriNode(serializerContext.RelativeUri);
		
		SerializeToNode(translation, baseNode);
		
		return baseNode;
	}

	public static void DeserializeFromNode(INode node, out Vector3 value)
	{
		AssertRdfType (node, "spatial:NonUniformScaling3D");
		INode valueNode = GetObjectByPredicateName(node,"vom:quantityValue");
		
		Vector3Serializer.DeserializeFromNode(valueNode,out value);
	}
}
