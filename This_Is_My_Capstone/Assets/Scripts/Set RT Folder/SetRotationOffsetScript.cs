using UnityEngine;

public class SetRotationOffsetScript : MonoBehaviour
{
    [SerializeField] private GameObject objectToRotate;         // 회전할 GameObject

    private Quaternion originalRotation;                        // 원본 회전값
    private Quaternion originalRelativeRotation;                // 원본 상대적 회전값
    private Quaternion targetRotation;                          // 타켓 회전값 
    private Quaternion relativeRotation;                        // 타겟의 상대적 회전값

    /// <summary>
    /// 상대적 회전값을 반환하는 Get함[
    /// </summary>
    /// <returns>상대적 회전값</returns>
    public Quaternion getRelativeRotation()
    {
        return relativeRotation;
    }

    /// <summary>
    /// 타켓의 회전 오프셋 값을 현재 회전값으로 변경하는 함수 
    /// </summary>
    public void setRotation()
    {
        targetRotation = Quaternion.identity;

        // 게임 오브젝트의 초기 회전값을 저장합니다.
        originalRotation = transform.rotation;

        // objectToRotate의 초기 상대 회전값을 저장합니다.
        originalRelativeRotation = Quaternion.Inverse(transform.rotation) * objectToRotate.transform.rotation;
    }

    /// <summary>
    /// 타겟의 회전 오프셋 값을 0으로 변경하는 함수
    /// </summary>
    public void callBack()
    {
        targetRotation = originalRotation;
    }

    void Start()
    {
        // 게임 오브젝트의 초기 회전값을 저장합니다.
        originalRotation = transform.rotation;

        // objectToRotate의 초기 상대 회전값을 저장합니다.
        originalRelativeRotation = Quaternion.Inverse(transform.rotation) * objectToRotate.transform.rotation;

        targetRotation = objectToRotate.transform.rotation;

        callBack();
    }

    void Update()
    {
        // objectToRotate에 상대적인 회전값을 적용합니다.
        relativeRotation = Quaternion.Inverse(originalRotation) * transform.rotation;

        objectToRotate.transform.rotation = targetRotation * relativeRotation;
    }
}
