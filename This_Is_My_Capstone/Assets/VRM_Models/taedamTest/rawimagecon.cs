using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rawimagecon : MonoBehaviour
{
    // Start is called before the first frame update
    RawImage rawImage; // Raw Image 컴포넌트
    public Texture newTexture1; // 변경할 텍스처
    public Texture newTexture2; // 변경할 텍스처
    public Texture newTexture3; // 변경할 텍스처
    private int flagtextture = 1;

    private void Start()
    {
        // Raw Image 컴포넌트 가져오기
        rawImage = GetComponent<RawImage>();
    }

    public void ChangeTexture()
    {
        // 텍스처 변경
        if(flagtextture == 1)
        {
            rawImage.texture = newTexture2;
            flagtextture++;
        }else if(flagtextture == 2)
        {
            rawImage.texture = newTexture3;
            flagtextture++;
        }
        else
        {
            rawImage.texture = newTexture1;
            flagtextture = 1;
        }
        
    }
}
