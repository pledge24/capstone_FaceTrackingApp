using UnityEngine;

public class SetRotationOffsetProtoScript : MonoBehaviour
{
    private Quaternion originalRotation;                    // 원본 회전 값 	
    private Quaternion targetRotation;                      // 타겟 회전 값 

    [SerializeField] private GameObject targetObject;       // 타겟 오브젝트 

    private Quaternion deltaRotation;                       // 회전 변화량 값 

    /// <summary>
    /// 회전 변화량 값 반환 
    /// </summary>
    /// <returns>회전 변화량 </returns>
    public Quaternion getDeltaRotation()
    {
        return deltaRotation;
    }

    void Start()
    {
        // 게임 오브젝트의 초기 회전값을 저장합니다.
        originalRotation = transform.rotation;
        targetRotation = targetObject.transform.rotation;
    }

    void Update()
    {
        // 회전 후의 Transform 값
        Quaternion currentRotation = transform.rotation;

        // 두 개의 Quaternion을 비교하여 회전한 축과 회전한 각도를 구합니다.
        deltaRotation = currentRotation * Quaternion.Inverse(originalRotation);

        targetObject.transform.rotation = deltaRotation * targetRotation;
        //Debug.Log(deltaRotation);
    }
}
