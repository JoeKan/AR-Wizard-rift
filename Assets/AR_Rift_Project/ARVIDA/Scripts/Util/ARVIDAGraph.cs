using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;


public class ARVIDAGraph  {
    public enum NotationType
    {
        Turtle,
        Xml,
        Json,
        Invalid
    }

    protected IGraph graph;

    public ARVIDAGraph(IGraph graph)
    {
        this.graph = graph;
    }

    public ARVIDAGraph(string baseUri)
    {
        graph = new Graph();

        graph.BaseUri = new Uri(baseUri);
        addNamespace("demo", "http://foo.bar#");
        addNamespace("ldp", "http://www.w3.org/ns/ldp#");
        addNamespace("xsd", "http://www.w3.org/2001/XMLSchema#");
        addNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
        addNamespace("rdfs", "http://www.w3.org/2000/01/rdf-schema#");
        addNamespace("owl", "http://www.w3.org/2002/07/owl#");
        addNamespace("core", "http://vocab.arvida.de/2015/06/core/vocab#");
        addNamespace("dataflow", "http://vocab.arvida.de/2015/06/dataflow/vocab#");
        addNamespace("vom", "http://vocab.arvida.de/2015/06/vom/vocab#");
        addNamespace("maths", "http://vocab.arvida.de/2015/06/maths/vocab#");
        addNamespace("spatial", "http://vocab.arvida.de/2015/06/spatial/vocab#");
        addNamespace("tracking", "http://vocab.arvida.de/2015/06/tracking/vocab#");
        addNamespace("ubitrack", "http://vocab.arvida.de/2015/06/ubitrack/vocab#");
        addNamespace("scenegraph", "http://vocab.arvida.de/2015/06/scenegraph/vocab#");
        addNamespace("vis", "http://vocab.arvida.de/2015/06/vis/vocab#");
    }

    public void addNamespace(string prefix, string uri)
    {
        graph.NamespaceMap.AddNamespace(prefix, new Uri(uri));
    }


    public void readRDFText(string rdfText, string baseUri)
    {
        graph.BaseUri = new Uri(baseUri);
        StringParser.Parse(graph, rdfText);
    }

    public INode getUriNode(string qname)
    {
        return getUriNode(graph.ResolveQName(qname));
    }

    public INode getUriNode(Uri uri)
    {
        return graph.CreateUriNode(uri);
    }

    public INode getBlankNode()
    {
        return graph.CreateBlankNode();
    }

    public RDFTerm getUriTerm(string qname)
    {
        return getUriTerm(graph.ResolveQName(qname));
    }

    public RDFTerm getUriTerm(Uri uri)
    {
        return new RDFTerm(graph.CreateUriNode(uri));
    }

    public List<RDFTerm> getSubjects(string predicate, string obj)
    {
        return getSubjects(graph.ResolveQName(predicate), graph.ResolveQName(obj));

    }
    public List<RDFTerm> getSubjects(Uri predicate, Uri obj)
    {
        //INode gPredicate = graph.CreateUriNode(predicate);
        graph.CreateUriNode(predicate);
        var triples = graph.GetTriplesWithPredicateObject(getUriNode(predicate), getUriNode(obj));

        List<RDFTerm> result = new List<RDFTerm>();

        foreach (var item in triples)
        {
            //result.Add(new RDFTerm(item.Object, item.Subject, item.Predicate, true));
            result.Add(new RDFTerm(item.Subject, null, null, true));
        }

        return result;
    }

    public string SerializeToString(ARVIDAGraph.NotationType notation)
    {
        if (notation == NotationType.Turtle)
            return serializeToTurtle();
        else if (notation == NotationType.Xml)
            return serializeToXml();
        else if (notation == NotationType.Json)
            return serializeToJson();

        return "";
    }

    public string serializeToTurtle()
    {
        CompressingTurtleWriter writer = new CompressingTurtleWriter();
        writer.CompressionLevel = WriterCompressionLevel.High;
        writer.PrettyPrintMode = true;

        String data = StringWriter.Write(graph, writer);
        return data;
    }
    public string serializeToXml()
    {
        var writer = new VDS.RDF.Writing.PrettyRdfXmlWriter();
        writer.CompressionLevel = WriterCompressionLevel.High;
        writer.PrettyPrintMode = true;

        String data = StringWriter.Write(graph, writer);
        return data;
    }
    public string serializeToJson()
    {
        var writer = new VDS.RDF.Writing.RdfJsonWriter();
        writer.PrettyPrintMode = true;

        String data = StringWriter.Write(graph, writer);
        return data;
    }

    public IGraph getGraph()
    {
        return graph;
    }
}

