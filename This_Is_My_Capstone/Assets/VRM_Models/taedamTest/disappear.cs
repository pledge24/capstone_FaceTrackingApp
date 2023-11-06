

using UnityEngine;
using UnityEngine.UI;

public class disappear : MonoBehaviour
{
    private Button myButton; // 자신의 버튼을 여기에 연결
    private bool flagbb = true;

    private void Start()
    {
        // 자신의 버튼 컴포넌트 가져오기
        myButton = GetComponent<Button>();
    }

    public void OnClick()
    {
        if (flagbb)
        {
            flagbb = false;
            Color transparentColor = new Color(1f, 1f, 1f, 0f);
            myButton.image.color = transparentColor;
        }
        else
        {
            flagbb = true;
            Color transparentColor = new Color(1f, 1f, 1f, 100f);
            myButton.image.color = transparentColor;

        }
            // 버튼을 투명하게 만듭니다.
        
    }
}