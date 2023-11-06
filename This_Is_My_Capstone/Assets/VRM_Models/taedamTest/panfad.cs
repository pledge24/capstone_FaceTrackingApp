using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panfad : MonoBehaviour
{
    public float scaleSpeed = 1.0f; // 조정 가능한 크기 변화 속도
    private RectTransform panelRectTransform;
    float currentTime = 0.0f;
    float t = 0.0f;
    bool isOn = false;

    private void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
        panelRectTransform.localScale = new Vector3(2, 0, 0); // 초기에는 보이지 않도록 설정
    }


    
    IEnumerator makePlatform()
    {
        Debug.Log("Acess");
        float lerpSpeed = 4f;
        float currentTime = 0.0f;
        float t = 0.0f;

        if (isOn)
        {
            while (t <= 1f)
            {
                currentTime += Time.deltaTime;
                t = currentTime * 1f;
                Vector3 targetScale = new Vector3(2, 1, 0); // 최종 크기 10 10 10
                panelRectTransform.localScale = Vector3.Lerp(panelRectTransform.localScale, targetScale, Time.deltaTime * lerpSpeed);

                //T.localPosition = Vector3.Lerp(src, dst, t);
                yield return null;
            }

        }
        else
        {
            while (t <= 1f)
            {
                currentTime += Time.deltaTime;
                t = currentTime * 1f;
                Vector3 targetScale = new Vector3(2, 0, 0); // 최종 크기 0 0 0
                panelRectTransform.localScale = Vector3.Lerp(panelRectTransform.localScale, targetScale, Time.deltaTime * lerpSpeed);

                //T.localPosition = Vector3.Lerp(src, dst, t);
                yield return null;
            }

        }

    }

    public void toggleUI()
    {
        isOn = !isOn;

        StartCoroutine(makePlatform());

    }
}

                
