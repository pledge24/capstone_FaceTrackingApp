using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmonoff : MonoBehaviour
{
    public AudioSource audioSource; // Audio Source 컴포넌트를 인스펙터에서 설정할 수 있도록 public 변수를 만듭니다.
    public float volume = 1.0f; // 조절할 볼륨 값을 인스펙터에서 설정할 수 있도록 public 변수를 만듭니다.

    // 스크립트가 활성화될 때 실행됩니다.
    private void Start()
    {
        // Audio Source가 설정되지 않은 경우, 현재 GameObject에서 Audio Source 컴포넌트를 찾습니다.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // 초기 볼륨 설정
        audioSource.volume = volume;
    }

    // 볼륨을 변경하는 메서드
    public void SetVolumeOff(float newVolume)
    {
        if (audioSource != null)
        {
            volume = 0.0f;
            audioSource.volume = volume;
        }
    }
     public void SetVolumeOn(float newVolume)
    {
        if (audioSource != null)
        {
            volume = 1.0f;
            audioSource.volume = volume;
        }
    }
}
