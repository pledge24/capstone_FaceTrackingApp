using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonPress : MonoBehaviour
{
    // Start is called before the first frame update
    public Text displayText1; // 텍스트를 표시할 UI Text 오브젝트를 연결할 변수
    public Text displayText2; // 텍스트를 표시할 UI Text 오브젝트를 연결할 변수
    public Text displayText3; // 텍스트를 표시할 UI Text 오브젝트를 연결할 변수
    public Text displayText4; // 텍스트를 표시할 UI Text 오브젝트를 연결할 변수
    public Text displayText5; // 텍스트를 표시할 UI Text 오브젝트를 연결할 변수
    public Text displayText6; // 텍스트를 표시할 UI Text 오브젝트를 연결할 변수
    private bool isButtonPressed = false;


    private void Update()
    {
        if (isButtonPressed)
        {
            // 버튼이 눌린 동안 실행할 동작을 이곳에 추가합니다.
            Debug.Log("버튼이 눌렸습니다.");
            displayText1.gameObject.SetActive(true);
            displayText2.gameObject.SetActive(true);
            displayText3.gameObject.SetActive(true);
            displayText4.gameObject.SetActive(true);
            displayText5.gameObject.SetActive(true);
            displayText6.gameObject.SetActive(true);
           
        }
        else{
            displayText1.gameObject.SetActive(false);
            displayText2.gameObject.SetActive(false);
            displayText3.gameObject.SetActive(false);
            displayText4.gameObject.SetActive(false);
            displayText5.gameObject.SetActive(false);
            displayText6.gameObject.SetActive(false);
           
        }
    }

    public void OnButtonPressed()
    {
        isButtonPressed = true;
    }

    public void OnButtonReleased()
    {
        isButtonPressed = false;
    }

}
