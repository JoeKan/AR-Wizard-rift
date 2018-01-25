using UnityEngine;
using System.Collections;
using UnityEditor;

namespace FAR{
    /*
[System.Serializable]
[CustomEditor(typeof(AF_PoseSource),true)]


public class PoseSourceCustom : Editor {
			
	string[] MainChoices = {"UbiTrack","ARVIDA"};
	
	string[] LocalWorld = {"Local","World"};
	
	string[] PushPull = {"Push","Pull"};


	public override void OnInspectorGUI()
	{
	
			AF_PoseSource myAF_PoseSource = (AF_PoseSource) target;
	
			myAF_PoseSource.Switch = EditorGUILayout.Popup("Framework",myAF_PoseSource.Switch,MainChoices);
			
			switch(myAF_PoseSource.Switch)
			{
				case 0:
					StartUbiTrackInterface(myAF_PoseSource);
					break;
				case 1:
					StartARVIDAInterface(myAF_PoseSource);
					break;
				default:
					break;
			}
	}
	
	public void StartUbiTrackInterface(AF_PoseSource trgt)
	{
		
		trgt.Simple_Facade = EditorGUILayout.ObjectField("SimpleFacade",trgt.Simple_Facade,typeof(SimpleFacade),true) as SimpleFacade;

		trgt.PatternID = EditorGUILayout.TextField("PatternID",trgt.PatternID);
		
		trgt.LocalWorld = EditorGUILayout.Popup("Local/World-Space",trgt.LocalWorld,LocalWorld);
		
		trgt.PushPull = EditorGUILayout.Popup ("Push/Pull",trgt.PushPull,PushPull);
		
	}
	public void StartARVIDAInterface(AF_PoseSource trgt)
	{
		
		trgt.URI = EditorGUILayout.TextField("URI",trgt.URI);
		
		trgt.LocalWorld = EditorGUILayout.Popup("Local/World-Space",trgt.LocalWorld,LocalWorld);
			
		trgt.PushPull = EditorGUILayout.Popup ("Push/Pull",trgt.PushPull,PushPull);
		
	}
			
}

    */
}