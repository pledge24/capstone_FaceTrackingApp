using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AxisActive : MonoBehaviour
{

    [SerializeField] GameObject VRCamera;
    camcam1 cam_script;
    Text text;
    Transform VRCamera_transform;

    float moveSpeed = 0.03f;

    bool isButtonheld = false;

    public void activeXaxisRotate()
    {
        cam_script.activeXRotate = (cam_script.activeXRotate + 1) % 2;

        if(cam_script.activeXRotate == 1.0)
        {
            text.text = string.Format("Xaxis: ON");
        }
        else 
        {
            text.text = string.Format("Xaxis: OFF");
        }
    }

    public void activeYaxisRotate()
    {
        cam_script.activeYRotate = (cam_script.activeYRotate + 1) % 2;

        if (cam_script.activeYRotate == 1.0)
        {
            text.text = string.Format("Yaxis: ON");
        }
        else
        {
            text.text = string.Format("Yaxis: OFF");
        }
    }

    public void moveUP()
    {
        VRCamera_transform.Translate(0, moveSpeed, 0);
    }

    public void moveDown()
    {
        VRCamera_transform.Translate(0, -moveSpeed, 0);
    }

    private void Start()
    {
        cam_script = VRCamera.GetComponent<camcam1>();
        text = GetComponentInChildren<Text>();
        VRCamera_transform = VRCamera.GetComponent<Transform>();

    }

    
}
