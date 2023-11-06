using System.Collections;
using UnityEngine;

public class testcc: MonoBehaviour
{
    [SerializeField] Transform A;
    [SerializeField] Transform B;

    bool isOn = true;

    IEnumerator movePlatform()
    {
        Debug.Log("Acess");
        float lerpSpeed = 3f;
        float currentTime = 0.0f;
        float t = 0.0f;

        if (isOn)
        {
            while (t <= 1f)
            {
                currentTime += Time.deltaTime;
                t = currentTime * 1f;
                transform.position = Vector3.Lerp(transform.position, B.position, Time.deltaTime * lerpSpeed);

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
                transform.position = Vector3.Lerp(transform.position, A.position, Time.deltaTime * lerpSpeed);

                //T.localPosition = Vector3.Lerp(src, dst, t);
                yield return null;
            }

        }

    }
    public void toggleUI()
    {
        isOn = !isOn;

        StartCoroutine(movePlatform());

    }

}