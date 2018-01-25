using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using System;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

public class ScenegraphRoot : MonoBehaviour {
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
			if (mimeType == "" || mimeType == "*" || mimeType == "*/*" || mimeType == "text/turtle") 
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
		if (context.Request.AcceptTypes.Length == 0)
		{
			type = RdfSerializer.NotationType.Turtle;
			typeString = "text/turtle";
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
			context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			return;
		}
		else
		{
			int requestedID = int.MaxValue;

			try
			{
				requestedID = Convert.ToInt32(parameters[0]);
			}
			catch(Exception)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				return;
			}

			GameObject sceneNode = gameObject.Find(requestedID);
			

			if (sceneNode == null)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				return;
			}
			if (parameters.Length > 1)
			{
				string returnValue = null;
				RdfSerializerContext serializerContext = new RdfSerializerContext(context.Request.Url.ToString(),"");

				if (parameters.Length > 2)
				{
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					return;
				}
				if (parameters[1] == "transform")
					returnValue = TransformSerializer.SerializeToString(sceneNode.transform,serializerContext,notation);
				else if (parameters[1] == "coord")
					returnValue = CoordinateSystemSerializer.SerializeToString(sceneNode,serializerContext,notation);

				if (returnValue != null)
				{
					context.Response.StatusCode = (int)HttpStatusCode.OK;
					SetResponseBody(returnValue,context.Response);
				}
				else
				{
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				}

			}
			else
			{
				RdfSerializerContext serializerContext = new RdfSerializerContext(context.Request.Url.ToString(),"");
				string returnValue = SceneNodeSerializer.SerializeToString(sceneNode,serializerContext,notation);
				context.Response.StatusCode = (int)HttpStatusCode.OK;
				SetResponseBody(returnValue,context.Response);
				return;
			}
		}
	}

	public void PutMethod(HttpListenerContext context,string[] parameters)
	{
		DebugOutputParameters (parameters);

		if (parameters.Length == 0)
		{
			context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			return ;
		}
		else
		{
			int requestedID = int.MaxValue;
			try
			{
				requestedID = Convert.ToInt32(parameters[0]);
			}
			catch(Exception)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				return;
			}

			GameObject sceneNode = gameObject.Find(requestedID);
			
			if (sceneNode == null)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				return;
			}
			IGraph rdfGraph = new Graph();
			string body = new StreamReader(context.Request.InputStream).ReadToEnd();
			try 
			{
				StringParser.Parse(rdfGraph, body);
			} 
			catch (RdfParseException parseEx) 
			{
				//This indicates a parser error e.g unexpected character, premature end of input, invalid syntax etc.
				Debug.LogException(parseEx);
			} 
			catch (RdfException rdfException)
			{
				//This represents a RDF error e.g. illegal triple for the given syntax, undefined namespace
				Debug.LogException(rdfException);
			}

			if (parameters.Length > 1)
			{
				if (parameters.Length > 2)
				{
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					return;
				}
				if (parameters[1] == "transform")
				{
					//RdfSerializerContext serializerContext = new RdfSerializerContext(context.Request.Url.ToString(),"");
					TransformSerializer.DeserializeFromNode(rdfGraph.CreateUriNode(),sceneNode.transform);
					context.Response.StatusCode = (int)HttpStatusCode.NoContent;
					return;
				}

				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}
			else
			{
				RdfSerializerContext serializerContext = new RdfSerializerContext(context.Request.Url.ToString(),"");
				SceneNodeSerializer.UpdateFromNode(sceneNode,serializerContext,rdfGraph.CreateUriNode());
				context.Response.StatusCode = (int)HttpStatusCode.NoContent;
			}
		}
	}



	void Start()
	{
		WebListener.WebCallbacks callbacks = new WebListener.WebCallbacks();
	    callbacks.GetMethod = GetMethod;
		callbacks.PutMethod = PutMethod;
        GameObject.Find("Webserver").GetComponent<WebListener>().localCallbackMapping["scenenode"] = callbacks;

	}

	static void SetResponseBody(string value, HttpListenerResponse response)
	{
		byte[] buffer = Encoding.UTF8.GetBytes(value);
		response.ContentLength64 = buffer.Length;
		response.OutputStream.Write(buffer, 0, buffer.Length);
	}



}
