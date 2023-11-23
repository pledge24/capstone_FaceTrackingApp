using System.Collections.Generic;
using UnityEngine;

public class SetRotationOffsetARRScript : MonoBehaviour
{
    private Quaternion originalRotation;                    // 원본 회전 값 	
    private List<Quaternion> targetRotations;                      // 타겟 회전 값 
    [SerializeField] private GameObject[] targetModelsIK;       // 타겟 오브젝트 

    private Quaternion deltaRotation;                       // 회전 변화량 값 
    public int characterIndex = 0;

    public void setCharacterIndex(int id)
    {
        characterIndex = id;
    }
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
        targetRotations = new List<Quaternion> ();
        originalRotation = transform.rotation;

        foreach (GameObject model in targetModelsIK)
        {
            targetRotations.Add(model.transform.rotation);
        }

        //targetModelsIK[0].SetActive(false);
//        Debug.Log(targetRotations.Count);
    }

    void Update()
    {
        // 회전 후의 Transform 값
        Quaternion currentRotation = transform.rotation;

        // 두 개의 Quaternion을 비교하여 회전한 축과 회전한 각도를 구합니다.
        deltaRotation = currentRotation * Quaternion.Inverse(originalRotation);

        //// apply rotation to all of characters
        //for(int i = 0; i < targetModelsIK.Length; i++)
        //{
        //    targetModelsIK[i].transform.rotation = deltaRotation * targetRotations[i];           
        //}

        targetModelsIK[characterIndex].transform.rotation = deltaRotation * targetRotations[characterIndex];
    }
}
