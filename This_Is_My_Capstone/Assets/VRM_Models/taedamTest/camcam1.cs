using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camcam1 : MonoBehaviour
{
    private Camera mainCamera;
    private bool flagcam = true;
    private float initialDistance; // 초기 손가락 간 거리
    private float zoomSpeed = 0.001f; // 조정 가능한 줌 속도
    private float rotateSpeed = 0.002f; // 조정 가능한 줌 속도
    private Vector2 initialTouchPosition; // 초기 터치 위치

    private float minFOV = -2f;
    private float maxFOV = 0.35f;

    [SerializeField] private GameObject stage;

    private float xRotateMove, yRotateMove;

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
                //Debug.Log(string.Format("deltaDistance {0:F2}", deltaDistance));
                Zoom(deltaDistance);
                initialDistance = currentDistance;
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

                //Debug.Log(string.Format("s_pos: {0:F2}, deltaX: {1:F2} deltaY {2:F2}", stage.transform.position, deltaX, deltaY));

                transform.RotateAround(stage.transform.position, Vector3.up, deltaX * rotateSpeed);
                transform.LookAt(stage.transform.position);
            }
        }
    }

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Zoom(float delta)
    {
        float z_pos = mainCamera.transform.position.z + delta * zoomSpeed;
        
        // 필요한 경우 최소/최대 제한을 추가할 수 있습니다.
        
        if (minFOV < z_pos && z_pos < maxFOV)
        {
            mainCamera.transform.Translate(0, 0, delta * zoomSpeed);
        }
        
    }
    void RotateCamera(float x, float y)
    {
        // 카메라 회전 로직을 구현하세요.
        transform.Rotate(Vector3.up * x * 0.01f);
        //transform.Rotate(Vector3.left * y * 0.01f);
    }
}
