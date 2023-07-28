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

    public Transform lookForwardOffset;

    private ARAnchor anchorData;
    private Vector3 prevRotation = Vector3.zero;
    private Vector3 prePosition = Vector3.zero;

    private Vector3 _ARKitFaceRotation = Vector3.zero;
    private Vector3 _ARKitFacePosition = Vector3.zero;

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

        // transform text.
        //t_text = GetComponent<Text>();

        // ==== print log about model's blendshape number.====

        //var blendShapeCount = faceMeshRenderer.sharedMesh.blendShapeCount;

        //for(var i = 0; i < blendShapeCount; i++){
        //    var blendShapeName = faceMeshRenderer.sharedMesh.GetBlendShapeName(i);
        //    Debug.Log($"{i} : {blendShapeName}");
        //}

        // ====================================================


        // 위치 초기화
        lookForwardOffset.position = new Vector3(0f, 0f, 0f);

        // 회전 초기화 (Quaternion.identity는 회전이 적용되지 않은 상태를 나타냅니다)
        lookForwardOffset.rotation = Quaternion.identity;

        // 스케일 초기화
        lookForwardOffset.localScale = new Vector3(1f, 1f, 1f);


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
        
        _ARKitFaceRotation = _arFace.transform.localEulerAngles;
        _ARKitFacePosition = _arFace.transform.position;
        

        // ===Apply Person's Rotation To Character===
        //var rotation = _arFace.transform.localRotation;
        //var rot = new Vector3((rotation.eulerAngles.y - prevRotation.y) * -1.0f, (rotation.eulerAngles.z - prevRotation.z) * -1.0f, (rotation.eulerAngles.x - prevRotation.x) * 1.0f);
        //headJoint.transform.Rotate(rot);

        //prevRotation = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);

        // ==== TEST ====
        //Quaternion result_quat = Quaternion.Euler(new Vector3(current_rot.x - offset_rot.x, current_rot.y - offset_rot.y, current_rot.z - offset_rot.z));

        //Vector3 result_rot = result_quat.eulerAngles;

        //headJoint.localRotation = Quaternion.Euler(result_rot.y * -1.0f, result_rot.z * -1.0f, result_rot.x * 1.0f);
        //headJoint.localRotation = Quaternion.Euler(offset_rot);

        // ===Apply Person's Translation To Character===

        var position = _arFace.transform.position;
        var trans = new Vector3((position.x - prePosition.x) * -0.2f, (position.y - prePosition.y) * 0.2f, (position.z - prePosition.z) * -0.3f);
        model.transform.Translate(trans);

        prePosition = new Vector3(position.x, position.y, position.z);


    }

    public void changeOffset()
    {
        //lookForwardOffset.localEulerAngles = headJoint.transform.localEulerAngles;
        foreach (ARFace face in _arFaceManager.trackables)
        {
            _offsetObj.transform.localRotation = face.transform.rotation;

            break;
        }
        
    }

    public void initialize_lookforward()
    {
        changeOffset();
        headJoint.localRotation = Quaternion.identity;
        
    }

    public void callBack_lookforward()
    {
        _offsetObj.transform.localRotation = Quaternion.identity;
        //foreach (ARFace face in _arFaceManager.trackables)
        //{
        //    var rot = new Vector3(face.transform.rotation.eulerAngles.y * -1f, face.transform.rotation.eulerAngles.z * -1f, face.transform.rotation.eulerAngles.x * 1f);
        //    headJoint.localRotation = Quaternion.Euler(rot);

        //    break;
        //}

    }

    void Update()
    {
        Apply();                            // Apply BlendShape Coefficient to Character BlendShape Key.
        //Print_Transformation_Data();        // Print Dev data.
    }   

    private void Apply()
    {
        ApplyEyeBlink();
        ApplyEyeMovement();
        ApplyLipO();

        //Print_Transformation_Data();        // 왜 여기있을 때 되고 아래있으면 안되는건데 ㅅ

        ApplyTransformation();
        //Print_Transformation_Data();        // 여기 있으면 회전은 작동.
        Print_test();
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
        Vector3 current_rot = new Vector3(_ARKitFaceRotation.x, _ARKitFaceRotation.y, _ARKitFaceRotation.z);
        Vector3 offset_rot = new Vector3(_offsetObj.transform.localEulerAngles.x, _offsetObj.transform.localEulerAngles.y, _offsetObj.transform.localEulerAngles.z);



        Quaternion result_quat = Quaternion.Euler(new Vector3(current_rot.x - offset_rot.x, current_rot.y - offset_rot.y, current_rot.z - offset_rot.z));

        Vector3 result_rot = result_quat.eulerAngles;

        headJoint.localRotation = Quaternion.Euler(result_rot.y * -1.0f, result_rot.z * -1.0f, result_rot.x * 1.0f);
        

    }

    private void Print_test()
    {
        t_text.text = "====ARKit Face Rot Test====\n";
        t_text.text += string.Format("x: {0:F2} y: {1:F2} z: {2:F2}\n", _ARKitFaceRotation.x, _ARKitFaceRotation.y, _ARKitFaceRotation.z);
        t_text.text += string.Format("x: {0:F2} y: {1:F2} z: {2:F2}\n", _offsetObj.transform.localEulerAngles.x, _offsetObj.transform.localEulerAngles.y, _offsetObj.transform.localEulerAngles.z);
    }

    private void Print_Transformation_Data()
    {
        

        //foreach (ARFace face in _arFaceManager.trackables)
        //{

        //    Transform ARKitHead_T = face.transform;

        //    // Apply Person's Rotation To Character.       
        //    Quaternion current_rot = ARKitHead_T.rotation;

        //    Transform CharacterHead_T = headJoint.transform;


        //    // Print text About Person's head, Character's head Pos & Rot.
        //    t_text.text = "====person's pos and rot====\n";
        //    string str1 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", ARKitHead_T.position.x, ARKitHead_T.position.y, ARKitHead_T.position.z);
        //    string str2 = string.Format("rx: {0:F2} ry: {1:F2} rz: {2:F2}", ARKitHead_T.rotation.eulerAngles.x, ARKitHead_T.rotation.eulerAngles.y, ARKitHead_T.rotation.eulerAngles.z);
        //    t_text.text += str1 + "\n" + str2;

        //    string str3 = string.Format("====character_rotation====");
        //    //string str4 = string.Format("ry: {0:F2} rz: {1:F2} rx: {2:F2}", CharacterHead_T.localRotation.eulerAngles.x, CharacterHead_T.localRotation.eulerAngles.y, CharacterHead_T.localRotation.eulerAngles.z);
        //    string str4 = string.Format("ry: {0:F2} rz: {1:F2} rx: {2:F2}", headJoint.localEulerAngles.x, headJoint.localEulerAngles.y, headJoint.localEulerAngles.z);
        //    t_text.text += "\n" + str3 + "\n" + str4;

        //    string str5 = string.Format("====character_position====");
        //    string str6 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", model.transform.localPosition.x, model.transform.localPosition.y, model.transform.localPosition.z);
        //    t_text.text += "\n" + str5 + "\n" + str6;


        //    //lookForwardOffset = ARKitHead_T.rotation.eulerAngles;
        //    string str7 = string.Format("====offset rotation====");
        //    string str8 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", lookForwardOffset.localRotation.eulerAngles.x, lookForwardOffset.localRotation.eulerAngles.y, lookForwardOffset.localRotation.eulerAngles.z);
        //    //string str8 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", lookForwardOffset.x, lookForwardOffset.y, lookForwardOffset.z);
        //    t_text.text += "\n" + str7 + "\n" + str8;

        //    //t_text.text = "====person's pos and rot====\n";
        //    //string str1 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", lookForwardOffset.x, lookForwardOffset.y, lookForwardOffset.z);
        //    ////string str2 = string.Format("rx: {0:F2} ry: {1:F2} rz: {2:F2}", ARKitHead_T.rotation.eulerAngles.x, ARKitHead_T.rotation.eulerAngles.y, ARKitHead_T.rotation.eulerAngles.z);
        //    //t_text.text += str1;// + "\n" + str2;
        //}
        ////t_text.text = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", lookForwardOffset.x, lookForwardOffset.y, lookForwardOffset.z);
    }



}
