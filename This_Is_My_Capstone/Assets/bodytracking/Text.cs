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
        string body = "[BodyPosition] - X: " + string.Format("{0:0.000}", bodypos.x)
            + "\tY: "  + string.Format("{0:0.000}", bodypos.y)
            + "\tZ: " + string.Format("{0:0.000}", bodypos.z);
        Debug.Log(body);
        foreach(XRHumanBodyJoint i in data.GetComponent<ARComponent>().Joints)
        {
            if (i.index == (int)BoneController.Joints.Head ||
                i.index == (int)BoneController.Joints.Neck ||
                i.index == (int)BoneController.Joints.UpperArm_L ||
                i.index == (int)BoneController.Joints.UpperArm_R ||
                i.index == (int)BoneController.Joints.LowerArm_L ||
                i.index == (int)BoneController.Joints.LowerArm_R ||
                i.index == (int)BoneController.Joints.Hand_L ||
                i.index == (int)BoneController.Joints.Hand_R ||
                i.index == (int)BoneController.Joints.UpperLeg_L ||
                i.index == (int)BoneController.Joints.UpperLeg_R ||
                i.index == (int)BoneController.Joints.LowerLeg_L ||
                i.index == (int)BoneController.Joints.LowerLeg_R ||
                i.index == (int)BoneController.Joints.Foot_L ||
                i.index == (int)BoneController.Joints.Foot_R)
            {
                string name = System.Enum.GetName(typeof(BoneController.Joints), i.index);
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
