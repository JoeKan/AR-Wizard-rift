using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDS.RDF;


public class TransformSerializer : RdfSerializer
{
	const string spatialRelationRdf = "scenegraph:DefinedSpatialRelationship";
	const string translationPropertyRdf = "spatial:translation";
	const string rotationPropertyRdf = "spatial:rotation";
	const string scalingPropertyRdf  = "spatial:scaling";



    public static String SerializeToString(Transform transform, RdfSerializerContext serializerContext, RdfSerializer.NotationType notation)
    {
        var graph = CreateArvidaGraph(serializerContext);

        SerializeToNewNode(transform, serializerContext, graph);

		return SerializeGraphToString(graph, notation);
    }



	public static void SerializeToNode(Transform transform, IUriNode spatialRelationNode, RdfSerializerContext serializerContext)
    {
		if (transform.parent == null)
			return;

        IGraph graph = spatialRelationNode.Graph;
		AddRdfType(spatialRelationNode,spatialRelationRdf);


		IBlankNode translationNode = graph.CreateBlankNode();
		IBlankNode rotationNode = graph.CreateBlankNode();
		IBlankNode scalingNode = graph.CreateBlankNode();
		graph.Assert(spatialRelationNode, graph.CreateUriNode(translationPropertyRdf), translationNode);
		graph.Assert(spatialRelationNode, graph.CreateUriNode(rotationPropertyRdf), rotationNode);
		graph.Assert(spatialRelationNode, graph.CreateUriNode(scalingPropertyRdf), scalingNode);

		TranslationSerializer.SerializeToNode(transform.localPosition,translationNode);
		RotationSerializer.SerializeToNode(transform.localRotation,rotationNode);
		ScalingSerializer.SerializeToNode(transform.localScale,scalingNode);

		SerializeCoordinateSystemReferences (transform, spatialRelationNode, serializerContext); 

    }

	static void SerializeCoordinateSystemReferences (Transform transform, IUriNode spatialRelationNode, RdfSerializerContext serializerContext)
	{
		IGraph graph = spatialRelationNode.Graph;
		GameObject parentNode = transform.parent.gameObject;
		INode parentCoordUriNode = CreateCoordinateSystemNode (parentNode, serializerContext, graph);
		INode coordUriNode = CreateCoordinateSystemNode (transform.gameObject, serializerContext, graph);
		graph.Assert (spatialRelationNode, graph.CreateUriNode (SourceCoordPropertyRdf), parentCoordUriNode);
		graph.Assert (spatialRelationNode, graph.CreateUriNode (TargetCoordPropertyRdf), coordUriNode);
	}

    public static IUriNode SerializeToNewNode(Transform transform, RdfSerializerContext serializerContext, IGraph graph)
    {
        IUriNode baseNode = graph.CreateUriNode(serializerContext.RelativeUri);

        SerializeToNode(transform,baseNode,serializerContext);

        return baseNode;
    }

	public static void DeserializeFromNode(INode node, Transform value)
	{
		AssertRdfType (node, spatialRelationRdf);


		INode translationNode = GetObjectByPredicateName(node,translationPropertyRdf);
		INode rotationNode = GetObjectByPredicateName(node,rotationPropertyRdf);
		INode scalingNode = GetObjectByPredicateName(node,scalingPropertyRdf);
		
	
		Vector3 translation = new Vector3();
		Vector3 scaling = new Vector3();
		Quaternion rotation = new Quaternion();
		TranslationSerializer.DeserializeFromNode(translationNode,out translation);
		RotationSerializer.DeserializeFromNode(rotationNode,out rotation);
		ScalingSerializer.DeserializeFromNode(scalingNode, out scaling);
		 
		value.localPosition = translation;
		value.localScale = scaling;
		value.localRotation = rotation;
	}

}

