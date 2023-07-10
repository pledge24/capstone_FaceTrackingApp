using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Text : MonoBehaviour
{
    private GameObject data;
    public TextMeshProUGUI txt;
    // Start is called before the first frame update
    void Start()
    {
        data = GameObject.Find("HumanBodyTracker");
        txt = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = data.GetComponent<ARComponent>().HeadPosition;
        Vector3 rot = data.GetComponent<ARComponent>().HeadRotation;
        GetComponent<TMP_Text>().text = pos.ToString() + "\n" + rot.ToString();
        string posT = "X: " + string.Format("{0:0.000}", pos.x) + "\nY: " + string.Format("{0:0.000}", pos.y)
            + "\nZ: " + string.Format("{0:0.000}", pos.x);
        string rotT = "RX: " + string.Format("{0:0.000}", rot.x) + "\nRY: " + string.Format("{0:0.000}", rot.y)
            + "\nRZ: " + string.Format("{0:0.000}", rot.z);
        txt.text = posT + "\n" + rotT;
    }
}
