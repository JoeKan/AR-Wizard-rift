using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereCalibOrganizer : MonoBehaviour
{
    //public Material VisMaterial;
    public Texture2D areaVisTex;

    Vector3 SpherePosToVector3(float r, float theta, float phi)
    {
        float x = r * Mathf.Sin(theta) * Mathf.Cos(phi);
        float y = r * Mathf.Sin(theta) * Mathf.Sin(phi);
        float z = r * Mathf.Cos(theta);

        return new Vector3(x, y, z);
    }
    Vector3 Vector3ToSpherePos(Vector3 pos)
    {
        float m = pos.magnitude;
        float r = m;
        float theta = Mathf.Acos(pos.z / m);
        float phi = Mathf.Atan(pos.y / pos.x);

        return new Vector3(r, theta, phi);
    }

    GameObject SphereTarget;
    public Transform TargetTransform;

    public GameObject CalibObject, relCalibObject;
    Vector3 relativeCalibPos;

    public GameObject[] AreaPlanes;

    public float Radius;
    public int Depth;
    public float PanelScale;
    public List<Vector3> vData;


    // Use this for initialization
    void Start()
    {
        //ChangeScale(seperationValX, seperationValY, Radius);
        SphereTarget = GameObject.FindGameObjectWithTag("Target");
        init_Sphere(ref vData, Depth);
        CreateSphere();
    }

//    Vector3 n;

    // Update is called once per frame
    void Update()
    {
        if (TargetTransform != null)
        {
            SphereTarget.transform.position = TargetTransform.position;
        }
        relativeCalibPos = SphereTarget.transform.position + (CalibObject.transform.position - TargetTransform.position).normalized * Radius;
        relCalibObject.transform.position = relativeCalibPos;
        //n = Vector3ToSpherePos(relativeCalibPos);
    }


    void OnGUI()
    {
        //GUI.Label(new Rect(100, 100, 100, 100), n.ToString());
    }
    /*void ChangeScale(int x_division, int y_division, float radius)
    {   
        SphereTarget = null;

        seperationValX = x_division;
        seperationValY = y_division;
        Radius = radius;

        int row = 1, line = 1;

        int sumAreas;
        
    }*/


    void subdivide(ref Vector3 v1, ref Vector3 v2, ref Vector3 v3, ref List<Vector3> vData, int depth)
    {
        if (depth == 0)
        {
            vData.Add(v1);
            vData.Add(v2);
            vData.Add(v3);
            return;
        }

        Vector3 v12, v23, v31;

        v12 = (v1 + v2).normalized;
        v23 = (v3 + v2).normalized;
        v31 = (v1 + v3).normalized;
        subdivide(ref v1, ref v12, ref v31, ref vData, depth-1);
        subdivide(ref v2, ref v23, ref v12, ref vData, depth-1);
        subdivide(ref v3, ref v31, ref v23, ref vData, depth-1);
        subdivide(ref v12, ref v23, ref v31, ref vData, depth-1);
    }

    void init_Sphere(ref List<Vector3> vData, int depth)
    {
        float X = 0.525731112119133606f, Z = 0.850650808352039932f;
        Vector3[] tempVData = new Vector3[12] {new Vector3(-X,0,Z), new Vector3(X,0,Z), new Vector3(-X,0,-Z), new Vector3(X,0,-Z),
                              new Vector3(0,Z,X), new Vector3(0,Z,-X), new Vector3(0,-Z,X), new Vector3(0,-Z,-X),
                              new Vector3(Z,X,0), new Vector3(-Z,X,0), new Vector3(Z,-X,0), new Vector3(-Z,-X,0)};
        int[,] TempIData = new int[20,3] {  {0,4,1},{0,9,4},{9,5,4},{4,5,8},
                                            {4,8,1},{8,10,1},{8,3,10},{5,3,8},
                                            {5,2,3},{2,7,3},{7,10,3},{7,6,10},
                                            {7,11,6},{11,0,6},{0,1,6},{6,1,10},
                                            {9,0,11},{9,11,2},{9,2,5},{7,2,11}};
        for (int i = 0; i < 20; i++ ) {
            subdivide(ref tempVData[TempIData[i, 0]], ref tempVData[TempIData[i, 1]], ref tempVData[TempIData[i, 2]], ref vData, depth);
        }
    }
    void CreateSphere()
    {
        GameObject p = GameObject.CreatePrimitive(PrimitiveType.Quad);
        for (int i = 0; i < vData.Count; i++)
        {
            GameObject t = Instantiate(p, (vData[i] + SphereTarget.transform.position).normalized*Radius, Quaternion.LookRotation(-vData[i])) as GameObject;
            t.transform.parent = SphereTarget.transform;
            t.tag = "Panel";
            t.transform.localScale = new Vector3(PanelScale, PanelScale, 1);
            //Material mt = VisMaterial;
            t.GetComponent<Renderer>().material = new Material(Shader.Find("Particles/Additive"));
            t.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.04f, 0.04f, 0.04f, 0.5f));
            t.GetComponent<Renderer>().material.mainTexture = areaVisTex;
        }
        Destroy(p);
        HighlightArea.PanelList = GameObject.FindGameObjectsWithTag("Panel");
    }
}
