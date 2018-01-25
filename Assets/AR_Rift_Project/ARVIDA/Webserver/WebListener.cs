using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using System.Net;
using System.Linq;
using System.Text;

public class WebListener : MonoBehaviour
{
    public class WebCallbacks
    {
        public delegate void ResponderDelegate(HttpListenerContext context,string[] parameters);

        public ResponderDelegate GetMethod;
        public ResponderDelegate PutMethod;
        public ResponderDelegate PostMethod;
        public ResponderDelegate DeleteMethod;
    }
    private ConcurrentQueue<HttpListenerContext> requestQueue = new ConcurrentQueue<HttpListenerContext>();
	private HttpListener listener = new HttpListener();
	

    public Dictionary<string, WebCallbacks> localCallbackMapping = new Dictionary<string, WebCallbacks>(); 
	


	public int Port = 8080;
	public string RelativeUri;

	public WebListener()
	{

	}

	public void Run()
	{
		ThreadPool.QueueUserWorkItem((o) =>
		                             {
			Console.WriteLine("Webserver running...");
			try
			{
				while (listener.IsListening)
				{
					ThreadPool.QueueUserWorkItem((c) =>
					{
						var ctx = c as HttpListenerContext;
						if (ctx != null)
						{

							requestQueue.Enqueue(ctx);
						}

					}, listener.GetContext());
				}
			}
			catch { } // suppress any exceptions
		});
	}

	void ProcessMethod(HttpListenerContext context, WebCallbacks.ResponderDelegate responder, string[] parameters)
	{
		try
		{
			if (responder == null)
				context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
			else
			{
				responder(context,parameters);
			}
		}
		//catch { } // suppress any exceptions
		finally
		{
			// always close the stream
			context.Response.OutputStream.Close();
		}
	}

	void FixedUpdate()
	{
		while (requestQueue.Count > 0)
		{
			//try
			//{
				HttpListenerContext context = requestQueue.Dequeue();
				HttpListenerRequest request = context.Request;
				Debug.Log("process: " + request.HttpMethod);

				string localName = "";
				string[] parameters = {};
				if (context.Request.Url.Segments.Count() > 1)
				{
                	localName = context.Request.Url.Segments[1].Replace("/", "");
                	parameters = context.Request.Url
                                        .Segments
                                        .Skip(2)
                                        .Select(s => s.Replace("/", ""))
                                        .ToArray();
				}
			    WebCallbacks callbacks = null;
			    if (!localCallbackMapping.TryGetValue(localName, out callbacks))
			    {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.OutputStream.Close();
			        continue;
			    }

				if (request.HttpMethod == "OPTIONS")
				{
					string returnValue = "";
					if (callbacks.GetMethod != null)
						returnValue += "GET;";
					if (callbacks.PostMethod != null)
						returnValue += "POST;";
                    if (callbacks.PutMethod != null)
						returnValue += "PUT;";
                    if (callbacks.DeleteMethod != null)
                        returnValue += "DELETE;";

					returnValue += "OPTIONS";

					context.Response.AddHeader("Allow",returnValue);
					context.Response.StatusCode = (int) HttpStatusCode.NoContent;
				

					context.Response.OutputStream.Close();

					continue;
				}
				WebCallbacks.ResponderDelegate requestMethod = null;
				if (request.HttpMethod == "GET")
                    requestMethod = callbacks.GetMethod;
				else if (request.HttpMethod == "PUT")
                    requestMethod = callbacks.PutMethod;
				else if (request.HttpMethod == "POST")
                    requestMethod = callbacks.PostMethod;
				else if (request.HttpMethod == "DELETE")
                    requestMethod = callbacks.DeleteMethod;

				ProcessMethod(context,requestMethod,parameters);
			//}
			//catch { }
		}
	}

	void Start()
	{
		string uri = "http://+:" + Port.ToString() + "/";
		if (!string.IsNullOrEmpty(RelativeUri))
			uri += RelativeUri + "/";
		
		
		listener.Prefixes.Add(uri);
	

		listener.Start();

		Run();

		Debug.Log("start listener: " + uri);
	}

	void Destroy()
	{
		listener.Stop();
		listener.Close();
	}
}
