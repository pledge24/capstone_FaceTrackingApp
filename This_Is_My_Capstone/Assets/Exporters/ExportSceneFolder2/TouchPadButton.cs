using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class TouchPadButton : MonoBehaviour
{
    [SerializeField] private Text toggle_text;
    [SerializeField] private Text debug_text;
    [SerializeField] private ARFaceManager _arFaceManager;
    [SerializeField] private MeshRenderer originalCube_meshRenderer;
    [SerializeField] private MeshRenderer applyOffsetCube_meshRenderer;

    private bool isToggled = false;

    private void Start()
    {
        // 디버그 테스트 비활성화한 상태로 시작합니다. 
        debug_text.enabled = false;
        originalCube_meshRenderer.enabled = false;
        applyOffsetCube_meshRenderer.enabled = false;
    }

    public void OnOffDevelopmentMode()
    {
        // 토글 상태를 변경합니다.
        isToggled = !isToggled;

        // 버튼이 클릭되었을 때 실행되는 코드를 작성합니다.
        toggle_text.text = "DebugMode: ";
        toggle_text.text += isToggled ? "On" : "Off";
        debug_text.enabled = isToggled;
        originalCube_meshRenderer.enabled = isToggled;
        applyOffsetCube_meshRenderer.enabled = isToggled;

        foreach (ARFace face in _arFaceManager.trackables)
        {
            for (int i = 0; i < face.transform.childCount; i++)
            {
                face.transform.GetChild(i).gameObject.SetActive(isToggled);
            }

        }
    }
  
}
