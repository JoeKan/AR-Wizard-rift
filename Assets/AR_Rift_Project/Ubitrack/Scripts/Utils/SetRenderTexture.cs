using UnityEngine;
using System.Collections;

public class SetRenderTexture : MonoBehaviour {
	public RenderTexture renderTexture;
	public RenderTextureFormat renderTextureformet = RenderTextureFormat.ARGB32;
	public int width=640;
	public int height=480;
	public int depth=16;

	// Use this for initialization
	void Awake () {
		renderTexture = new RenderTexture(width, height, depth, renderTextureformet);
		renderTexture.Create();

		GetComponent<Camera>().targetTexture = renderTexture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
