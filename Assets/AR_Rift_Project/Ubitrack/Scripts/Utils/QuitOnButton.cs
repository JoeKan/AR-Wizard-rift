using UnityEngine;
//using UnityEditor;
using System.Collections;

public class QuitOnButton : MonoBehaviour {

	public KeyCode quitButton = KeyCode.Escape;
	public bool pauseInsteadOfQuitInEditor = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(quitButton)){
			QuitApplicationWithUbitrack(pauseInsteadOfQuitInEditor);
		}
	}

	public static void QuitApplicationWithUbitrack (bool pauseInsteadOfQuitInEditor = false) {

		if(Application.isEditor){
			#if UNITY_EDITOR
			if(pauseInsteadOfQuitInEditor){
				UnityEditor.EditorApplication.isPaused = true;
			}
			else{
				UnityEditor.EditorApplication.isPlaying = false;
			}
			#endif
		}
		else{
			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}
	}
}
