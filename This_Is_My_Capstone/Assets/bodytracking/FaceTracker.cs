using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

public class FaceTracker : MonoBehaviour
{
    private const float COEFFICIENTVALUESCALE = 100f;

    [SerializeField] private SkinnedMeshRenderer faceMeshRenderer;

    [SerializeField] private ARFaceManager FaceManager;
    private ARKitFaceSubsystem FaceSubsystem;
    
    private enum Blendshapes
    {
        
        Blink_L = 6,
        Blink_R = 7,
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
