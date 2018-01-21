using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlayScript : MonoBehaviour
{
    //if Button "Play" is clicked, load Play Scene
    public void OnMouseButton()
    {
        //if we had a client running (scene Calibrate was opened first) we need to close the client before loading the next scene
        if (SceneManager.GetActiveScene().Equals("Calibrate"))
        {
            NetworkPoseSink poseSink = new NetworkPoseSink();
            poseSink.StopClient();
        }
        SceneManager.LoadScene("Play");
    }

}
