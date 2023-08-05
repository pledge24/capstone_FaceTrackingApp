using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

public class FaceTracker : MonoBehaviour
{
    private const float COEFFICIENTVALUESCALE = 100f;

    [SerializeField] private SkinnedMeshRenderer faceMeshRenderer;

    [SerializeField] private ARFaceManager FaceManager;
    private ARFace face;
    private ARKitFaceSubsystem FaceSubsystem;
    private Transform model;

    private Dictionary<ARKitBlendShapeLocation, float> blendShapes =
        new Dictionary<ARKitBlendShapeLocation, float>();

    void Start()
    {
        face = FaceManager.GetComponent<ARFace>();
        FaceSubsystem = FaceManager.subsystem as ARKitFaceSubsystem;
        face.updated += OnFaceUpdated;
        SetupBlendShapes();
        
    }

    private void SetupBlendShapes()
    {
        foreach (ARKitBlendShapeCoefficient blendShape in FaceSubsystem.GetBlendShapeCoefficients(face.trackableId,
                     Allocator.Temp))
        {
            blendShapes.Add(blendShape.blendShapeLocation, 0f);
        }
    }

    private void OnFaceUpdated(ARFaceUpdatedEventArgs args)
    {
        NativeArray<ARKitBlendShapeCoefficient> blendShapeCoefficients =
            FaceSubsystem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp);
        foreach (ARKitBlendShapeCoefficient blendShape in blendShapeCoefficients)
        {
            ARKitBlendShapeLocation blendShapeLocation = blendShape.blendShapeLocation;
            if (blendShapes.ContainsKey(blendShapeLocation))
            {
                blendShapes[blendShapeLocation] = blendShape.coefficient * COEFFICIENTVALUESCALE;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var value in blendShapes)
        {
            faceMeshRenderer.SetBlendShapeWeight((int)value.Key, value.Value);
        }
    }
}
