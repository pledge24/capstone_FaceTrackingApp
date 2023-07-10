using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    public Text text;
    int score = 0;

    void Start()
    {
        text = GetComponent<Text>();
        //SetText();
    }

    private void Update()
    {
        
    }
    public void GetScore()
    {
        score += 100;
        SetText();
    }

    public void SetText()
    {
       // text.text = "Score : " + score.ToString();
        text.text = "umumumumum";
    }

}
