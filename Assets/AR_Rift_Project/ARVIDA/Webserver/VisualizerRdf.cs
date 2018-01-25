using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using System;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

public class VisualizerRdf : MonoBehaviour {
	public GameObject rootNode = null;
	static void DebugOutputParameters (string[] parameters)
	{
		StringBuilder builder = new StringBuilder ();
		foreach (string value in parameters) {
			builder.Append (value);
			builder.Append ('.');
		}
		Debug.Log (builder.ToString ());
	}
	
	static RdfSerializer.NotationType HandleMimeType (HttpListenerContext context)
	{
		RdfSerializer.NotationType type = RdfSerializer.NotationType.Invalid;
		string typeString = "";
		foreach (string mimeType in context.Request.AcceptTypes) 
		{
			if (mimeType == "text/turtle" || mimeType == "*/*") 
			{
				type = RdfSerializer.NotationType.Turtle;
				typeString = mimeType;
				break;
			}
			else if (mimeType == "application/xml" || mimeType == "text/xml") 
			{
				type = RdfSerializer.NotationType.Xml;
				typeString = mimeType;
				break;
			}
			else if (mimeType == "application/json") 
			{
				type = RdfSerializer.NotationType.Json;
				typeString = mimeType;
				break;
			}
		}
		
		if (type == RdfSerializer.NotationType.Invalid)
			context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
		else 
			context.Response.ContentType = typeString;
		
		return type;
	}
	
	public void GetMethod(HttpListenerContext context,string[] parameters)
	{
		DebugOutputParameters (parameters);
		
		RdfSerializer.NotationType notation = HandleMimeType(context);
		if (notation == RdfSerializer.NotationType.Invalid)
			return;
		
		
		if (parameters.Length == 0)
		{

			RdfSerializerContext serializerContext = new RdfSerializerContext(context.Request.Url.ToString(),"");
			IGraph graph = RdfSerializer.CreateArvidaGraph(serializerContext);


			INode visualizerNode = graph.CreateUriNode();
			RdfSerializer.AddRdfType(visualizerNode,"vis:VisualizationSystem");
			RdfSerializer.AddRdfLabel(visualizerNode,"Unity");

			graph.Assert(new Triple(visualizerNode,graph.CreateUriNode("vis:rootNode"),RdfSerializer.CreateSceneUriNode(rootNode,serializerContext,graph)));
			String responseBody = RdfSerializer.SerializeGraphToString(graph,notation);
			

			context.Response.StatusCode = (int)HttpStatusCode.OK;
			//transform.root.gameObject.GetInstanceID().ToString()                               
			SetResponseBody(responseBody,context.Response);
		}
		else
		{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				return;
		}
	}

	
	void Start()
	{
		WebListener.WebCallbacks callbacks = new WebListener.WebCallbacks();
		callbacks.GetMethod = GetMethod;
		GameObject.Find("Webserver").GetComponent<WebListener>().localCallbackMapping[""] = callbacks;
		
	}
	
	static void SetResponseBody(string value, HttpListenerResponse response)
	{
		byte[] buffer = Encoding.UTF8.GetBytes(value);
		response.ContentLength64 = buffer.Length;
		response.OutputStream.Write(buffer, 0, buffer.Length);
	}
	
	
	
}
