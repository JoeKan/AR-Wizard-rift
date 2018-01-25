using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDS.RDF;


public class Vector3Serializer : RdfSerializer
{
	const string rdfType =  "maths:Vector3D";
	const string xRdf = "maths:x";
	const string yRdf = "maths:y";
	const string zRdf = "maths:z";


	public static String SerializeToString(Vector3 vector, RdfSerializerContext serializerContext, RdfSerializer.NotationType notation)
    {
        var graph = CreateArvidaGraph(serializerContext);

        SerializeToNewNode(vector, serializerContext, graph);

        return SerializeGraphToString(graph,notation);
    }

    public static void SerializeToNode(Vector3 vector, INode node)
    {
        IGraph graph = node.Graph;

        AddRdfType(node, rdfType);

		graph.Assert(node, graph.CreateUriNode(xRdf), vector.x.ToLiteral(graph));
		graph.Assert(node, graph.CreateUriNode(yRdf), vector.y.ToLiteral(graph));
		graph.Assert(node, graph.CreateUriNode(zRdf), vector.z.ToLiteral(graph));
    }

    public static IUriNode SerializeToNewNode(Vector3 vector, RdfSerializerContext serializerContext, IGraph graph)
    {
        IUriNode baseNode = graph.CreateUriNode(serializerContext.RelativeUri);

		SerializeToNode(vector,baseNode);

        return baseNode;
    }


    
	public static void DeserializeFromNode(INode node, out Vector3 value)
	{
		AssertRdfType (node, rdfType);

		value.x = GetLiteralByPredicateName<float>(node,xRdf);
		value.y = GetLiteralByPredicateName<float>(node,yRdf);
		value.z = GetLiteralByPredicateName<float>(node,zRdf);
	}


}

