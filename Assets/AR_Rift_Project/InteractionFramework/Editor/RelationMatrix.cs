using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using FAR;



public class RelationMatrix : EditorWindow {
	
	public List<Action> AllActions = new List<Action>();
	Vector2 ScrollPos = new Vector2(0,0);
	
	GUIStyle CenterAlign = new GUIStyle();
	GUIStyle LeftAlign = new GUIStyle();
	
	public List<IInteractionMethod> MethodsWithActions = new List<IInteractionMethod>();
	
	public List<List<bool>> ToggleInput = new List<List<bool>>();
	

	[MenuItem("Window/RelationMatrix")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(RelationMatrix));
	}
	
	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/UbiTrackCalibFileEditor")]
	public static void Init()
	{
		//Get existing open window or if none, make a new one:
		RelationMatrix window = (RelationMatrix)EditorWindow.GetWindow(typeof(RelationMatrix));
		window.Show();
	}
	
	void OnGUI()
	{
		//Setzt fest wieviele Interaktionsmethoden und Actions es überhaupt gibt
		SetCount();
		SetToggleLists();
		CenterAlign.alignment = TextAnchor.MiddleCenter;
		LeftAlign.alignment = TextAnchor.MiddleLeft;
		
		EditorGUILayout.LabelField("RelationMatrix - ActionFramework",CenterAlign);
		EditorGUILayout.Separator();		
		EditorGUILayout.Space();

		DrawAllElements();
		MethodsWithActions.Clear();

	}
	
	public void SetToggleLists()
	{
	
		for(int i = 0; i < MethodsWithActions.Count; i++)
		{
			MethodsWithActions[i].ReturnInteractionMethod().GetAllActionsOnObjs();
			List<bool> ThisMethodsState = new List<bool>();
			for ( int j = 0; j < AllActions.Count; j++ )
			{
				bool NewElem = false;
				if(MethodsWithActions[i].GetAllActions().Contains(AllActions[j]) && MethodsWithActions[i].ReturnInteractionMethod() != AllActions[j])
				{
					NewElem = true;
				}
				else
				{
					NewElem = false;
				}
				ThisMethodsState.Add (NewElem);
			}
		ToggleInput.Add (ThisMethodsState);
		}
	}
	
	
	
	public void DrawAllElements()
	{
		GUIStyle TextFieldAlign = new GUIStyle(GUI.skin.textField);
		TextFieldAlign.alignment = TextAnchor.MiddleCenter;
		
		ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
				
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.TextField("Actions\\InteractionMethods",TextFieldAlign);
		EditorGUILayout.Space ();
		foreach(Action A in AllActions)
		{
			EditorGUILayout.LabelField(A.GetType().ToString().Replace("FAR.",""),CenterAlign);
			EditorGUILayout.Space();
		}
		EditorGUILayout.Space();
		EditorGUILayout.TextArea("",TextFieldAlign);
		
		EditorGUILayout.EndVertical();
		
		
		for(int i = 0; i < MethodsWithActions.Count; i++)
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField(MethodsWithActions[i].ReturnInteractionMethod().GetType().ToString().Replace("FAR.",""),LeftAlign);//CenterAlign);
			EditorGUILayout.Space();
			MethodsWithActions[i].ReturnInteractionMethod().GetAllActionsOnObjs();
			
			for ( int j = 0; j < AllActions.Count; j++ )
			{
				if(MethodsWithActions[i].GetAllActions().Contains(AllActions[j]) && MethodsWithActions[i].ReturnInteractionMethod() != AllActions[j])
				{
				

				
					EditorGUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();

					//Mit TEMP prüfen wir ob diese ToggleBox ihren Wert geändert hat
					bool Temp = EditorGUILayout.Toggle(ToggleInput[i][j]);

					//Wird aufgerufen wenn diese Aktion Listener ist.
					//Ist ToggleInput hier also "FALSE" müssen wir den Listener entfernen
					//IF-Beschreibung: War die ToggleBox auf TRUE und ist nun FALSE...
					if(ToggleInput[i][j] == true && Temp == false)
					{
						//...setzen wir sie auf FALSE
						//Die ToggleBox wurde abgeschaltet.
						ToggleInput[i][j] = false;
						//Hier können wir eine Aktion entfernen
						
						//Easiest way to remove an Action?
						//Remove its Listener from the ListenersList
						MonoBehaviour tmp = (MonoBehaviour) AllActions[j];
						MethodsWithActions[i].GetAllListeners().Remove(tmp.gameObject);
						

						//And don't forget to remove itself from the Actions-List
						MethodsWithActions[i].GetAllActions().Remove(AllActions[j]);
						
						
						//ABER: Haben wir ein Objekt mit 2 oder mehr Actions entfernt müssen auch diese
						//ToggleBoxes umgestellt werden
						//Wir nehmen das Objekt der entfernten Action...
						tmp = (MonoBehaviour)AllActions[j];
						GameObject Removed = tmp.gameObject;
						//...und alle seine Skripte.
						List<Action> ActionsOnRemObj = new List<Action>();
						//Wir prüfen welche davon Actions waren...
						foreach(MonoBehaviour MB in Removed.GetComponents<MonoBehaviour>())
						{
							if(MB is Action)
							{
								MethodsWithActions[i].ReturnInteractionMethod().Actions.Remove((Action)MB);
								ActionsOnRemObj.Add ((Action)MB);
							}
						}
						//...und vergleichen.
						/*
							War die Action nicht die entfernte Action und dennoch am selben Objekt wie sie,
							so muss sie mit-gelöscht werden.
							Genauso muss ihre ToggleBox mit-abgeschaltet werden.
						*/
						foreach(Action A in AllActions)
						{
							if(A != AllActions[j] && ActionsOnRemObj.Contains(A))
							{
								ToggleInput[i][AllActions.IndexOf(A)] = false;
								MethodsWithActions[i].GetAllActions().Remove(A);
							}
						}
						
					}


					EditorGUILayout.EndHorizontal();
				}
				//Complex InteractionMethods
				else if(MethodsWithActions[i].ReturnInteractionMethod() == AllActions[j])
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					
					//EditorGUILayout.Toggle(false);
					EditorGUILayout.LabelField("       -SAME-");

					
					EditorGUILayout.EndHorizontal();
				}
				else
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					
					//Mit TEMP unterscheiden wir ob der Button auch vorher schon auf einem bestimmten Wert stand
					
					/*if(HelperFunc.CheckEventCompatibility(MethodsWithActions[i],AllActions[j],false))
					{
						GUI.enabled = true;
					}
					else
					{
						GUI.enabled = false;
					}*/
					
					bool Temp = EditorGUILayout.Toggle(ToggleInput[i][j]);
					GUI.enabled = true;
					
					
					EditorGUILayout.EndHorizontal();
					
					//Wird aufgerufen wenn diese Aktion kein Listener ist
					//Wird sie auf "TRUE" gesetzt müssen wir einen Listener hinzufügen
					//IF-Beschreibung: Falls der Button auf FALSE stand und nun TRUE ist
					//===> Er wurde gedrückt
					if(ToggleInput[i][j] == false && Temp == true)
					{

						//Hier werden Aktionen hinzugefügt
						//Einfachster Weg: Einfach Listener einfügen
						
						/*
							Hier müssen wir prüfen ob Sendable- und ReceivableEvents kompatibel sind.
							Das heißt: Ist wenigstens ein ReceivableEvent jeder Action in den Sendables
							enthalten?
						*/
							
						ToggleInput[i][j] = true;
						
						//Ist dieses Objekt noch nicht als Listener enthalten....
						MonoBehaviour tmp = (MonoBehaviour)AllActions[j];
						if(!MethodsWithActions[i].GetAllListeners().Contains(tmp.gameObject))
						{
							//...fügen wir es hinzu....
							MethodsWithActions[i].GetAllListeners().Add (tmp.gameObject);
							//...und prüfen bei jeder Aktion...
							foreach(Action A in AllActions)
							{
								//...ob sie auch an diesem Objekt hing
								if(((MonoBehaviour)A).gameObject == tmp.gameObject && A!=AllActions[j])
								{

									//Jede Action A die das erfüllt, muss ihre ToggleBox ebenfalls auf TRUE setzen, da sie 
									//mit dem GameObject hinzugefügt wurde
									ToggleInput[i][AllActions.IndexOf(A)] = true;


								}
							}
						}
						
						
					}
				}
				EditorGUILayout.Space();
			}
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(MethodsWithActions[i].ReturnInteractionMethod().ReturnName().Replace("(UnityEngine.GameObject)",""),LeftAlign);//CenterAlign);
			
			EditorGUILayout.EndVertical();
		}
		
		
		EditorGUILayout.BeginVertical();
		EditorGUILayout.TextArea("",TextFieldAlign);
		EditorGUILayout.Space ();
		foreach(Action A in AllActions)
		{
			char[] text = A.ToString().ToCharArray();
			char[] NewArr = new char[10];
			for( int i = 0; i < text.Length; i++ )
			{
				if ( text[i] == '(' )
				{
					NewArr = new char[i-1];
					for ( int j = 0; j < i-1; j++ )
					{
						NewArr[j] = text[j];
					}
				}
				
			}
			string temp = new string(NewArr);
			EditorGUILayout.LabelField(temp,CenterAlign);			
			EditorGUILayout.Space();
		}
		EditorGUILayout.Space();
		EditorGUILayout.TextArea("MethodObjects\\ActionObjects",TextFieldAlign);
		
		EditorGUILayout.EndVertical();
		


		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		EditorGUILayout.TextArea("Keep in mind: A grayed out toggle-box is the result of incompatible EventTypes."
								+ " This means that your Action's ReceivableEvents do not contain a single match to your InteractionMethod's SendableEvents",TextFieldAlign);
		
		EditorGUILayout.EndScrollView();
		
	}
	
	public List<Action> GetAllActionsListening(InteractionMethod IM)
	{
		List<Action> AllListeningActions = new List<Action>();
		
		foreach(GameObject GO in IM.Listeners)
		{
			List<Action> ObjectActions = new List<Action>();//GetAllActionsOnObjs(GO);
			
			MonoBehaviour[] ScriptsOnObj = GO.GetComponents<MonoBehaviour>();
			foreach(MonoBehaviour temp in ScriptsOnObj)
			{
				if(temp is Action)
				{
					ObjectActions.Add ((Action)temp);
					AllListeningActions.Add ((Action)temp);
				}
			}
		}
		
		return AllListeningActions;
	}
	
	
	
	public void SetCount()
	{
		List<InteractionMethod> IMs = new List<InteractionMethod>(GameObject.FindObjectsOfType<InteractionMethod>());
		List<Action> ACs = new List<Action>();
		foreach(MonoBehaviour MB in GameObject.FindObjectsOfType<MonoBehaviour>())
		{
			if(MB is Action)
			{
				ACs.Add((Action)MB);
			}
		}
		AllActions = ACs;
		
		foreach(InteractionMethod IM in IMs)
		{
			if(IM is IInteractionMethod)
			{
				IInteractionMethod I = (IInteractionMethod) IM;
				MethodsWithActions.Add(I);
				I.GetAllActions().Clear();
				foreach(MonoBehaviour MB in IM.gameObject.GetComponents<MonoBehaviour>())
				{
					if(MB is Action)
					{
						I.AddAction((Action)MB);
					}
				}
			}
		}

	}

	

	

}
