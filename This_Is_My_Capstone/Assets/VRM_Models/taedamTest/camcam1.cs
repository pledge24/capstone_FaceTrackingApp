using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camcam1 : MonoBehaviour
{
    [SerializeField] private float left = 0.0f;
    [SerializeField] private float right = 1.0f;
    [SerializeField] private float bottom = 0.0f;
    [SerializeField] private float top = 1.0f;

    private Camera mainCamera;
    private bool flagcam = true;
    private float initialDistance; // 초기 손가락 간 거리
    private float zoomSpeed = 0.1f; // 조정 가능한 줌 속도
    private Vector2 initialTouchPosition; // 초기 터치 위치

    void Update()
    {
        if (Input.touchCount == 2)
        {
            // 두 손가락 간 거리 계산
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialDistance = currentDistance;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                // 손가락 간 거리 변화에 따라 카메라 줌 조절
                float deltaDistance = currentDistance - initialDistance;
                Zoom(deltaDistance);
            }
        }
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                initialTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // 손가락의 이동량에 따라 카메라 회전
                float deltaX = touch.position.x - initialTouchPosition.x;
                float deltaY = touch.position.y - initialTouchPosition.y;
                RotateCamera(deltaX, deltaY);
            }
        }
    }

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }
    void Zoom(float delta)
    {
        mainCamera.fieldOfView += delta * zoomSpeed;
        // 필요한 경우 최소/최대 제한을 추가할 수 있습니다.
        // if (Camera.main.fieldOfView < minFOV) Camera.main.fieldOfView = minFOV;
        // if (Camera.main.fieldOfView > maxFOV) Camera.main.fieldOfView = maxFOV;
    }
    void RotateCamera(float x, float y)
    {
        // 카메라 회전 로직을 구현하세요.
        transform.Rotate(Vector3.up * x * 0.1f);
        transform.Rotate(Vector3.left * y * 0.2f);
    }
}
