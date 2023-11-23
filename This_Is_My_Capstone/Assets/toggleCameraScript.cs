using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleCameraScript : MonoBehaviour
{
    bool wideCameraMode = false;

    [SerializeField] GameObject defaultCamera;
    [SerializeField] GameObject wideShotCamera;

    public void toggleMode()
    {
        wideCameraMode = !wideCameraMode;
        wideShotCamera.SetActive(wideCameraMode); 
        defaultCamera.SetActive(!wideCameraMode);
    }
}
