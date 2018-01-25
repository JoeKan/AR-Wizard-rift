using UnityEngine;
using System;
using System.Collections;

namespace FAR{

	public class ImageTextureUpdate : UbitrackSourceComponent<ulong> {

	
	public bool m_flipVertical;
    public bool m_flipHorizontal;
	public TextureFormat textureformat = TextureFormat.RGBA32;
	public RenderTextureFormat renderTextureformet = RenderTextureFormat.ARGB32;

	public bool useRenderTexture = false;

	private IntPtr texturePtr;
	
    private int m_imageWidth = 320;
    private int m_imageHeight = 240;
	private int m_imageChannels = 3; 
    private int m_texWidth;
    private int m_texHeight;
    private float m_textureInsetWidth;
    private float m_textureInsetHeight;
	private float m_textureInsetX;
	private float m_textureInsetY;

    protected Texture m_texture=null;

    private int m_screenWidth;
    private int m_screenHeight;
	public GUITexture m_guiTexture;
	public Material[] m_material;	
	public string TexturePropertyName = "_MainTex";

	private bool m_isTextureInitialized = false;
		private SimplePosition3D m_dummyValue;

    private SimpleApplicationPullSinkPosition3D m_dummy;


	
    public override void utInit(SimpleFacade simpleFacade)
    {
        base.utInit(simpleFacade);

			m_dummyValue = new SimplePosition3D();

        m_dummy = simpleFacade.getPullSinkPosition3D(patternID);
        if (m_dummy == null)
        {
            throw new Exception("SimpleApplicationPushSourceButton not found for ID:" + patternID);
        }
		 this.m_screenHeight = Screen.height;
         this.m_screenWidth = Screen.width;
		//initTexture();
    }


	// Update is called once per frame
	//void OnRenderObject () {
	//void OnPreCull(){
		void Update () {
			
        if(m_isTextureInitialized){			
			//Profiler.StartProfile("updateTextureComponent");  


			ulong dummy = (ulong) m_texture.GetNativeTexturePtr();
			m_dummy.getPosition3D(m_dummyValue, dummy);

			if(m_dummyValue.timestamp > 0) {
					m_lastData = new Measurement<ulong>(m_dummyValue.timestamp,m_dummyValue.timestamp);
					triggerPull(m_dummyValue.timestamp);
			}
			//if(m_dummyValue.timestamp == dummy) Profiler.EndProfile("updateTextureComponent");                    				
		} else {
				Debug.Log ("do init");
			ulong dummy = (ulong)0;
			m_dummy.getPosition3D(m_dummyValue, dummy);
			//if(m_dummyValue.x > 10f){
				m_imageWidth = 640;//(int)m_dummyValue.x;
				m_imageHeight = 480;//(int)m_dummyValue.y;
				m_imageChannels = 3;//(int)m_dummyValue.z;
					initTexture();
			//}
		}
		//Profiler.PrintResults();
	}
	void OnApplicationPause(){
		//Profiler.PrintResults();
	}
	private void initTexture(){
				
		m_texWidth = m_imageWidth;//(int)Math.Pow(2, Math.Ceiling(Math.Log(m_imageWidth) / Math.Log(2)));
		m_texHeight = m_imageHeight;// (int)Math.Pow(2, Math.Ceiling(Math.Log(m_imageHeight) / Math.Log(2)));
		
		
		
		m_textureInsetWidth = ((float)m_screenWidth) / m_imageWidth * m_texWidth;
		m_textureInsetHeight = ((float)m_screenHeight) / m_imageHeight * m_texHeight;
		m_textureInsetX = 0;
		m_textureInsetY = 0;		
		
		if(m_flipVertical){
			m_textureInsetHeight = -m_textureInsetHeight;
			m_textureInsetY = m_screenHeight;
		}
		
		if(m_flipHorizontal){
			m_textureInsetWidth = -m_textureInsetWidth;
			m_textureInsetX = m_screenWidth;
		}
		
		Debug.Log("Init Texture2D: " + m_imageWidth + "x" + m_imageHeight + "  on screen:" + m_screenWidth + "x" + m_screenHeight + " with textureSize:" + m_texWidth + "x" + m_texHeight);					

		if(useRenderTexture) {
			RenderTexture rt = new RenderTexture(m_texWidth, m_texHeight, 16, renderTextureformet);
			rt.Create();
			m_texture = rt;
			Debug.Log("create render texture:: "+renderTextureformet); 

		} else {

                Debug.Log("Ubitrack: create texture with:" + m_texWidth + " : " + m_texHeight + " : " + textureformat);

			if(m_imageChannels == 3)
				m_texture = new Texture2D(m_texWidth, m_texHeight, textureformat, false);
			else
				m_texture = new Texture2D(m_texWidth, m_texHeight, textureformat, false);

		
			

			m_texture.filterMode = FilterMode.Bilinear;	
		}



						
		
		if(m_material.Length > 0)
		{
			float scaleX = (float)m_imageWidth / m_texWidth;
			if(m_flipVertical)
				scaleX = -scaleX;
			float scaleY = (float)m_imageHeight / m_texHeight;						
			if(m_flipHorizontal)
				scaleY = -scaleY;
			for(int i=0;i<m_material.Length;i++) {
				m_material[i].SetTexture(TexturePropertyName, m_texture);
				
				m_material[i].SetTextureScale(TexturePropertyName, new Vector2(scaleX, scaleY));
				//m_material[i].mainTextureScale = new Vector2(scaleX, scaleY);
			}
			
		}
		if(m_guiTexture != null)
		{					
			Debug.Log("m_guiTexture:");
			m_guiTexture.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
			m_guiTexture.pixelInset = new Rect(m_textureInsetX, m_textureInsetY, m_textureInsetWidth, m_textureInsetHeight);
			m_guiTexture.texture = m_texture;
		}	

		
		texturePtr = m_texture.GetNativeTexturePtr();
		Debug.Log("texturePtr:"+texturePtr);
		m_isTextureInitialized = true;
        
	}

	public Texture getImageTexture() {	
		return m_texture;
	}
}

}