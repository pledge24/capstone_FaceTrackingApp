using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;      // ARFace, ARFaceManager.
using UnityEngine.XR.ARKit;             // ARKitFaceSubsystem. 

public class ARFaceBlendShapeVisualizerV2 : MonoBehaviour
{
    private const float CoefficientValueScale = 100f;

    public SkinnedMeshRenderer faceMeshRenderer;

    private Renderer[] _characterRenderers;

    private ARFace _arFace;
    private ARFaceManager _arFaceManager;
    private ARKitFaceSubsystem _arKitFaceSubsystem;

    // ==== BlendShape Index Variable ====

    private const int BlendShapeIndexLeftEyeBlink = 6;
    private const int BlendShapeIndexRightEyeBlink = 7;

    private const int BlendShapeIndexLookUp = 14;
    private const int BlendShapeIndexLookDown = 15;
    private const int BlendShapeIndexLookLeft = 16;
    private const int BlendShapeIndexLookRight = 17;

    private const int BlendShapeIndexLipO = 4;

    private readonly Dictionary<ARKitBlendShapeLocation, float> _arKitBlendShapeValueTable
        = new Dictionary<ARKitBlendShapeLocation, float>();

    // ======================================

    [SerializeField]
    private Transform headJoint;

    [SerializeField]
    private Transform model;

    private ARAnchor anchorData;
    private Vector3 prevRotation = Vector3.zero;
    private Vector3 prePosition = Vector3.zero;
    private Transform T;

    public Text t_text;

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

        // transform text.
        t_text = GetComponent<Text>();

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
        Transform ARKitHead_T = _arFace.transform;

        // ===Apply Person's Rotation To Character===       
        Quaternion current_rot = ARKitHead_T.rotation;

        Transform CharacterHead_T = headJoint.transform;
        current_rot = Quaternion.Euler(new Vector3(current_rot.eulerAngles.y * -1f, current_rot.eulerAngles.z * -1f, current_rot.eulerAngles.x * 1f));
        CharacterHead_T.localRotation = current_rot;

        //float Y_rot = current_rot.eulerAngles.y < 0 ? 
        //model.transform.localRotation = Quaternion.Euler(new Vector3(0,  180f + current_rot.eulerAngles.y, 0));
        // ===Apply Person's Translation To Character===

        //var position = _arFace.transform.position;
        //var tran = new Vector3(position.x - prePosition.x, position.y - prePosition.y, position.z - prePosition.z);
        //model.transform.Translate(tran * -0.5f);

        //prePosition = new Vector3(position.x, position.y, position.z);


    }

    void Update()
    {
        Apply();                            // Apply BlendShape Coefficient to Character BlendShape Key.    
        Print_Transformation_Data();        // Print Dev data.
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

    private void Print_Transformation_Data()
    {
        
        foreach (ARFace face in _arFaceManager.trackables)
        {

            Transform ARKitHead_T = face.transform;

            // Apply Person's Rotation To Character.       
            Quaternion current_rot = ARKitHead_T.rotation;

            Transform CharacterHead_T = headJoint.transform;


            // Print text About Person's head, Character's head Pos & Rot.
            t_text.text = "====person's pos and rot====\n";
            string str1 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", ARKitHead_T.position.x, ARKitHead_T.position.y, ARKitHead_T.position.z);
            string str2 = string.Format("rx: {0:F2} ry: {1:F2} rz: {2:F2}", ARKitHead_T.rotation.eulerAngles.x, ARKitHead_T.rotation.eulerAngles.y, ARKitHead_T.rotation.eulerAngles.z);
            t_text.text += str1 + "\n" + str2;

            string str3 = string.Format("====character_rotation====");
            string str4 = string.Format("ry: {0:F2} rz: {1:F2} rx: {2:F2}", CharacterHead_T.localRotation.eulerAngles.x, CharacterHead_T.localRotation.eulerAngles.y, CharacterHead_T.localRotation.eulerAngles.z);
            t_text.text += "\n" + str3 + "\n" + str4;

            string str5 = string.Format("====character_position====");
            string str6 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", model.transform.localPosition.x, model.transform.localPosition.y, model.transform.localPosition.z);
            t_text.text += "\n" + str5 + "\n" + str6;

        }
    }
 


}
