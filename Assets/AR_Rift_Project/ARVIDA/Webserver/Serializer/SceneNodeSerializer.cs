using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF;


public class SceneNodeSerializer : RdfSerializer
{
	const string rdfType = "scenegraph:SceneNode";
	const string childPropertyRdf = "scenegraph:childOf";
	const string parentPropertyRdf = "scenegraph:parentOf";
	const string componentEnabledPropertyRdf = "scenegraph:componentEnabled";

	public static String SerializeToString(GameObject sceneNode, RdfSerializerContext serializerContext, RdfSerializer.NotationType notation)
    {
        var graph = CreateArvidaGraph(serializerContext);

		SerializeToNewNode(sceneNode, serializerContext, graph);

		return SerializeGraphToString(graph,notation);
    }



	public static void SerializeToNode(GameObject sceneNode, RdfSerializerContext serializerContext, INode node)
    {
		AddRdfType(node, rdfType);

		AddRdfLabel (node, sceneNode.name);

		SerializeChildren (sceneNode, serializerContext, node);

		SerializeParent(sceneNode,serializerContext,node);

		AddCoordinateSystem(sceneNode, serializerContext,node);

		SerializeComponents(sceneNode,serializerContext,node);
	}

	public static void UpdateFromNode(GameObject sceneNode, RdfSerializerContext serializerContext, INode node)
	{
		AssertRdfType (node, rdfType);

		sceneNode.name = GetRdfLabel(node);

		DeserializeComponents(sceneNode,serializerContext,node);
		DeserializeParent(sceneNode,serializerContext,node);
		DeserializeChildren(sceneNode,serializerContext,node);
	}

	public static IUriNode SerializeToNewNode(GameObject sceneNode, RdfSerializerContext serializerContext, IGraph graph)
    {
		IUriNode baseNode = graph.CreateUriNode(serializerContext.RelativeUri);

		SerializeToNode(sceneNode,serializerContext,baseNode);

        return baseNode;
    }
    
	private static void SerializeChildren (GameObject sceneNode, RdfSerializerContext serializerContext, INode node)
	{
		IGraph graph = node.Graph;

		for (int i = 0; i < sceneNode.transform.childCount; ++i) {
			GameObject childNode = sceneNode.transform.GetChild(i).gameObject;
			INode childUriNode = CreateSceneUriNode(childNode,serializerContext,graph); 
			graph.Assert (node, graph.CreateUriNode (parentPropertyRdf), childUriNode);
		}
	}

	private static void SerializeParent(GameObject sceneNode, RdfSerializerContext serializerContext, INode node)
	{
		if (sceneNode.transform.parent == null)
			return;
		
		GameObject parentNode = sceneNode.transform.parent.gameObject; 
		if (parentNode == null)
			return;
		
		IGraph graph = node.Graph;
		
		INode parentUriNode = CreateSceneUriNode(parentNode,serializerContext,graph);
		graph.Assert(node,graph.CreateUriNode(childPropertyRdf),parentUriNode);
	}

	private static void DeserializeParent(GameObject sceneNode, RdfSerializerContext serializerContext, INode node)
	{
		INode parentRdfNode = GetObjectByPredicateName(node,childPropertyRdf,false);
		GameObject parentNode = null;
		if (parentRdfNode != null && parentRdfNode.NodeType == NodeType.Uri)
		{
			int id = GetSceneNodeIdFromUriNode(parentRdfNode);
			parentNode = sceneNode.Find(id);
		}

		if (parentNode == null)
		{
			sceneNode.transform.parent = null;
		}
		else
		{
			sceneNode.transform.parent = parentNode.transform;
		}
	}

	private static void DeserializeChildren(GameObject sceneNode, RdfSerializerContext serializerContext, INode node)
	{
		IGraph graph = node.Graph;
		
		IEnumerable<Triple> directParentTriples = graph.GetTriplesWithSubjectPredicate(node,graph.CreateUriNode(parentPropertyRdf));

		List<GameObject> requestedChilds = new List<GameObject>();

		foreach (Triple triple in directParentTriples)
		{
			int id = GetSceneNodeIdFromUriNode(triple.Object);
			GameObject childNode = sceneNode.Find (id);
			requestedChilds.Add(childNode);
		}

		for (int i=0; i<sceneNode.transform.childCount; ++i)
		{
			sceneNode.transform.GetChild(i).parent = null;
		}

		foreach (GameObject child in requestedChilds)
		{
			child.transform.parent = sceneNode.transform;
		}

	}


	private static void AddCoordinateSystem(GameObject sceneNode,RdfSerializerContext serializerContext, INode node )
	{
		IGraph graph = node.Graph;
		INode coordinateSystemNode = CreateCoordinateSystemNode(sceneNode,serializerContext,graph);
		
		graph.Assert (node, graph.CreateUriNode (NodeFixedCoordPropertyRdf),coordinateSystemNode);
	}

	private static void SerializeComponents(GameObject sceneNode,RdfSerializerContext serializerContext, INode node )
	{
		IGraph graph = node.Graph;
		
		var components = sceneNode.GetComponents(typeof(Component));

		IUriNode componentPropertyNode = graph.CreateUriNode(ComponentPropertyRdf);
		IUriNode componentEnabledPropertyNode = graph.CreateUriNode(componentEnabledPropertyRdf);
		foreach (Component component in components)
		{
			string name = component.GetType().ToString();
			if (name.StartsWith("UnityEngine.Transform"))
			    continue;

			IBlankNode componentNode = graph.CreateBlankNode();

			graph.Assert(node,componentPropertyNode,componentNode);
			AddRdfType(componentNode, ComponentTypeRdf);
			AddRdfLabel(componentNode,name);
			bool enabled = true;
			if (component.GetType().IsSubclassOf(typeof(MonoBehaviour)))
				enabled = (component as MonoBehaviour).enabled;
			graph.Assert(componentNode,componentEnabledPropertyNode,enabled.ToLiteral(graph));

			if (name == "ExternalUri")
			{
				AddRdfType(componentNode, "demo:ExternalLink");
				Uri link = new Uri((component as ExternalUri).WebLink);
				graph.Assert(componentNode,graph.CreateUriNode("rdfs:seeAlso"), graph.CreateUriNode(link));
			}
		}
	}

	private static void DeserializeComponents(GameObject sceneNode,RdfSerializerContext serializerContext, INode node )
	{
		IGraph graph = node.Graph;
		
		var existingComponents = sceneNode.GetComponents(typeof(Component));
		List<Component> removedComponents = existingComponents.ToList();
		removedComponents.Remove(sceneNode.GetComponent<Transform>());
		
		IUriNode componentPropertyNode = graph.CreateUriNode(ComponentPropertyRdf);

		var components = graph.GetTriplesWithSubjectPredicate(node,componentPropertyNode);

		List<string> newComponentNames  = new List<string>();

		foreach (Triple triple in components)
		{
			string name = GetRdfLabel(triple.Object);

			bool exists = false;
			foreach (Component component in existingComponents)
			{
				if (component.GetType().ToString() == name)
				{
					exists = true;
					removedComponents.Remove(component);

					System.Reflection.PropertyInfo enabledFlag = component.GetType().GetProperty("enabled",typeof(bool));

					if (enabledFlag != null)
					{
						bool enabled = GetLiteralByPredicateName<bool>(triple.Object,componentEnabledPropertyRdf);
						enabledFlag.SetValue(component,enabled,null);
					}

					if (name == "ExternalUri")
					{
						AssertRdfType(triple.Object,"demo:ExternalLink");
						var triples = graph.GetTriplesWithSubjectPredicate(triple.Object,graph.CreateUriNode(SeeAlsoPropertyRdf));
						var linkObject = triples.Single().Object;

						if (linkObject.NodeType == NodeType.Uri)
							(component as ExternalUri).WebLink = (linkObject as IUriNode).Uri.ToString();
					}

					break;
				}
			}
			if (!exists)
				newComponentNames.Add(name);
		}

		foreach (string newComponentName in newComponentNames)
		{
			if (newComponentName == "UnityEngine.Rigidbody")
			{
				Rigidbody newComponent = sceneNode.AddComponent<Rigidbody>();

				newComponent.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			}
		}

		foreach (Component removedComponent in removedComponents)
		{
			GameObject.Destroy(removedComponent);
		}

	}


}
