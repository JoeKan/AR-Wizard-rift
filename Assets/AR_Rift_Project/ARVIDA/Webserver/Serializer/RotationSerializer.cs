using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDS.RDF;


class RotationSerializer : RdfSerializer
{
	
	public static String SerializeToString(Quaternion rotation, RdfSerializerContext serializerContext, RdfSerializer.NotationType notation)
	{
		var graph = CreateArvidaGraph(serializerContext);
		
		SerializeToNewNode(rotation, serializerContext, graph);
		
		return SerializeGraphToString(graph,notation);
	}
	
	public static void SerializeToNode(Quaternion rotation, INode node)
	{
		IGraph graph = node.Graph;
		
		AddRdfType(node, "spatial:Rotation3D");
		
		IBlankNode quaternionNode = graph.CreateBlankNode();
		graph.Assert(node, graph.CreateUriNode("vom:quantityValue"), quaternionNode);
		QuaternionSerializer.SerializeToNode(rotation,quaternionNode);
	}
	
	public static IUriNode SerializeToNewNode(Quaternion rotation, RdfSerializerContext serializerContext, IGraph graph)
	{
		IUriNode baseNode = graph.CreateUriNode(serializerContext.RelativeUri);
		
		SerializeToNode(rotation, baseNode);
		
		return baseNode;
	}

	public static void DeserializeFromNode(INode node, out Quaternion rotation)
	{
		AssertRdfType (node, "spatial:Rotation3D");

		INode valueNode = GetObjectByPredicateName(node,"vom:quantityValue");
		
		QuaternionSerializer.DeserializeFromNode(valueNode,out rotation);
	}


}
