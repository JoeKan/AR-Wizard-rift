using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkPoseSource : MonoBehaviour {

    string[] wandCoordString;
    float[] wandCoords;
    const short recieveMsg = 111;
    Vector3 wandPosition;
    Quaternion wandRotation;

    // Use this for initialization
    void Start () {

        SetupServer();

        wandCoords = new float[7];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetupServer()
    {
        NetworkServer.Listen(41844);
        NetworkServer.RegisterHandler(recieveMsg, ReceivedMessage);
    }

    public void ReceivedMessage(NetworkMessage netMsg)
    {
        StringMessage rcvString = netMsg.ReadMessage<StringMessage>();
        Debug.Log("Receiving data...");
        wandCoordString = (rcvString.value).Split(' ');

        for (int i = 0; i < wandCoords.Length; i++) {
            wandCoords[i] = Single.Parse(wandCoordString[i]);
            Debug.Log("WandCoords: " + wandCoords[i] + " " + wandCoords[i].GetType());
        }

        wandPosition = new Vector3(wandCoords[0], wandCoords[1], wandCoords[2]);
        wandRotation = new Quaternion(wandCoords[3], wandCoords[4], wandCoords[5], wandCoords[6]);

        this.transform.position = wandPosition;
        this.transform.rotation = wandRotation;
    }
}