using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCalibrateScript : MonoBehaviour
{
    //if Button "Calibrate" is clicked, load Calibration Scene
    public void OnMouseButton()
    {
        SceneManager.LoadScene("Calibrate");
    }
    
}
