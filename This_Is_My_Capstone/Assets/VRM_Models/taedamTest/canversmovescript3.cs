using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canversmovescript3 : MonoBehaviour
{

    public RectTransform targetRectTransform;
    public RectTransform targetRectTransformw;
    public RectTransform targetRectTransformz;
    public Vector2 x;
    public Vector2 y;
    public float lerpTime;
    private bool flag1 = false;
    private bool canversmovemove = false;

    public void pushhedButton()
    {
        {
            targetRectTransform.anchoredPosition = new Vector2(0f, -30f);
            targetRectTransformw.anchoredPosition = new Vector2(-5000f, 70f);
            targetRectTransformz.anchoredPosition = new Vector2(-5000f, 70f);
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