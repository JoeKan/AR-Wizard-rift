using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using VDS.RDF;
using VDS.RDF.Writing;


public class RDFTerm  {

    protected INode parent;
    protected INode parentPredicate;
    bool addedToGraph = false;

    protected INode node = null;
    protected IGraph graph;

    public RDFTerm(Triple triple)
    {
        this.node = triple.Object;
        this.graph = node.Graph;
        this.parent = triple.Subject;
        this.parentPredicate = triple.Predicate;
        addedToGraph = true;
    }

    public RDFTerm(INode node, INode parent = null, INode parentPredicate = null, bool alreadyAdded = false)
    {
        this.node = node;
        this.graph = node.Graph;
        this.parent = parent;
        this.parentPredicate = parentPredicate;
        addedToGraph = alreadyAdded;
    }

    public RDFTerm(IGraph graph, INode parent = null, INode parentPredicate = null, bool alreadyAdded = false)
    {
        this.graph = graph;
        this.parent = parent;
        this.parentPredicate = parentPredicate;
        addedToGraph = alreadyAdded;
    }

    public RDFTerm this[INode predicate]
    {
        get
        {
            if ((!isURI && !isBlankNode) || isLiteral)
            {
                throw new Exception("RDFTerm is either not a uri or black, or is a literal, operator illegal");
            }
            var triples = graph.GetTriplesWithSubjectPredicate(node, predicate);

            INode obj = null;

            if (triples != null && triples.Any())
                obj = triples.Single().Object;

            if (obj == null)
                return new RDFTerm(graph, node, predicate);

            return new RDFTerm(obj, node, predicate);
        }
    }

    public RDFTerm this[string predicate]
    {
        get
        {
            INode gPredicate = graph.CreateUriNode(predicate);
            return this[gPredicate];
        }
    }

    public RDFTerm this[RDFTerm predicate]
    {
        get {

            return this[predicate.node];
        }        
    }

    public RDFTerm addObject(string predicate)
    {
        INode gPredicate = graph.CreateUriNode(predicate);
        return new RDFTerm(graph, node, gPredicate);
    }

    public List<RDFTerm> getObjects(string predicate)
    {
        INode gPredicate = graph.CreateUriNode(predicate);
        var triples = graph.GetTriplesWithSubjectPredicate(node, gPredicate);

        List<RDFTerm> result = new List<RDFTerm>();

        foreach (var item in triples)
        {
            //result.Add(new RDFTerm(item.Object, item.Subject, item.Predicate, true));
            result.Add(new RDFTerm(item));
        }

        return result;
        
    }

    public bool hasObject(string qname)
    {
        
        INode gPredicate = graph.CreateUriNode(qname);
        var triples = graph.GetTriplesWithSubjectPredicate(node, gPredicate);
        return triples.Any();
    }

    public bool hasTriple(string qpredicate, string qobject){
        
        INode obj = graph.CreateUriNode(qobject);
        RDFTerm rdfobj = new RDFTerm(obj, null, null, true);
        var types = getObjects(qpredicate);

        return types.Contains(rdfobj);
    }

   public  T getLiteralValue<T>()
    {
       

        
        if (!isLiteral)
            throw new Exception("object of predicate \"" + Convert.ToString(parentPredicate) + "\" of subject: \"" + Convert.ToString(parent) + "\" is not a literal");

        ILiteralNode literal = node as ILiteralNode;
        return (T)Convert.ChangeType(literal.Value, typeof(T), CultureInfo.InvariantCulture); 
    }

    public void setLiteralValue<T>(T value)
    {
        //if (null != null && !isLiteral)
        //    throw new Exception("can't set literal value, RDFTerm is already of type:"+node.NodeType.ToString());

        Uri type = null;
        if (typeof(T) == typeof(int))
            type = new Uri("http://www.w3.org/2001/XMLSchema#int");
        else if (typeof(T) == typeof(float))
            type = new Uri("http://www.w3.org/2001/XMLSchema#int");
        else if (typeof(T) == typeof(double))
            type = new Uri("http://www.w3.org/2001/XMLSchema#double");
        else if (typeof(T) == typeof(short))
            type = new Uri("http://www.w3.org/2001/XMLSchema#short");
        else if (typeof(T) == typeof(long))
            type = new Uri("http://www.w3.org/2001/XMLSchema#long");
        else if (typeof(T) == typeof(string))
            type = new Uri("http://www.w3.org/2001/XMLSchema#string");
        

        if (node == null) {
            if (type == null)
                node = graph.CreateLiteralNode(value.ToString());
            else
                node = graph.CreateLiteralNode(value.ToString(), type);
        }

        graph.Assert(parent, parentPredicate, node);
        
    }

    public void setURI(string uri)
    {
        //if (null != null && !isURI)
        //    throw new Exception("can't set uri, RDFTerm is already of type:" + node.NodeType.ToString());

        if (node == null)
        {
            node = graph.CreateUriNode(uri);
            checkNode();
        }
        else
        {

        }
    }

    public void setURI(Uri uri)
    {
        //if (null != null && !isURI)
        //    throw new Exception("can't set uri, RDFTerm is already of type:" + node.NodeType.ToString());

        if (node == null)
        {
            node = graph.CreateUriNode(uri);
            checkNode();
        }
        else
        {

        }
        

        
    }

    public Uri getURI()
    {
        IUriNode urinode = node as IUriNode;
        return urinode.Uri;
    }

    public RDFTerm makeBlankNode()
    {
        //if (null != null && !isBlankNode)
        //    throw new Exception("can't make blank node, RDFTerm is already of type:" + node.NodeType.ToString());

        if (isBlankNode)
            return this;

        node = graph.CreateBlankNode();
        checkNode();
        return this;
    }

    public bool isURI
    {
        get { return node is IUriNode; } 
    }

    public bool isBlankNode
    {
        get { return node is IBlankNode; }
    }

    public bool isLiteral
    {
        get { return node is ILiteralNode; }
    }

    protected void checkNode()
    {
        if (addedToGraph)
            return;
        addedToGraph = true;
        if(parent != null && parentPredicate != null)
            graph.Assert(parent, parentPredicate, node);
    }

    public override bool Equals(System.Object obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        RDFTerm p = obj as RDFTerm;
        if ((System.Object)p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return node.Equals(p.node) && graph.Equals(p.graph);
    }

   

    public override string ToString()
    {
        return Convert.ToString(node);
    }

    public override int GetHashCode()
    {
        if (node == null)
            return base.GetHashCode();

        return node.GetHashCode();
    }
}
