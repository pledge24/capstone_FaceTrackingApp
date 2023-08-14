using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SetDebug : MonoBehaviour
{
    public Camera VRCamera;

    public Camera ARCamera;

    private bool status;

    private const float WAITTIME = 2;

    private float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (VRCamera == null)
            VRCamera = GameObject.Find("VS Phone Screen").GetComponent<Camera>();
        if (ARCamera == null)
            ARCamera = GameObject.Find("AR Camera").GetComponent<Camera>();
        status = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length > 3)
        {
            if (currentTime >= WAITTIME)
            {
                currentTime = 0;
                status = !status;
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }

        if (status)
        {
            ARCamera.rect = new Rect(0, 0, 0.5f, 1f);
            VRCamera.rect = new Rect(0.5f, 0, 0.5f, 1f);
        }
        else
        {
            VRCamera.rect = new Rect(0, 0, 1, 1);
            ARCamera.rect = new Rect(-1, 0, 1f, 1f);
        }
    }
}
