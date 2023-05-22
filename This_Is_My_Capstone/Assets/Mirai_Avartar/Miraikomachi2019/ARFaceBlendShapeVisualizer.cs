using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

public class ARFaceBlendShapeVisualizer : MonoBehaviour
{
    private const float CoefficientValueScale = 100f;

    public SkinnedMeshRenderer faceMeshRenderer;

    private Renderer[] _characterRenderers;

    private ARFace _arFace;
    private ARFaceManager _arFaceManager;
    private ARKitFaceSubsystem _arKitFaceSubsystem;

    private const int BlendShapeIndexLeftEyeBlink = 6;
    private const int BlendShapeIndexRightEyeBlink = 7;

    private const int BlendShapeIndexLookUp = 14;
    private const int BlendShapeIndexLookDown = 15;
    private const int BlendShapeIndexLookLeft = 16;
    private const int BlendShapeIndexLookRight = 17;

    private const int BlendShapeIndexLipO = 4;

    private readonly Dictionary<ARKitBlendShapeLocation, float> _arKitBlendShapeValueTable
        = new Dictionary<ARKitBlendShapeLocation, float>();

    // Start is called before the first frame update
    void Start()
    {
        _characterRenderers = GetComponentsInChildren<Renderer>();
        _arFace = GetComponent<ARFace>();
        _arFaceManager = FindObjectOfType<ARFaceManager>();

        _arKitFaceSubsystem = _arFaceManager.subsystem as ARKitFaceSubsystem;

        SetupARKitBlendShapeTable();

        _arFace.updated += OnFaceUpdated;

        ARSession.stateChanged += OnARSessionStateChanged;
        //var blendShapeCount = faceMeshRenderer.sharedMesh.blendShapeCount;

        //for(var i = 0; i < blendShapeCount; i++)
        //{
        //    var blendShapeName = faceMeshRenderer.sharedMesh.GetBlendShapeName(i);

        //    Debug.Log($"{i} : {blendShapeName}");
            
        //}
    }

    private void OnARSessionStateChanged(ARSessionStateChangedEventArgs args)
    {
        if(args.state > ARSessionState.Ready && _arFace.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
        {
            foreach(var characterRenderer in _characterRenderers)
            {
                characterRenderer.enabled = true;
            }
        }
        else
        {
            foreach(var characterRenderer in _characterRenderers)
            {
                characterRenderer.enabled = false;
            }
        }
    }

    private void SetupARKitBlendShapeTable()
    {
        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeBlinkLeft, 0f);
        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeBlinkRight, 0f);

        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeLookInLeft, 0f);
        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeLookInRight, 0f);
        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeLookOutLeft, 0f);
        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeLookOutRight, 0f);
        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeLookDownLeft, 0f);
        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeLookDownRight, 0f);
        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeLookUpLeft, 0f);
        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.EyeLookUpRight, 0f);

        _arKitBlendShapeValueTable.Add(ARKitBlendShapeLocation.JawOpen, 0f);
    }

    private void OnFaceUpdated(ARFaceUpdatedEventArgs args)
    {
        UpdateArKitBlendShapeValues();
    }

    private void UpdateArKitBlendShapeValues()
    {
        var blendShapeCofficients
            = _arKitFaceSubsystem.GetBlendShapeCoefficients(_arFace.trackableId, Allocator.Temp);

        foreach(var blendShapeCoefficient in blendShapeCofficients)
        {
            var blendShapeLocation = blendShapeCoefficient.blendShapeLocation;

            if (_arKitBlendShapeValueTable.ContainsKey(blendShapeLocation))
            {
                _arKitBlendShapeValueTable[blendShapeLocation] = blendShapeCoefficient.coefficient * CoefficientValueScale;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        Apply();    
    }

    private void Apply()
    {
        ApplyEyeBlink();
        ApplyEyeMovement();
        ApplyLipO();
    }

    private void ApplyEyeBlink()
    {
       var leftBlinkValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeBlinkLeft];
       faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexLeftEyeBlink, leftBlinkValue);

       var rightBlinkValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeBlinkRight];
       faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexRightEyeBlink, rightBlinkValue);
    }

    private void ApplyEyeMovement()
    {
        var leftEyeMovementValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeLookOutLeft];
        faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexLookLeft, leftEyeMovementValue);

        var rightEyeMovementValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeLookInLeft];
        faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexLookRight, rightEyeMovementValue);

        var UpEyeMovementValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeLookUpLeft];
        faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexLookUp, UpEyeMovementValue);

        var downEyeMovementValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeLookDownLeft];       
        faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexLookDown, downEyeMovementValue);
    }

    private void ApplyLipO()
    {
        var lipOValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.JawOpen];
        faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexLipO, lipOValue);
    }
}
