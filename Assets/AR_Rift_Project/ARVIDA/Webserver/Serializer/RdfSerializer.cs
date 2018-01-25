using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


using VDS.RDF;
using VDS.RDF.Writing;




public class RdfSerializer
{
	public enum NotationType
	{
		Turtle,
		Xml,
		Json,
		Invalid
	}

	public const string QuantityValuePropertyRdf = "vom:quantityValue";
	public const string UnitPropertyRdf = "vom:unit";
	public const string MeterRdf = "vom:Meter";
	public const string SourceCoordPropertyRdf = "spatial:sourceCoordinateSystem";
	public const string TargetCoordPropertyRdf = "spatial:targetCoordinateSystem";
	public const string NodeFixedCoordPropertyRdf = "spatial:nodeFixedCoordinateSystem";
	public const string ComponentPropertyRdf = "scenegraph:component";
	public const string SeeAlsoPropertyRdf = "rdfs:seeAlso";
	public const string ComponentTypeRdf = "scenegraph:Component";

	public static int GetSceneNodeIdFromUriNode(INode node)
	{
		Uri uri = (node as IUriNode).Uri;
		string idString = uri.Segments[uri.Segments.Count()-1];
		idString = idString.TrimEnd('/');
		return Convert.ToInt32(idString);
	}

	public static IUriNode CreateSceneUriNode(GameObject sceneNode, RdfSerializerContext serializerContext, IGraph graph)
	{
		Uri baseUri = serializerContext.BaseUri;
		int instanceID = sceneNode.GetInstanceID ();
		String instanceIDString = instanceID.ToString ();
		Uri nodeUri = new Uri (baseUri, "/scenenode/" + instanceIDString + "/");
		return graph.CreateUriNode (nodeUri);
	}
	public static IUriNode CreateTransformUriNode(GameObject sceneNode, RdfSerializerContext serializerContext, IGraph graph)
	{
		Uri nodeUri = new Uri (serializerContext.BaseUri, "/scenenode/" + sceneNode.GetInstanceID ().ToString () + "/transform/");
		return graph.CreateUriNode (nodeUri);
	}

	public static IUriNode CreateCoordinateSystemNode(GameObject sceneNode, RdfSerializerContext serializerContext, IGraph graph)
	{
		Uri nodeUri = new Uri (serializerContext.BaseUri, "/scenenode/" + sceneNode.GetInstanceID ().ToString () + "/coord/");
		return graph.CreateUriNode (nodeUri);
	}

    public static IGraph CreateArvidaGraph(RdfSerializerContext serializerContext)
    {
        IGraph graph = new Graph();

        graph.BaseUri = serializerContext.BaseUri;
        graph.NamespaceMap.AddNamespace("demo", new Uri("http://foo.bar#"));
		graph.NamespaceMap.AddNamespace("ldp", new Uri("http://www.w3.org/ns/ldp#"));
        graph.NamespaceMap.AddNamespace("vom", new Uri("http://vocab.arvida.de/2014/03/vom/vocab#"));
		graph.NamespaceMap.AddNamespace("maths", new Uri("http://vocab.arvida.de/2014/03/maths/vocab#"));
		graph.NamespaceMap.AddNamespace("spatial", new Uri("http://vocab.arvida.de/2014/03/spatial/vocab#"));
		graph.NamespaceMap.AddNamespace("scenegraph", new Uri("http://vocab.arvida.de/2014/03/scenegraph/vocab#"));
		graph.NamespaceMap.AddNamespace("vis", new Uri("http://vocab.arvida.de/2014/04/vis/vocab#"));
		
        return graph;
    }

	public static void AddRdfLabel (INode node,string label)
	{
		IGraph graph = node.Graph;
		graph.Assert (node, graph.CreateUriNode ("rdfs:label"), graph.CreateLiteralNode (label));
	}

	public static string GetRdfLabel (INode subject)
	{
		var triples = subject.Graph.GetTriplesWithSubjectPredicate(subject,subject.Graph.CreateUriNode("rdfs:label"));
		
		return ((ILiteralNode)triples.Single().Object).Value;
	}

	public static string SerializeGraphToString(IGraph graph, RdfSerializer.NotationType notation)
	{
		if (notation == NotationType.Turtle)
			return SerializeGraphToTurtle(graph);
		else if (notation == NotationType.Xml)
			return SerializeGraphToXml(graph);
		else if (notation == NotationType.Json)
			return SerializeGraphToJson(graph);

		return "";
	}

    public static string SerializeGraphToTurtle(IGraph graph)
    {
        CompressingTurtleWriter writer = new CompressingTurtleWriter();
        writer.CompressionLevel = WriterCompressionLevel.High;
        writer.PrettyPrintMode = true;

        String data = StringWriter.Write(graph, writer);
        return data;
    }
	public static string SerializeGraphToXml(IGraph graph)
	{
		var writer = new VDS.RDF.Writing.PrettyRdfXmlWriter();
		writer.CompressionLevel = WriterCompressionLevel.High;
		writer.PrettyPrintMode = true;
		
		String data = StringWriter.Write(graph, writer);
		return data;
	}
	public static string SerializeGraphToJson(IGraph graph)
	{
		var writer = new VDS.RDF.Writing.RdfJsonWriter();
		writer.PrettyPrintMode = true;
		
		String data = StringWriter.Write(graph, writer);
		return data;
	}


	public static void AddArvidaUnit(INode node, string unit)
	{
		IGraph graph = node.Graph;
		IUriNode typeNode = graph.CreateUriNode("vom:unit");
		graph.Assert(node, typeNode, graph.CreateUriNode(unit));
	}

    public static void AddRdfType(INode node, string type)
    {
		IGraph graph = node.Graph;
        IUriNode typeNode = graph.CreateUriNode("rdf:type");
		graph.Assert(node, typeNode, graph.CreateUriNode(type));
    }

	public static void AssertRdfType (INode node, string typeName)
	{
		IGraph graph = node.Graph;
		var triples =  graph.GetTriplesWithSubjectPredicate(node,graph.CreateUriNode("rdf:type"));

		INode typeNode = graph.CreateUriNode (typeName);
		foreach (Triple triple in triples) {
			if (triple.Object.Equals (typeNode))
					return;
		}

		throw new Exception("expected RDF type not found: \"" + typeName + "\"");
	}
	
	public static void SerializeLdpContainer(IGraph graph, INode node, List<Uri> containerMember)
    {
		RdfSerializer.AddRdfType( node, "ldp:Container");

		graph.Assert(node, graph.CreateUriNode("ldp:membershipSubject"),
            graph.CreateUriNode(new Uri("/", UriKind.Relative)));

		graph.Assert(node, graph.CreateUriNode("ldp:membershipPredicate"),
            graph.CreateUriNode("spatial:transformation"));

		graph.Assert(node, graph.CreateUriNode("ldp:membershipObject"),
            graph.CreateUriNode("ldp:membershipSubject"));

        foreach (Uri uri in containerMember)
        {
			graph.Assert(node, graph.CreateUriNode("ldp:member"), graph.CreateUriNode(uri));
        }
    }

	public static INode GetObjectByPredicateName(INode subject, string predicateName, bool required = true)
	{
		var triples = subject.Graph.GetTriplesWithSubjectPredicate(subject,subject.Graph.CreateUriNode(predicateName));

		if (required || (triples != null && triples.Any()))
			return triples.Single().Object;
		else
			return null;
	}


	public static ILiteralNode GetLiteralByPredicateName (INode subject, string predicateName)
	{
		var triples = subject.Graph.GetTriplesWithSubjectPredicate(subject,subject.Graph.CreateUriNode(predicateName));
		
		INode obj = null;
		
		try 
		{
			obj = triples.Single().Object;
		}
		catch (InvalidOperationException)
		{
			throw new Exception("invalid count of \"" + predicateName + "\" predicate of subject: \"" + subject.ToString() + "\""); 
		}
		
		if (obj.NodeType != NodeType.Literal)
			throw new Exception("object of predicate \"" + predicateName + "\" of subject: \"" + subject.ToString() + "\" is not a literal");
		
		return obj as ILiteralNode;
	}

	public static T GetLiteralByPredicateName<T> (INode subject, string predicateName)
	{
		IUriNode predicateNode = subject.Graph.CreateUriNode(predicateName);
		var triples = subject.Graph.GetTriplesWithSubjectPredicate(subject,predicateNode);

		INode obj = null;

		try 
		{
			 obj = triples.Single().Object;
		}
		catch (InvalidOperationException)
		{
			throw new Exception("invalid count of \"" + predicateName + "\" predicate of subject: \"" + subject.ToString() + "\""); 
		}

		if (obj.NodeType != NodeType.Literal)
			throw new Exception("object of predicate \"" + predicateName + "\" of subject: \"" + subject.ToString() + "\" is not a literal");

		return (T)Convert.ChangeType((obj as ILiteralNode).Value,typeof(T), CultureInfo.InvariantCulture);
	}


}
