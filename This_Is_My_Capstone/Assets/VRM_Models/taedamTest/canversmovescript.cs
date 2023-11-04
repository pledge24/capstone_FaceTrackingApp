using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canversmovescript : MonoBehaviour
{

    public RectTransform targetRectTransform;
    public Vector2 startAnchoredPosition;
    public Vector2 endAnchoredPosition;
    public float lerpTime;
    private bool flag1 = false;
    private bool canversmovemove = false;

    public void pushhedButton()
    {
        if (flag1)
        {
            targetRectTransform.anchoredPosition = new Vector2(-5000f, 70f);
            flag1 = false;
        }
        else
        {
            targetRectTransform.anchoredPosition = new Vector2(-300f, 70f);
            flag1 = true;
        }

    }
    IEnumerator WaitAndDoSomething(float waitTime)
    {
        // WaitForSeconds를 사용하여 일정 시간 동안 대기
        yield return new WaitForSeconds(waitTime);

        // 대기 후에 수행할 작업
        Debug.Log("Waited for " + waitTime + " seconds. Now do something!");
    }
}