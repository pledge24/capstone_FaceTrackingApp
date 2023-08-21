using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionOffsetScript : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;       // 이동을 적용할 타겟 오브젝트 	
    [SerializeField] private bool applyZPos = true;         // z좌표값 이동 여부 	
    [SerializeField] private float Weight = 1.0f;           // z좌표값 적용 가중치 

    private Vector3 originalPosition;                       // 원본 위치값
    private Vector3 targetPosition;                         // 타겟 위치값 	

    public static Vector3 deltaPosition;                    // 위치 변화값

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
        originalPosition = transform.position;
        targetPosition = targetObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 변화량 값 계산 및 저장 
        deltaPosition = transform.position - originalPosition;

        // z좌표값 이동 설정 유무에 따라 적용 
        if(!applyZPos) deltaPosition.Set(deltaPosition.x, deltaPosition.y, 0.0f);

        targetObject.transform.position = targetPosition + (deltaPosition * Weight);
    }
}
