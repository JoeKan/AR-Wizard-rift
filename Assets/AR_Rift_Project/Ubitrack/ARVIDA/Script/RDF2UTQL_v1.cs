using UnityEngine;
using System.Collections;
using VDS.RDF;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class RDF2UTQL_v1 : IRDF2UTQL {
    XmlDocument document;
    XmlNode utqlresponse;

    protected void setNameAndId(ref XmlNode utql, RDFTerm rdf)
    {
          XmlAttribute name = document.CreateAttribute("name"); 
          name.Value = rdf["rdf:label"].getLiteralValue<string>();
          utql.Attributes.Append(name);

          XmlAttribute id = document.CreateAttribute("id");
          //name.Value = rdf["ubitrack:id"].getLiteralValue<string>();
          name.Value = rdf.getURI().ToString();
          utql.Attributes.Append(id);
    }

    public string rdf2utql(IGraph graph)
    {
        document = new XmlDocument();

        string namespaceUri = "testNamespace";


        //ARVIDAGraph graph = new ARVIDAGraph(rdfgraph);
        INode subject = graph.CreateUriNode("rdf:type");
        INode obj = graph.CreateUriNode("ubitrack:UtqlPattern");
        var triples = graph.GetTriplesWithPredicateObject(subject, obj);


        utqlresponse = document.CreateElement("UTQLResponse", namespaceUri);
        document.AppendChild(utqlresponse);
        

        foreach (var item in triples)
        {
            RDFTerm pattern = new RDFTerm(item.Subject, null, null, true);            
            XmlNode utqlpattern = document.CreateNode(XmlNodeType.Element, "Pattern", namespaceUri);
            utqlresponse.AppendChild(utqlpattern);

            //HashSet<RDFTerm> csc = new HashSet<RDFTerm>();
            

            setNameAndId(ref utqlpattern, pattern);

            XmlNode outputNodes = document.CreateNode(XmlNodeType.Element, "Output", namespaceUri);
            utqlpattern.AppendChild(outputNodes);
            XmlNode inputNodes = document.CreateNode(XmlNodeType.Element, "Input", namespaceUri);
            utqlpattern.AppendChild(inputNodes);

            var src = pattern.getObjects("spatial:spatialRelationship");
            foreach (var sr in src)
            {
                XmlNode edgeNode = document.CreateNode(XmlNodeType.Element, "Edge", namespaceUri);
                
                XmlAttribute name = document.CreateAttribute("name");
                name.Value = sr["rdf:label"].getLiteralValue<string>();
                edgeNode.Attributes.Append(name);

                XmlAttribute source = document.CreateAttribute("source");                
                //source.Value = sr["spatial:sourceCoordinateSystem"]["ubitrack:id"].getLiteralValue<string>();
                source.Value = sr["spatial:sourceCoordinateSystem"].getURI().ToString();
                edgeNode.Attributes.Append(source);

                XmlAttribute target = document.CreateAttribute("target");
                //target.Value = sr["spatial:targetCoordinateSystem"]["ubitrack:id"].getLiteralValue<string>();
                target.Value = sr["spatial:targetCoordinateSystem"].getURI().ToString();
                edgeNode.Attributes.Append(target);

                if (sr.hasTriple("rdf:type","core:Input"))
                {
                    if (sr.hasObject("core:connectedTo"))
                    {
                        RDFTerm connected = sr["core:connectedTo"];
                        RDFTerm connectedPattern = sr["spatial:spatialRelationshipOf"];

                        XmlAttribute patternRef = document.CreateAttribute("pattern-ref");
                        //patternRef.Value = connectedPattern["ubitrack:id"].getLiteralValue<string>();
                        patternRef.Value = connectedPattern.getURI().ToString();
                        edgeNode.Attributes.Append(patternRef);

                        XmlAttribute edgeRef = document.CreateAttribute("edge-ref");
                        //edgeRef.Value = connected["ubitrack:id"].getLiteralValue<string>();
                        edgeRef.Value = connected.getURI().ToString();
                        edgeNode.Attributes.Append(edgeRef);

                    }

                    inputNodes.AppendChild(edgeNode);
                }

                
                if (sr.hasTriple("rdf:type", "core:Output"))    
                {
                    outputNodes.AppendChild(edgeNode);
                }

                // add attributes
            }
            
            // add dateflow attributes and class

        }
        
        StringWriter stringWriter = new StringWriter();
        XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
        document.WriteTo(xmlTextWriter);
        return stringWriter.ToString();
    }
}
