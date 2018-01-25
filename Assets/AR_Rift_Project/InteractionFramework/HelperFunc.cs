using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FAR{

    public static class HelperFunc {
    
        //THIS
        /// <summary>
        /// Returns a string-array with all public methods contained in this script
        /// and all its Instances
        /// </summary>
        /// <returns>The methods in script.</returns>
        /// <param name="Script">Script.</param>
        public static string[] GetMethodsInScript(MonoBehaviour Script)
        {
            
            
            //If no script is attached return empty string array
            if (Script == null) 
            {
                return new string[0];
            }

            MethodInfo[] methodInfos = Script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);//BindingFlags.Public | BindingFlags.Static);
            
            /*Debug.Log("Public Static: --------------------");
            
            foreach(MethodInfo MI in methodInfos)
            {
                Debug.Log(MI.Name);
            }*/
            
            string[] Methods = new string[methodInfos.Length];
            
            for( int i = 0; i < methodInfos.Length; i++ )
            {
                Methods[i] = methodInfos[i].Name;
            }
            
            
            return Methods;
        }
        
       
		
		public static void AddMeToAllInteractionMethods(Action A)
		{
			List<InteractionMethod> AllInteractionMethods = new List<InteractionMethod>();
			
			GameObject[] AllGameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
			
			foreach(GameObject G in AllGameObjects)
			{
				MonoBehaviour[] AllMonoBehaviours = G.GetComponents<MonoBehaviour>();
				
				foreach(MonoBehaviour MB in AllMonoBehaviours)
				{
					if(MB is InteractionMethod)
					{
						AllInteractionMethods.Add ((InteractionMethod)MB);
					}	
				}
			}
			
			foreach(InteractionMethod IM in AllInteractionMethods)
			{
				MonoBehaviour B = (MonoBehaviour)A;
				IM.Listeners.Add(B.gameObject);
			}
			
			
		}


		public static void RemoveMeFromAllInteractionMethods(GameObject A)
		{
			List<InteractionMethod> AllInteractionMethods = new List<InteractionMethod>();

			MonoBehaviour[] PossibleActions = A.GetComponents<MonoBehaviour> ();
			List<Action> ActionsOnObj = new List<Action> ();
			foreach (MonoBehaviour M in PossibleActions) 
			{
				if(M is Action)
				{
					ActionsOnObj.Add ((Action)M);
				}
			}
			
			GameObject[] AllGameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
			
			foreach(GameObject G in AllGameObjects)
			{
				MonoBehaviour[] AllMonoBehaviours = G.GetComponents<MonoBehaviour>();
				
				foreach(MonoBehaviour MB in AllMonoBehaviours)
				{
					if(MB is InteractionMethod)
					{
						AllInteractionMethods.Add ((InteractionMethod)MB);
					}	
				}
			}
			
			foreach(InteractionMethod IM in AllInteractionMethods)
			{
				foreach(Action AC in ActionsOnObj)
				{
					if(IM.Actions.Contains(AC))
					{
						IM.Actions.Remove(AC);
					}
				}
			}
			
			
		}

		

		public static Vector3 DecreaseMyDistanceToObjBy(GameObject Me,GameObject Anchor,float Amount)
		{
			Vector3 direction = Vector3.Normalize(Me.transform.position - Anchor.transform.position);
			
			float distance = Vector3.Distance(Me.transform.position,Anchor.transform.position);
			
			Vector3 NewPos = Anchor.transform.position + (direction * (distance - Amount));
			
			return NewPos;
		}
		public static Vector3 IncreaseMyDistanceToObjBy(GameObject Me,GameObject Anchor,float Amount)
		{
			Vector3 direction = Vector3.Normalize(Me.transform.position - Anchor.transform.position);
			float distance = Vector3.Distance(Me.transform.position,Anchor.transform.position);
			Vector3 NewPos = Anchor.transform.position + (direction * (distance + Amount));

			return NewPos;
		}

    }


}
