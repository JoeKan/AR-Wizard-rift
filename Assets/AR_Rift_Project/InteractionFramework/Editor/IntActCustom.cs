using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

namespace FAR{

[System.Serializable]
[CustomEditor(typeof(InteractionMethod),true)]
public class IntActCustom : Editor {

	public GameObject Twinkie;
	public IInteractionMethod ThisScript;
	
	private Dictionary<string,Action> MyActions;
	
	string SendableEventTextField;

	public override void OnInspectorGUI()
	{

		//ThisScript = (InteractionMethod) target;
		//ThisScript = EditorGUILayout.ObjectField("Script",ThisScript,typeof(InteractionMethod),false) as InteractionMethod;
	
		if(MyActions==null)
		{
			MyActions = new Dictionary<string, Action>();
		}

		IInteractionMethod myInteractionMethod = (IInteractionMethod) target;
		
		Twinkie = EditorGUILayout.ObjectField("Add Object with Actions",Twinkie,typeof(GameObject),true) as GameObject;
		
		EditorGUILayout.LabelField("Active Listeners:");

		
		if(myInteractionMethod.ReturnInteractionMethod().Listeners.Count > 0)
		{
			for(int i = 0; i < myInteractionMethod.ReturnInteractionMethod().Listeners.Count; i++)
			{
				string Text = "";
				if(myInteractionMethod.ReturnInteractionMethod().Listeners[i]!=null)
				{
					Text = myInteractionMethod.ReturnInteractionMethod().Listeners[i].name+"=> ";
				}
				else
				{
					break;
				}

				MonoBehaviour[] Skripts = myInteractionMethod.ReturnInteractionMethod().Listeners[i].GetComponents<MonoBehaviour>();
				
				foreach(MonoBehaviour MB in Skripts)
				{
					if(MB is Action)
					{
						Text += MB.ToString().Remove(MB.ToString().IndexOf(myInteractionMethod.ReturnInteractionMethod().Listeners[i].name),myInteractionMethod.ReturnInteractionMethod().Listeners[i].name.Length);
					}
				}
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.TextField(Text);
				if(GUILayout.Button ("Delete"))
				{
					DeleteElement(Text,myInteractionMethod.ReturnInteractionMethod());
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		


		if(Twinkie != null)
		{
			if(!myInteractionMethod.ReturnInteractionMethod().Listeners.Contains(Twinkie)){
			
				MonoBehaviour[] Skripts = Twinkie.GetComponents<MonoBehaviour>();
				
				myInteractionMethod.ReturnInteractionMethod().Listeners.Add (Twinkie);
				
				foreach(MonoBehaviour MB in Skripts)
				{
					if(MB is Action)
					{
						string KeyName = MB.ToString();
						
						while(MyActions.ContainsKey(KeyName))
						{
							KeyName+="_New";
						}
						MyActions.Add (KeyName,(Action) MB);
	
					}
				}
			}
				Twinkie = null;
			
		}
		
		
		

	EditorGUILayout.Space();
	EditorGUILayout.TextArea("--------------------------");
	EditorGUILayout.Space();
	DrawDefaultInspector();

	}
	
	public void DeleteElement(string txt,InteractionMethod target)
	{
		string[] seps = {"=>"};
		
		string[] OrigName = txt.Split(seps,System.StringSplitOptions.None);
		
		List<GameObject> ToDelete = new List<GameObject>();
		
		foreach(GameObject GO in target.Listeners)
		{
			if(GO.name == OrigName[0])
			{
				ToDelete.Add (GO);
			} 
		}
		
		foreach(GameObject GO in ToDelete)
		{
			target.Listeners.Remove(GO);
		}
		
	}
}

}
