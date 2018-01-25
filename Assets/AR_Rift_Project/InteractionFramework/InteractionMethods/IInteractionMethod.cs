using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FAR{

public interface IInteractionMethod {


		 ulong CreateTimeStamp();
		
		 void fireEvent(InteractionEvent evt);

		//Fügt direkt zu Actions hinzu
		void AddAction(Action a);
		void RemoveAction(Action a);
		//returns Actions
		List<Action> GetAllActions();
		List<GameObject> GetAllListeners();
		
		//Get Name of the GameObject attached
		string ReturnName();
		
		//Get the current InteractionMethod
		InteractionMethod ReturnInteractionMethod();
		
}

}