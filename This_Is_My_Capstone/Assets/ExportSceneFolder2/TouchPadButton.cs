using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class TouchPadButton : MonoBehaviour
{
    public Text toggle_text;
    public Text debug_text;

    private bool isToggled = false;

    public ARFaceManager _arFaceManager;

    private void Start()
    {
        // 디버그 테스트 비활성화한 상태로 시작합니다. 
        debug_text.enabled = false;
        
    }

    public void OnOffDevelopmentMode()
    {
        // 토글 상태를 변경합니다.
        isToggled = !isToggled;

        // 버튼이 클릭되었을 때 실행되는 코드를 작성합니다.
        toggle_text.text = "DebugMode: ";
        toggle_text.text += isToggled ? "On" : "Off";
        debug_text.enabled = isToggled;

        foreach (ARFace face in _arFaceManager.trackables)
        {
            for (int i = 0; i < face.transform.childCount; i++)
            {
                face.transform.GetChild(i).gameObject.SetActive(isToggled);
            }

        }
    }
  
}
