using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;      // ARFace, ARFaceManager.
using UnityEngine.XR.ARKit;             // ARKitFaceSubsystem. 

public class ARFaceBlendShapeVisualizerV5 : MonoBehaviour { 
    private ARFace _arFace;
    private ARFaceManager _arFaceManager;
    private ARKitFaceSubsystem _arKitFaceSubsystem;

    private readonly Dictionary<ARKitBlendShapeLocation, float> _arKitBlendShapeValueTable
        = new Dictionary<ARKitBlendShapeLocation, float>();

    [SerializeField] private Text text_debug;                       // debug text.
    [SerializeField] private GameObject originalTransCube;          // 얼굴 트랜스폼 원본 값을 저장할 큐브. 

    private const float CoefficientValueScale = 100f;

    private Renderer[] _characterRenderers;

    private Quaternion headRotation;
    private Vector3 originalCubePosition;

    public static Vector3 cubePositionOffset;
    private static Vector3 _ARKitFaceRotation = Vector3.zero;
    private static Vector3 _ARKitFacePosition = Vector3.zero;

    SetPositionOffsetScript script_posOffset;                       // Set Pos external script in Original cube
    SetRotationOffsetScript script_rotOffset;                       // Set Rot external script in Original cube


    // ====== New Code ===========

    static private SkinnedMeshRenderer faceMeshRenderer;
    private static Transform T_rig;
    private GameObject model;
    private static Transform model_root;
    private Transform model_head;
    static Dictionary<string, int> _characterBlendshapeIndexTable;

    private CharacterInterface[] CI;
    public int CharacterIndex = 0;

    private int previous_active_character_ID = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("스타드 함수에 진입. 각 캐릭터를 인터페이스에 추가합니다.\n");
        CI = new CharacterInterface[] { FindObjectOfType<Mirai_Komachi>(), FindObjectOfType<MonoCat>(), FindObjectOfType<Val>(), FindObjectOfType<Midori>(), FindObjectOfType<Ashtra>() };

        Debug.Log("각 캐릭터 추가 완료. 기본 캐릭터(Mirai_Komachi)로 설정을 불러옵니다.  \n");
        faceMeshRenderer = CI[0].GetfaceMeshRenderer();
        T_rig = CI[0].GetRig_Transform();
        model = CI[0].GetModel();
        model_root = CI[0].GetModel().GetComponent<Transform>();
        model_head = CI[0].GetHead().GetComponent<Transform>();
        _characterBlendshapeIndexTable = CI[0].GetBlendshapeTable();


        Debug.Log("코마치의 데이터를 정상적으로 불러왔습니다. 불러온 블렌드쉐입을 출력합니다\n");

        //foreach (string key in _characterBlendshapeIndexTable.Keys)
        //{
        //    Debug.Log(string.Format(key + ": {0}\n", _characterBlendshapeIndexTable[key]));
        //}

        Debug.Log("블렌드쉐입을 정상적으로 출력하였습니다.\n");

        // ==== print log about model's blendshape number.====

        //var blendShapeCount = faceMeshRenderer.sharedMesh.blendShapeCount;

        //for (var i = 0; i < blendShapeCount; i++)
        //{
        //    var blendShapeName = faceMeshRenderer.sharedMesh.GetBlendShapeName(i);
        //    Debug.Log($"{i} : {blendShapeName}");
        //}

        // ====================================================

        // == model code == 
        _characterRenderers = model_root.GetComponentsInChildren<Renderer>();

        // ======================== Cube Set Code =======================
        originalCubePosition = originalTransCube.transform.position;
        headRotation = T_rig.rotation;

        script_posOffset = originalTransCube.GetComponentInChildren<SetPositionOffsetScript>();
        script_rotOffset = originalTransCube.GetComponentInChildren<SetRotationOffsetScript>();

        cubePositionOffset = Vector3.zero;

        

        // ======================= Default ARKit Set Code ================
        //_characterRenderers = GetComponentsInChildren<Renderer>();
        _arFace = GetComponent<ARFace>();
        _arFaceManager = FindObjectOfType<ARFaceManager>();

        _arKitFaceSubsystem = _arFaceManager.subsystem as ARKitFaceSubsystem;

        SetupARKitBlendShapeTable();

        _arFace.updated += OnFaceUpdated;

        ARSession.stateChanged += OnARSessionStateChanged;

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
        //if (first_update)
        //{
        //    for(int i = 1; i < CI.Length; i++)
        //    {
        //        CI[i].GetModel().SetActive(false);
        //    }
        //    first_update = false;
        //}
        Apply();                            // Apply BlendShape Coefficient to Character BlendShape Key.
        Print_Transformation_Data();        // Print Debug data.
    }

    /// <summary>
    /// 블렌드쉐입 적용 함수 호출
    /// </summary>
    private void Apply()
    {
        // Apply BlendShapeCoefficient To Character's Face if not use Expression.
        ApplyEyeBlink();
        ApplyEyeMovement();      
        ApplyLipO();

        //// Apply Transformation To Character Transformation.
        ApplyTransformation();
        
    }

    /// <summary>
    /// 눈 깜빡임 블렌드쉐입 값 적용 
    /// </summary>
    private void ApplyEyeBlink()
    {
        var funFaceIntensity = faceMeshRenderer.GetBlendShapeWeight(_characterBlendshapeIndexTable["FunFace"]);
        //Debug.Log(funFaceIntensity);
        if (funFaceIntensity > 0.1f) return;

        var leftBlinkValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeBlinkLeft];
        var leftBlinkCharacterIndex= _characterBlendshapeIndexTable["BlendShapeIndexLeftEyeBlink"];
        faceMeshRenderer.SetBlendShapeWeight(leftBlinkCharacterIndex, leftBlinkValue);
       
        var rightBlinkValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeBlinkRight];
        var rightBlinkCharacterIndex = _characterBlendshapeIndexTable["BlendShapeIndexRightEyeBlink"];
        faceMeshRenderer.SetBlendShapeWeight(rightBlinkCharacterIndex, rightBlinkValue);
            
    }

    /// <summary>
    /// 눈동자 움직임 블렌드쉐입 값 적용
    /// </summary>
    private void ApplyEyeMovement()
    { 
        var LeftEyeMovementValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeLookOutLeft];
        var LeftEyeMovementCharacterIndex = _characterBlendshapeIndexTable["BlendShapeIndexLookLeft"];
        if (LeftEyeMovementCharacterIndex != -1)
        {
            faceMeshRenderer.SetBlendShapeWeight(LeftEyeMovementCharacterIndex, LeftEyeMovementValue);
        }

        var RightEyeMovementValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeLookInLeft];
        var RightEyeMovementCharacterIndex = _characterBlendshapeIndexTable["BlendShapeIndexLookRight"];
        if (RightEyeMovementCharacterIndex != -1)
        {
            faceMeshRenderer.SetBlendShapeWeight(RightEyeMovementCharacterIndex, RightEyeMovementValue);
        }
        

        var UpEyeMovementValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeLookUpLeft];
        var UpEyeMovementCharacterIndex = _characterBlendshapeIndexTable["BlendShapeIndexLookUp"];
        if (UpEyeMovementCharacterIndex != -1)
        {
            faceMeshRenderer.SetBlendShapeWeight(UpEyeMovementCharacterIndex, UpEyeMovementValue);
        }
        

        var DownEyeMovementValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.EyeLookDownLeft];
        var DownEyeMovementCharacterIndex = _characterBlendshapeIndexTable["BlendShapeIndexLookDown"];
        if (DownEyeMovementCharacterIndex != -1)
        {
            faceMeshRenderer.SetBlendShapeWeight(DownEyeMovementCharacterIndex, DownEyeMovementValue);
        }
        
    }

    /// <summary>
    /// 입 모양 블렌드쉐입 값 적용 
    /// </summary>
    private void ApplyLipO()
    {
        var LipOValue = _arKitBlendShapeValueTable[ARKitBlendShapeLocation.JawOpen];
        var LipOCharacterIndex = _characterBlendshapeIndexTable["BlendShapeIndexLipO"];
        faceMeshRenderer.SetBlendShapeWeight(LipOCharacterIndex, LipOValue);
    }

    /// <summary>
    /// 카메라로 인식된 얼굴의 회전 및 이동 값 캐릭터에 적용 
    /// </summary>
    private void ApplyTransformation()
    {
        originalTransCube.transform.position = originalCubePosition + cubePositionOffset + _ARKitFacePosition;
        originalTransCube.transform.rotation = Quaternion.Euler(new Vector3(_ARKitFaceRotation.x, _ARKitFaceRotation.y, _ARKitFaceRotation.z));
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

    public void Switch_Character_to_ID(int id)
    {
        //Debug.Log("Switch_Character 함수에 진입 성공. 캐릭터 스위칭을 진행합니다\n");

        model.SetActive(false);
        Set_Expression_default();
        
        previous_active_character_ID = id;
        CharacterIndex = id;

        // DEBUG PRINT
        // Debug.Log("test다\n");
        faceMeshRenderer = CI[CharacterIndex].GetfaceMeshRenderer();
        T_rig = CI[CharacterIndex].GetRig_Transform();
        model = CI[CharacterIndex].GetModel();
        model_head = CI[CharacterIndex].GetHead().transform;
        model.SetActive(true);
        model_root = CI[CharacterIndex].GetModel().GetComponent<Transform>();
        _characterBlendshapeIndexTable = CI[CharacterIndex].GetBlendshapeTable();

        // DEBUG PRINT
        //foreach (string key in _characterBlendshapeIndexTable.Keys)
        //{
        //    Debug.Log(string.Format(key + ": {0}\n", _characterBlendshapeIndexTable[key]));
        //}

        //Debug.Log(string.Format("캐릭터가 정상적으로 스위칭되었습니다. 현재캐릭터 Index: {0}\n", CharacterIndex));
    }

    private void Set_Blendshapes_zero()
    {
        var blendShapeCount = faceMeshRenderer.sharedMesh.blendShapeCount;

        for (var i = 0; i < blendShapeCount; i++)
        {
            var blendShapeName = faceMeshRenderer.sharedMesh.GetBlendShapeName(i);
            faceMeshRenderer.SetBlendShapeWeight(_characterBlendshapeIndexTable[blendShapeName], 0.0f);
        }

    }

    public void Set_Expression_default()
    {
        var expressionHappyCharacterIndex = _characterBlendshapeIndexTable["FunFace"];
        faceMeshRenderer.SetBlendShapeWeight(expressionHappyCharacterIndex, 0.0f);

        var expressionSadCharacterIndex = _characterBlendshapeIndexTable["SadFace"];
        faceMeshRenderer.SetBlendShapeWeight(expressionSadCharacterIndex, 0.0f);

        var expressionAngryCharacterIndex = _characterBlendshapeIndexTable["AngryFace"];
        faceMeshRenderer.SetBlendShapeWeight(expressionAngryCharacterIndex, 0.0f);

    }

    public void Set_Expression_happy()
    {
        Set_Expression_default();

        faceMeshRenderer.SetBlendShapeWeight(_characterBlendshapeIndexTable["BlendShapeIndexLeftEyeBlink"], 0.0f);
        faceMeshRenderer.SetBlendShapeWeight(_characterBlendshapeIndexTable["BlendShapeIndexRightEyeBlink"], 0.0f);

        var expressionHappyCharacterIndex = _characterBlendshapeIndexTable["FunFace"];
        faceMeshRenderer.SetBlendShapeWeight(expressionHappyCharacterIndex, 100.0f);

    }

    public void Set_Expression_sad()
    {
        Set_Expression_default();

        var expressionSadCharacterIndex = _characterBlendshapeIndexTable["SadFace"];
        faceMeshRenderer.SetBlendShapeWeight(expressionSadCharacterIndex, 100.0f);

    }

    public void Set_Expression_angry()
    {
        Set_Expression_default();

        var expressionAngryCharacterIndex = _characterBlendshapeIndexTable["AngryFace"];
        faceMeshRenderer.SetBlendShapeWeight(expressionAngryCharacterIndex, 100.0f);

    }

    public void start_setting()
    {
        for(int i = 1; i<CI.Length; i++)
        {
            CI[i].GetModel().SetActive(false);
        }

        setTranslation();
    }

    
}
