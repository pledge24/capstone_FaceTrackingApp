using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARSubsystems;

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
        Vector3 bodypos = data.GetComponent<ARComponent>().HeadPosition;
        //[BodyPosition] - 
        string body = "X: " + string.Format("{0:0.000}", bodypos.x)
            + "\nY: "  + string.Format("{0:0.000}", bodypos.y)
            + "\nZ: " + string.Format("{0:0.000}", bodypos.z);
        txt.text = body;
        foreach(XRHumanBodyJoint i in data.GetComponent<ARComponent>().Joints)
        {
            if (i.index == (int)ARComponent.JointIndices3D.Head ||
                i.index == (int)ARComponent.JointIndices3D.Neck1 ||
                i.index == (int)ARComponent.JointIndices3D.LeftArm ||
                i.index == (int)ARComponent.JointIndices3D.RightArm ||
                i.index == (int)ARComponent.JointIndices3D.LeftForearm ||
                i.index == (int)ARComponent.JointIndices3D.RightForearm ||
                i.index == (int)ARComponent.JointIndices3D.LeftHand ||
                i.index == (int)ARComponent.JointIndices3D.RightHand ||
                i.index == (int)ARComponent.JointIndices3D.LeftUpLeg ||
                i.index == (int)ARComponent.JointIndices3D.RightUpLeg ||
                i.index == (int)ARComponent.JointIndices3D.LeftLeg ||
                i.index == (int)ARComponent.JointIndices3D.RightLeg ||
                i.index == (int)ARComponent.JointIndices3D.LeftFoot ||
                i.index == (int)ARComponent.JointIndices3D.RightFoot)
            {
                string name = System.Enum.GetName(typeof(ARComponent.JointIndices3D), i.index);
                Vector3 pos = i.anchorPose.position;
                Vector3 rot = i.anchorPose.rotation.eulerAngles;
                string type = "[" + name + " Rotation] -";// + string.Format("{0:0.000, 1:0.000, 2:0.000}", pos) + "\t"
                                                 //+ string.Format("{0:0.000, 1:0.000, 2:0.000}", rot);
                string posT = "X: " + string.Format("{0:0.000}", pos.x) + "\tY: " + string.Format("{0:0.000}", pos.y)
                + "\tZ: " + string.Format("{0:0.000}", pos.x);
                string rotT = "RX: " + string.Format("{0:0.000}", rot.x) + "\tRY: " + string.Format("{0:0.000}", rot.y)
                    + "\tRZ: " + string.Format("{0:0.000}", rot.z);
                Debug.Log(type + "\t" + rotT);

            }
            
        }
        //Vector3 pos = data.GetComponent<ARComponent>().HeadPosition;
        //Vector3 rot = data.GetComponent<ARComponent>().HeadRotation;
        //GetComponent<TMP_Text>().text = pos.ToString() + "\n" + rot.ToString();
        
        //txt.text = posT + "\n" + rotT;
    }
}
