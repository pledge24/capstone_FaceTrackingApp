using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickMoveCamera : MonoBehaviour
{

    public Transform Target1; // 카메라 타겟
    public Transform Target2; // 카메라 타겟
    public Transform Target3; // 카메라 타겟
    public Transform Menu;
    

    public Transform ARcamera00;

    public float moveDuration = 1.0f;

    private bool isMoving = false;
    private float startTime;
    private Vector3 startPosition;
    private Vector3 targetCameraPosition;
    Vector3 TargetPos;
    private Transform moveTarget;

    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 0.0f;           // 카메라의 y좌표
    public float offsetZ = 0.0f;          // 카메라의 z좌표

    public float CameraSpeed = 10.0f;       // 카메라의 속도

    public float fixsetX = 0.0f;
    public float fixsetY = 0.0f;
    public float fixsetZ = 0.0f;

    public int selectCharaterNum = 0; 

    public void buttonClick1()
    {
        isMoving = true;
        moveTarget = Target1;
        fixsetX = 0.0f;
        fixsetY = 0.2f;
        fixsetZ = 0.0f;
        selectCharaterNum = 1;
    }

    public void buttonClick2()
    {
        isMoving = true;
        moveTarget = Target2;
        fixsetX = 0.0f;
        fixsetY = 0.0f;
        fixsetZ = 0.0f;
        selectCharaterNum = 2;
    }

    public void buttonClick3()
    {
        isMoving = true;
        moveTarget = Target3;
        fixsetX = 0.0f;
        fixsetY = 0.0f;
        fixsetZ = 0.0f;
        selectCharaterNum = 3;
    }

    public void closeUpFace()
    {
        fixsetZ = 0.45f;
    }
    public void closeDownFace()
    {
        fixsetZ = 0f;
    }
    public void closeUpFace3()
    {
        isMoving = true;
        moveTarget = Target3;
        fixsetX = 0.0f;
        fixsetY = 0.0f;
        fixsetZ = 0.4f;
    }


    void FixedUpdate()
    {
        if (isMoving)
        {
            // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
            TargetPos = new Vector3(
                moveTarget.position.x + offsetX + fixsetX,
                moveTarget.position.y + 1F + fixsetY,
                moveTarget.position.z + 1F + fixsetZ
                );

            // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
        }
    }
}
