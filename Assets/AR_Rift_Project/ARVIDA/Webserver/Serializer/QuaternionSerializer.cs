using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDS.RDF;
using VDS.RDF.Writing;


public class QuaternionSerializer : RdfSerializer
{
	const string quaternionRdfType =  "maths:Quaternion";
	const string vector4RdfType =  "maths:Vector4D";
	const string xRdf = "maths:x";
	const string yRdf = "maths:y";
	const string zRdf = "maths:z";
	const string wRdf = "maths:w";

	public static String SerializeToString(Quaternion quaternion, RdfSerializerContext serializerContext, RdfSerializer.NotationType notation)
    {
        var graph = CreateArvidaGraph(serializerContext);

		SerializeToNewNode(quaternion, serializerContext,graph);

		return SerializeGraphToString(graph,notation);
    }

	public static void SerializeToNode(Quaternion quaternion, INode node)
	{
        IGraph graph = node.Graph;
		
		AddRdfType(node,quaternionRdfType);
		AddRdfType(node,vector4RdfType);

		graph.Assert(node, graph.CreateUriNode(xRdf), quaternion.x.ToLiteral(graph));
		graph.Assert(node, graph.CreateUriNode(yRdf), quaternion.y.ToLiteral(graph));
		graph.Assert(node, graph.CreateUriNode(zRdf), quaternion.z.ToLiteral(graph));
		graph.Assert(node, graph.CreateUriNode(wRdf), quaternion.w.ToLiteral(graph));
	}

	public static IUriNode SerializeToNewNode(Quaternion quaternion, RdfSerializerContext serializerContext, IGraph graph)
    {
		Uri tmp = new Uri(serializerContext.BaseUri, serializerContext.RelativeUri);
		IUriNode baseNode = graph.CreateUriNode(tmp);
		
		SerializeToNode(quaternion, baseNode);
		
		return baseNode;
    }

	public static void DeserializeFromNode(INode node, out Quaternion value)
	{
		AssertRdfType(node,quaternionRdfType);
		AssertRdfType(node,vector4RdfType);
		
		value.x = GetLiteralByPredicateName<float>(node,xRdf);
		value.y = GetLiteralByPredicateName<float>(node,yRdf);
		value.z = GetLiteralByPredicateName<float>(node,zRdf);
		value.w = GetLiteralByPredicateName<float>(node,wRdf);
	}
}
