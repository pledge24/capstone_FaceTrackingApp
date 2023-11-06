using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musiconoff : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    private void Start()
    {
        // 오디오 소스 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    public void ToggleAudio()
    {
        // 오디오 소스의 음소거 상태를 토글
        audioSource.mute = !audioSource.mute;
    }
}
