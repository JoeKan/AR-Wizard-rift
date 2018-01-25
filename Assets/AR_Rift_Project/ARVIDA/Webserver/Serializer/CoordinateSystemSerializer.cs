using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDS.RDF;


public class CoordinateSystemSerializer : RdfSerializer
{
	public static String SerializeToString(GameObject sceneNode, RdfSerializerContext serializerContext, RdfSerializer.NotationType notation)
	{
		IGraph graph = CreateArvidaGraph(serializerContext);
		
		SerializeToNewNode(sceneNode, serializerContext,graph);
		
		return SerializeGraphToString(graph,notation);
	}
	
	public static void SerializeToNode(GameObject sceneNode, RdfSerializerContext serializerContext, INode node)
	{
		IGraph graph = node.Graph;
		
		AddRdfType(node,"spatial:LeftHandedCartesianCoordinateSystem");
		
		graph.Assert(node, graph.CreateUriNode("spatial:axesUnit"), graph.CreateUriNode("vom:meter"));

		SerializeChildren (sceneNode, serializerContext, node);
		
		SerializeParent(sceneNode,serializerContext,node);

		IUriNode uriNode = CreateSceneUriNode(sceneNode,serializerContext,graph);
		graph.Assert(uriNode,graph.CreateUriNode("spatial:nodeFixedCoordinateSystem"),graph.CreateUriNode());

	}
	
	public static IUriNode SerializeToNewNode(GameObject sceneNode, RdfSerializerContext serializerContext, IGraph graph)
	{
		Uri tmp = new Uri(serializerContext.BaseUri, serializerContext.RelativeUri);
		IUriNode baseNode = graph.CreateUriNode(tmp);
		
		SerializeToNode(sceneNode, serializerContext,baseNode);
		
		return baseNode;
	}

	private static void SerializeParent(GameObject sceneNode, RdfSerializerContext serializerContext, INode node)
	{
		if (sceneNode.transform.parent == null)
			return;

		IGraph graph = node.Graph;
		
		
		INode transformNode = CreateTransformUriNode(sceneNode,serializerContext,graph);
		graph.Assert (transformNode, graph.CreateUriNode ("spatial:targetCoordinateSystem"),node);//CreateCoordinateSystemNode(sceneNode,serializerContext,graph));
		
	}

	private static void SerializeChildren (GameObject sceneNode, RdfSerializerContext serializerContext, INode node)
	{
		IGraph graph = node.Graph;
		for (int i = 0; i < sceneNode.transform.childCount; ++i) {
			GameObject childNode = sceneNode.transform.GetChild(i).gameObject;

			INode childTransformNode = CreateTransformUriNode(childNode,serializerContext,graph);
			graph.Assert (childTransformNode, graph.CreateUriNode ("spatial:sourceCoordinateSystem"),node);
		}
	}
}

