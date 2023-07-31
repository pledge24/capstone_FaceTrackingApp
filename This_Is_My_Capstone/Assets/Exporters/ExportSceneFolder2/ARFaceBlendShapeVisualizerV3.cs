using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;      // ARFace, ARFaceManager.
using UnityEngine.XR.ARKit;             // ARKitFaceSubsystem. 

public class ARFaceBlendShapeVisualizerV3 : MonoBehaviour
{
    private const float CoefficientValueScale = 100f;

    public SkinnedMeshRenderer faceMeshRenderer;

    private Renderer[] _characterRenderers;

    private ARFace _arFace;
    private ARFaceManager _arFaceManager;
    private ARKitFaceSubsystem _arKitFaceSubsystem;

    // ==== BlendShape Index Variable ====

    private const int BlendShapeIndexLipO = 4;

    private const int BlendShapeIndexLeftEyeBlink = 6;
    private const int BlendShapeIndexRightEyeBlink = 7;

    private const int BlendShapeIndexLookUp = 14;
    private const int BlendShapeIndexLookDown = 15;
    private const int BlendShapeIndexLookLeft = 16;
    private const int BlendShapeIndexLookRight = 17;

    private readonly Dictionary<ARKitBlendShapeLocation, float> _arKitBlendShapeValueTable
        = new Dictionary<ARKitBlendShapeLocation, float>();

    // ======================================

    [SerializeField]
    private Transform headJoint;

    [SerializeField]
    private Transform model;

    private Vector3 _ARKitFaceRotation = Vector3.zero;
    private Vector3 _ARKitFacePosition = Vector3.zero;
    private Vector3 prevRotation = Vector3.zero;
    private Vector3 prePosition = Vector3.zero;

    public Text t_text;

    public GameObject _offsetObj;

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

        // ==== print log about model's blendshape number.====

        //var blendShapeCount = faceMeshRenderer.sharedMesh.blendShapeCount;

        //for(var i = 0; i < blendShapeCount; i++){
        //    var blendShapeName = faceMeshRenderer.sharedMesh.GetBlendShapeName(i);
        //    Debug.Log($"{i} : {blendShapeName}");
        //}

        // ====================================================

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
        UpdateTransformation();
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

    private void UpdateTransformation()
    {
        // ===Update Values About ARKitFace===
        _ARKitFaceRotation = _arFace.transform.localEulerAngles;
        _ARKitFacePosition = _arFace.transform.position;
    }

    public void changeOffset()
    {
     
        foreach (ARFace face in _arFaceManager.trackables)
        {
            _offsetObj.transform.localRotation = face.transform.rotation;

            break;
        }
        
    }

    public void initialize_lookforward()
    {
        changeOffset();
    }

    public void callBack_lookforward()
    {
        _offsetObj.transform.localRotation = Quaternion.identity;
       
    }

    void Update()
    {
        Apply();                            // Apply BlendShape Coefficient to Character BlendShape Key.
        Print_Transformation_Data();        // Print Dev data.
    }   

    private void Apply()
    {
        // Apply BlendShapeCoefficient To Character's Face.
        ApplyEyeBlink();
        ApplyEyeMovement();
        ApplyLipO();

        // Apply Transformation To Character Transformation.
        ApplyTransformation();

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

    private void ApplyTransformation()
    {
        // Apply Rotation To Character Head Joint.
        Vector3 current_rot = new Vector3(_ARKitFaceRotation.x, _ARKitFaceRotation.y, _ARKitFaceRotation.z);
        Vector3 offset_rot = new Vector3(_offsetObj.transform.localEulerAngles.x, _offsetObj.transform.localEulerAngles.y, _offsetObj.transform.localEulerAngles.z);

        Quaternion result_quat = Quaternion.Euler(new Vector3(current_rot.x - offset_rot.x, current_rot.y - offset_rot.y, current_rot.z - offset_rot.z));

        Vector3 result_rot = result_quat.eulerAngles;

        headJoint.localRotation = Quaternion.Euler(result_rot.y * -1.0f, result_rot.z * -1.0f, result_rot.x * 1.0f);

        // Apply Rotation And Translation To Character Model.
        //model.transform.localRotation = Quaternion.Euler(0, result_rot.y * 1f, 0);

        //var position = _ARKitFacePosition;
        //var trans = new Vector3((position.x - prePosition.x) * -0.4f, (position.y - prePosition.y) * 0.2f, (position.z - prePosition.z) * -0.4f);
        //model.transform.Translate(trans);

        //prePosition = new Vector3(position.x, position.y, position.z);
    }

    private void Print_Transformation_Data()
    { 

        // Print text About Person's head, Character's head Pos & Rot.
        t_text.text = "====person's pos and rot====\n";
        string str1 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", _ARKitFacePosition.x, _ARKitFacePosition.y, _ARKitFacePosition.z);
        string str2 = string.Format("rx: {0:F2} ry: {1:F2} rz: {2:F2}", _ARKitFaceRotation.x, _ARKitFaceRotation.y, _ARKitFaceRotation.z);
        t_text.text += str1 + "\n" + str2;

        string str3 = string.Format("====character_rotation====");
        string str4 = string.Format("ry: {0:F2} rz: {1:F2} rx: {2:F2}", headJoint.localEulerAngles.x, headJoint.localEulerAngles.y, headJoint.localEulerAngles.z);
        t_text.text += "\n" + str3 + "\n" + str4;

        string str5 = string.Format("====character_position====");
        string str6 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", model.transform.localPosition.x, model.transform.localPosition.y, model.transform.localPosition.z);
        t_text.text += "\n" + str5 + "\n" + str6;

        string str7 = string.Format("====offset rotation====");
        string str8 = string.Format("rx: {0:F2} ry: {1:F2} rz: {2:F2}", _offsetObj.transform.localEulerAngles.x, _offsetObj.transform.localEulerAngles.y, _offsetObj.transform.localEulerAngles.z);
        t_text.text += "\n" + str7 + "\n" + str8;
 
    }



}
