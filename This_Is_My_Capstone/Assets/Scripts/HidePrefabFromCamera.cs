using UnityEngine;

public class HidePrefabFromCamera : MonoBehaviour
{
    public Camera arCamera; // AR 카메라를 여기에 할당해야 합니다.

    void Start()
    {
        // Default 레이어의 비트를 가져옵니다.
        int defaultLayerBit = 1 << LayerMask.NameToLayer("Default");

        // 카메라에서 Default 레이어를 렌더링하지 않도록 설정합니다.
        arCamera.cullingMask &= ~defaultLayerBit;
    }
}
