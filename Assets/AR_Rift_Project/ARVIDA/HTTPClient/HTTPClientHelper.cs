using System;
using System.Net;
using System.Text;
using System.IO;

public class HTTPClientHelper : ThreadedJob {

	private WebRequest request;
	private Stream dataStream;
	
	private string status;

	private string responseBody;
	private WebHeaderCollection responseHeader;


	public String Status
	{
		get
		{
			return status;
		}
		set
		{
			status = value;
		}
	}
	
	public HTTPClientHelper(string url)
	{
		// Create a request using a URL that can receive a post.
		
		request = WebRequest.Create(url);
		// Set the ContentType property of the WebRequest.
		request.ContentType = "text/turtle";
	}
	
	public HTTPClientHelper(string url, string method)
		: this(url)
	{
		
		if (method.Equals("GET") || method.Equals("POST") || method.Equals("PUT") || method.Equals("DELETE"))
		{
			// Set the Method property of the request to POST.
			request.Method = method;
		}
		else
		{
			throw new Exception("Invalid Method Type");
		}
	}

	public HTTPClientHelper(string url, string method, string contentType)
		: this(url, method)
	{
		// Set the ContentType property of the WebRequest.
		request.ContentType = contentType;
	}
	
	public HTTPClientHelper(string url, string method, string contentType, string data)
		: this(url, method, contentType)
	{

		// Create POST data and convert it to a byte array.
		string postData = data;
		byte[] byteArray = Encoding.UTF8.GetBytes(postData);

		// Set the ContentLength property of the WebRequest.
		request.ContentLength = byteArray.Length;
		
		// Get the request stream.
		dataStream = request.GetRequestStream();
		
		// Write the data to the request stream.
		dataStream.Write(byteArray, 0, byteArray.Length);
		
		// Close the Stream object.
		dataStream.Close();

	}

	protected override void ThreadFunction()
	{
		// Get the original response.
		WebResponse response = request.GetResponse();
		
		this.Status = ((HttpWebResponse)response).StatusDescription;
		
		responseHeader = response.Headers;
		// Get the stream containing all content returned by the requested server.
		dataStream = response.GetResponseStream();
		
		// Open the stream using a StreamReader for easy access.
		StreamReader reader = new StreamReader(dataStream);
		
		// Read the content fully up to the end.
		string responseFromServer = reader.ReadToEnd();
		
		// Clean up the streams.
		reader.Close();
		dataStream.Close();
		response.Close();
		
		responseBody = responseFromServer;		
	}

	public string GetResponseBody()
	{
		return responseBody;
	}

	public WebHeaderCollection getResponseHeader() {
		return responseHeader;
	}
}
