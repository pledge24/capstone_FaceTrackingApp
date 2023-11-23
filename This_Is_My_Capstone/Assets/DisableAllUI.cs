using UnityEngine;
using UnityEngine.UI;

public class DisableAllUI : MonoBehaviour
{
    private bool hideMode = false;

    Canvas canvas;

    private void Start()
    {
        // Canvas에 대한 참조 가져오기 (이 스크립트가 Canvas에 연결되어 있다고 가정)
        canvas = GameObject.Find("value_canvas").GetComponent<Canvas>();
    }

    public void DisableUI()
    {
        hideMode = !hideMode;

        // Canvas가 없다면 모든 Canvas 찾아서 비활성화
        if (canvas == null)
        {
            Canvas[] allCanvas = FindObjectsOfType<Canvas>();
            foreach (Canvas c in allCanvas)
            {
                c.gameObject.SetActive(false);
            }
        }
        else
        {
            // Canvas가 있다면 해당 Canvas의 모든 자식 비활성화
            for (int i = 0; i < canvas.transform.childCount; i++)
            {
                canvas.transform.GetChild(i).gameObject.SetActive(!hideMode);
            }


        }

        gameObject.SetActive(true);
 
    }
}
