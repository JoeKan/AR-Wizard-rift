using UnityEngine;
using System.Collections;

using System;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.IO;

using UnityEngine.XR.WSA.Input;
using UnityEngine.XR;
using UnityEngine.Windows.Speech;
using System.Linq;
using System.Collections.Generic;

public class RegisterHostMessage : MessageBase
{
    public string dataString;
}

//Vorher NetworkPoseSink jetzt auf UnityPerspektive geändert
public class NetworkPoseSink : MonoBehaviour
{
    private int port = 41845;
    private string id = "holoPose";

    static readonly DateTime StartOfEpoch = new DateTime(1970, 1, 1);

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    float calibrate = 0.0f;

    NetworkClient myClient;
    // RegisterHostMessage msg = new RegisterHostMessage();
    //public const short RegisterHostMsgId = 888;

    string dataString;
    //float rotX, rotY, posZ;

    //Camera m_MainCamera;
    private GameObject cam;
    private const short sendMsgID = 123;

    //private IEnumerator coroutine;

    Vector3 left, right, center, head, leftWorld, rightWorld, offset;
    Matrix4x4 m;
    Camera camMain;

    Quaternion rotationLocal;
    Vector3 positionLocal;

    //Vuforia image send coordinates
    GameObject vuforiaImage;

    // Use this for initialization
    void Start()
    {
        //m_MainCamera = Camera.main;
        cam = GameObject.FindWithTag("MainCamera");
        camMain = Camera.main;

        vuforiaImage = GameObject.FindWithTag("vuforiaImage");

        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
       // InvokeRepeating("SetupClient", 2.0f, 5.0f);
        SetupClient();

        //Create keywords for keyword recognizer
        keywords.Add("Calibrate", () =>
        {
            // action to be performed when this keyword is spoken
            calibrate = 666.666f;
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
            Debug.Log("Voice command activatedd");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (myClient.isConnected == false) return;

         if (!connected) return;

        /*rotX = -1.0f * cam.transform.rotation.x;
        rotY = -1.0f * cam.transform.rotation.y;
        posZ = -1.0f * cam.transform.position.z;

        dataString = "22 serialization::archive 13 8 " + id + " 0 0 " + (ulong)getUbitrackTimeStamp() + " 0 0 0 0 "
                 + rotX + " " + rotY + " " + cam.transform.rotation.z.ToString()
                 + " " + cam.transform.rotation.w.ToString() + " 0 0 " + cam.transform.position.x.ToString() + " " + cam.transform.position.y.ToString()
                 + " " + posZ + " " + ((ulong)getUbitrackTimeStamp() + 100000) + " 1";*/

        positionLocal = InputTracking.GetLocalPosition(XRNode.CenterEye);
        rotationLocal = InputTracking.GetLocalRotation(XRNode.CenterEye);

        /*dataString = cam.transform.position.x.ToString() + " " + cam.transform.position.y.ToString() + " " + cam.transform.position.z.ToString()
            + " " + cam.transform.rotation.x.ToString() + " " + cam.transform.rotation.y.ToString() + " " 
            + cam.transform.rotation.z.ToString() + " " +cam.transform.rotation.w.ToString();*/

        dataString = positionLocal.x.ToString() + " " + positionLocal.y.ToString() + " "
            + positionLocal.z.ToString() + " " + rotationLocal.x.ToString() + " "
            + rotationLocal.y.ToString() + " " + rotationLocal.z.ToString() + " " + rotationLocal.w.ToString() + " "
            + vuforiaImage.transform.localPosition.x.ToString() + " " + vuforiaImage.transform.localPosition.y.ToString() + " "
            + vuforiaImage.transform.localPosition.z.ToString() + " " + vuforiaImage.transform.localRotation.x.ToString() + " "
            + vuforiaImage.transform.localRotation.y.ToString() + " " + vuforiaImage.transform.localRotation.z.ToString() + " "
            + vuforiaImage.transform.localRotation.w.ToString() + " " + calibrate;

        byte[] data = Encoding.ASCII.GetBytes(dataString);

        //transform from local to world coordinates
        left = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.LeftEye)) * InputTracking.GetLocalPosition(XRNode.LeftEye);
        right = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.RightEye)) * InputTracking.GetLocalPosition(XRNode.RightEye);
        head = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.Head)) * InputTracking.GetLocalPosition(XRNode.Head);
        center = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.CenterEye)) * InputTracking.GetLocalPosition(XRNode.CenterEye);

        

        offset = (left - right) * 0.5f;

        m = camMain.cameraToWorldMatrix;
        leftWorld = m.MultiplyPoint(-offset);
        rightWorld = m.MultiplyPoint(offset);

        try
        {
            myClient.Send(sendMsgID, new StringMessage(dataString));
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private ulong getUbitrackTimeStamp()
    {
        ulong millis = (ulong)(DateTime.UtcNow - StartOfEpoch).Ticks / TimeSpan.TicksPerMillisecond;
        return millis * 1000000;
    }

    bool connected = false;

    void SetupClient()
    {
        try
        {
            Debug.Log("start SetupClient");
            if(!connected)
                myClient.Connect("131.159.10.129", port);
           // return myClient.isConnected;
        }

        catch (Exception e)
        {
            Debug.Log(e.ToString());
           // return false;
        }
    }

    public void OnConnected(NetworkMessage netMsg) {
        Debug.Log("Connected to server ");
        connected = true;
    }

    public void OnDisconnected(NetworkMessage netMsg)
    {
        Debug.Log("Disconnected from server ");
        connected = false;
    }

    //method that, when called, stops the client.
    public void StopClient()
    {
        myClient.Disconnect();
    }
}





