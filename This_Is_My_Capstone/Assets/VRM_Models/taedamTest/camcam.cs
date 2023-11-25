using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camcam : MonoBehaviour
{
    private float left = 0.0f;
    // private float right = 1.0f;
    private float bottom = 0.0f;
    // private float top = 1.0f;

    private Camera mainCamera;
    private bool flagcam = true;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    public void AdjustViewport()
    {
        if (flagcam)
        {
            // Set the viewport rect
            mainCamera.rect = new Rect(left, bottom, left, left);
            flagcam = false;
        }
        else
        {
            mainCamera.rect = new Rect(0.02f, 0.02f, 0.22f, 0.22f);
            flagcam = true;
        }
    }
}
