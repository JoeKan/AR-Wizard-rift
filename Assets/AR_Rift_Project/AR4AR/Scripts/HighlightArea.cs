using UnityEngine;
using System.Collections;

public class HighlightArea : MonoBehaviour {

    public static GameObject[] PanelList;
    GameObject currentPanel;
    //int x=0;

	// Update is called once per frame
	void Update () {
        if(PanelList.Length != 0) getPlane();
	}

    void getPlane()
    {
        float shortestDistance = -1;
        GameObject x = null;
        foreach (GameObject i in PanelList)
        {
            float tmp = Vector3.Distance(this.transform.position, i.transform.position);
            if (shortestDistance == -1 || shortestDistance > tmp)
            {
                shortestDistance = tmp;
                x = i;
            }

        }
        foreach (GameObject i in PanelList)
        {
            if (i == x)
            {
                i.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1, 0, 0, 0.2f));
            }
            else
            {
                i.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.04f, 0.04f, 0.04f, 0.5f));
            }
        }
        /*for (int i = 0; i < PanelList.Length; i++)
        {
            float tmp = Vector3.Distance(this.transform.position, PanelList[i].transform.position);
            if (shortestDistance == -1 || shortestDistance > tmp)
            {
                x = i;
            }
            yield return 0;
        }
        for (int i = 0; i < PanelList.Length; i++)
        {
            if (i == x)
            {
                PanelList[i].renderer.material.SetColor("_TintColor", new Color(0.04f, 0, 0, 0.5f));
            }
            else
            {
                PanelList[i].renderer.material.SetColor("_TintColor", new Color(0.04f, 0.04f, 0.04f, 0.5f));
            }
            yield return 0;
        }*/
    }
}
