using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;      // ARFace, ARFaceManager.
using UnityEngine.XR.ARKit;             // ARKitFaceSubsystem. 

public class ARFaceBlendShapeVisualizerV4 : MonoBehaviour
{
    private const float CoefficientValueScale = 100f;

    [SerializeField] private SkinnedMeshRenderer faceMeshRenderer;

    private Renderer[] _characterRenderers;

    private ARFace _arFace;
    private ARFaceManager _arFaceManager;
    private ARKitFaceSubsystem _arKitFaceSubsystem;

    // ==== BlendShape Index Variable(Mirai Komachi) ====

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

    [SerializeField] private Transform T_rig;                       // rig transform.
    [SerializeField] private Transform model_root;                  // model transform.
    [SerializeField] private Text text_debug;                       // debug text.
    [SerializeField] private GameObject originalTransCube;          // 얼굴 트랜스폼 원본 값을 저장할 큐브. 

    private Quaternion headRotation;
    private Vector3 originalCubePosition;

    public static Vector3 cubePositionOffset;
    private static Vector3 _ARKitFaceRotation = Vector3.zero;
    private static Vector3 _ARKitFacePosition = Vector3.zero;

    SetPositionOffsetScript script_posOffset;                       // Set Pos external script in Original cube
    SetRotationOffsetScript script_rotOffset;                       // Set Rot external script in Original cube

    // Start is called before the first frame update
    void Start()
    {
        // ======================== Cube Set Code =======================
        originalCubePosition = originalTransCube.transform.position;
        headRotation = T_rig.rotation;

        script_posOffset = originalTransCube.GetComponentInChildren<SetPositionOffsetScript>();
        script_rotOffset = originalTransCube.GetComponentInChildren<SetRotationOffsetScript>();

        // ======================= Default Code ================
        cubePositionOffset = Vector3.zero;
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
        if (args.state > ARSessionState.Ready && _arFace.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
        {
            foreach (var characterRenderer in _characterRenderers)
            {
                characterRenderer.enabled = true;
            }
        }
        else
        {
            foreach (var characterRenderer in _characterRenderers)
            {
                characterRenderer.enabled = false;
            }
        }
    }

    /// <summary>
    /// 블렌드쉐입 테이블 설정하는 함수(블렌드쉐입 로케이션 추가)
    /// </summary>
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

    /// <summary>
    /// ARKit에서 얼굴의 데이터 값이 갱신될 때 자동으로 호출되는 함수 
    /// </summary>
    /// <param name="args"></param>
    private void OnFaceUpdated(ARFaceUpdatedEventArgs args)
    {
        UpdateArKitBlendShapeValues();
        UpdateTransformation();
    }

    /// <summary>
    /// ARKit에서 인식된 얼굴의 블렌드쉐입 값 가져오는 함수
    /// </summary>
    private void UpdateArKitBlendShapeValues()
    {
        var blendShapeCofficients
            = _arKitFaceSubsystem.GetBlendShapeCoefficients(_arFace.trackableId, Allocator.Temp);

        foreach (var blendShapeCoefficient in blendShapeCofficients)
        {
            var blendShapeLocation = blendShapeCoefficient.blendShapeLocation;

            if (_arKitBlendShapeValueTable.ContainsKey(blendShapeLocation))
            {
                _arKitBlendShapeValueTable[blendShapeLocation] = blendShapeCoefficient.coefficient * CoefficientValueScale;
            }
        }
    }

    /// <summary>
    /// 인식된 얼굴의 회전 및 이동 값 저장
    /// </summary>
    private void UpdateTransformation()
    {
        // ===Update Values About ARKitFace===
        _ARKitFaceRotation = _arFace.transform.localEulerAngles;
        _ARKitFacePosition = _arFace.transform.position;

        // Apply Face rotation to transCube.
        originalTransCube.transform.rotation = _arFace.transform.rotation;
    }

    /// <summary>
    /// 현재 위치를 중앙으로 위치시키는 함수 
    /// </summary>
    public void setTranslation()
    {
        cubePositionOffset = _ARKitFacePosition * -1f;
    }

    void Update()
    {
        Apply();                            // Apply BlendShape Coefficient to Character BlendShape Key.
        Print_Transformation_Data();        // Print Debug data.
    }

    /// <summary>
    /// 블렌드쉐입 적용 함수 호출
    /// </summary>
    private void Apply()
    {
        // Apply BlendShapeCoefficient To Character's Face.
        ApplyEyeBlink();
        ApplyEyeMovement();
        ApplyLipO();

        // Apply Transformation To Character Transformation.
        ApplyTransformation();

    }

    /// <summary>
    /// 눈 깜빡임 블렌드쉐입 값 적용 
    /// </summary>
    private void ApplyEyeBlink()
    {
        var leftBlinkValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeBlinkLeft];
        faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexLeftEyeBlink, leftBlinkValue);

        var rightBlinkValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeBlinkRight];
        faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexRightEyeBlink, rightBlinkValue);

    }

    /// <summary>
    /// 눈동자 움직임 블렌드쉐입 값 적용
    /// </summary>
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

    /// <summary>
    /// 입 모양 블렌드쉐입 값 적용 
    /// </summary>
    private void ApplyLipO()
    {
        var lipOValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.JawOpen];
        faceMeshRenderer.SetBlendShapeWeight(BlendShapeIndexLipO, lipOValue);
    }

    /// <summary>
    /// 카메라로 인식된 얼굴의 회전 및 이동 값 캐릭터에 적용 
    /// </summary>
    private void ApplyTransformation()
    {
        originalTransCube.transform.position = originalCubePosition + cubePositionOffset + _ARKitFacePosition;
        //string test = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}\n", originalCubePosition.x, originalCubePosition.y, originalCubePosition.z);
        //Debug.Log(test);
        originalTransCube.transform.rotation = Quaternion.Euler( new Vector3(_ARKitFaceRotation.x, _ARKitFaceRotation.y, _ARKitFaceRotation.z));      
    }

    /// <summary>
    /// 디버그 데이터 출력문
    /// </summary>
    private void Print_Transformation_Data()
    {

        // Print text About Person's Pos & Rot data.
        string str0 = "====Person's pos and rot====\n";
        string str1 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}\n", _ARKitFacePosition.x, _ARKitFacePosition.y, _ARKitFacePosition.z);
        string str2 = string.Format("rx: {0:F2} ry: {1:F2} rz: {2:F2}\n", _ARKitFaceRotation.x, _ARKitFaceRotation.y, _ARKitFaceRotation.z);
        text_debug.text = str0 + str1 + str2;

        // Print text About Character's Pos & Rot data.
        string str3 = string.Format("====Character's pos and rot====\n");
        string str4 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}\n", model_root.transform.localPosition.x, model_root.transform.localPosition.y, model_root.transform.localPosition.z);
        string str5 = string.Format("ry: {0:F2} rz: {1:F2} rx: {2:F2}\n", T_rig.localEulerAngles.x, T_rig.localEulerAngles.y, T_rig.localEulerAngles.z);
        text_debug.text += str3 + str4 + str5;

        Vector3 posOffset = script_posOffset.getDeltaPosition();
        Quaternion rotOffset = script_rotOffset.getRelativeRotation();
        
        // Print text About Offset Pos & Rot data.
        string str6 = string.Format("====Character's offset====\n");
        string str7 = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}\n", posOffset.x, posOffset.y, posOffset.z);
        string str8 = string.Format("rx: {0:F2} ry: {1:F2} rz: {2:F2}\n", rotOffset.eulerAngles.x, rotOffset.eulerAngles.y, rotOffset.eulerAngles.z);
        text_debug.text += str6 + str7 + str8;

    }



}
