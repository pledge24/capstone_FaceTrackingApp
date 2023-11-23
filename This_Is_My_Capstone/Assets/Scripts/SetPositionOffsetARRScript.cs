using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionOffsetARRScript : MonoBehaviour
{  
    [SerializeField] private float Weight = 0.5f;           // z좌표값 적용 가중치
    private float test_weight = 1.0f;

    static private Transform curr_character_rig;
    static private GameObject curr_character_body;
    private Vector3 defaultPosition;                       // 원본 위치값
    private List<Vector3> defaultHeadPositions;                         // 타겟 위치값 	
    private List<Vector3> defaultRootPositions;                         // 타겟 위치값

    public static Vector3 deltaPosition;                    // 위치 변화값

    private CharacterInterface[] CI;
    private int characterIndex = 0;

    private Vector3 stage_position;

    private Vector3 defaultPos;

    public void setCharacterIndex()
    {
        characterIndex = (characterIndex + 1) % CI.Length;
        curr_character_rig = CI[characterIndex].GetRig_Transform();
        curr_character_body = CI[characterIndex].GetModel();
    }

    public void setCharacterIndexToID(int id)
    {
        characterIndex = id;
        curr_character_rig = CI[characterIndex].GetRig_Transform();
        curr_character_body = CI[characterIndex].GetModel();

        //curr_character_body.transform.position.Set(0, 0, 0);
        Debug.Log(string.Format("{0:F2}, {1:F2}, {2:F2} ", stage_position.x, stage_position.y, stage_position.z));
        Debug.Log(string.Format("characterIndex is {0}", characterIndex));
    }

    /// <summary>
    /// 위치 변화값을 반환하는 함수
    /// </summary>
    /// <returns>위치 변화값</returns>
    public Vector3 getDeltaPosition()
    {
        return deltaPosition;
    }

    // Start is called before the first frame update
    void Start()
    {

        defaultPos = new Vector3(0, -1.2f, 1);
       
        CI = new CharacterInterface[] { FindObjectOfType<Mirai_Komachi>(), FindObjectOfType<MonoCat>(), FindObjectOfType<Val>(), FindObjectOfType<Midori>(), FindObjectOfType<Ashtra>() };
        defaultHeadPositions = new List<Vector3>();
        defaultRootPositions = new List<Vector3>();

        defaultPosition = transform.position;       // 큐브의 처음 위치 값을 따로 보관함

        //Debug.Log(CI.Length);
        
        foreach (CharacterInterface c in CI)
        {
            
            //Vector3 test = new Vector3();
            //test = c.GetRig_Transform().position;
            //Debug.Log(string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", test.x, test.y, test.z));
            defaultHeadPositions.Add(c.GetRig_Transform().position);
            defaultRootPositions.Add(c.GetModel().transform.position);
        }
        //Debug.Log(string.Format("sss: {0}", defaultHeadPositions.Count));
        curr_character_rig = CI[0].GetRig_Transform();
        curr_character_body = CI[0].GetModel();
        
        //Debug.Log(string.Format("sss: {0} : {1}",defaultHeadPositions.Count, defaultRootPositions.Count));
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(string.Format("x: {0:F2} y: {1:F2} z: {2:F2}", defaultHeadPositions[0].x, defaultHeadPositions[0].y, defaultHeadPositions[0].z));
        // 변화량 값 계산 및 저장 
        deltaPosition = transform.position - defaultPosition;


        //string str4;// = string.Format("x: {0:F2} y: {1:F2} z: {2:F2}\n", deltaPosition.x, deltaPosition.y, deltaPosition.z);
        //Debug.Log(str4);
        curr_character_rig.position = defaultHeadPositions[characterIndex] + (deltaPosition * test_weight);
        //str4 = string.Format("{0:F2} y: {1:F2} z: {2:F2}\n", curr_character_rig.position.x, curr_character_rig.position.y, curr_character_rig.position.z);
        //Debug.Log(str4);
        // 몸 움직임 z축 봉인 
        deltaPosition.Set(deltaPosition.x, deltaPosition.y, 0.0f);
        curr_character_body.transform.position = defaultRootPositions[characterIndex] + (deltaPosition * Weight);
        
        
        //curr_character_body.transform.position = defaultPos + (deltaPosition * Weight);
    }
}
