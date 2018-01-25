using UnityEngine;


public static class Helper
{
	public static GameObject Find (this GameObject gameObject, int requestedID)
	{
		var gameObjectList = Object.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject obj in gameObjectList)
		{
			int id = obj.GetInstanceID ();
			if (id == requestedID) {
				return obj;
			}
		}
		return null;
	}
}


