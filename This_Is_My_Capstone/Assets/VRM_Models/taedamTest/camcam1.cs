using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camcam1 : MonoBehaviour
{
    [SerializeField] private GameObject stage;

    private Camera mainCamera;
    private bool flagcam = true;
    private float initialDistance; // 초기 손가락 간 거리
    private float zoomSpeed = 0.001f; // 조정 가능한 줌 속도
    private float rotateSpeed = 0.05f; // 조정 가능한 카메라 회전 속도
   
    private Vector2 initialTouchPosition; // 초기 터치 위치
    private Vector2 previous_pos;

    private float minFOV = 0.55f;
    private float maxFOV = 3.7f;

    private float xRotateMove, yRotateMove;

    Transform cameraTransform;
    private Quaternion initialRotation;
    private Vector3 initialPosition;

    public float activeXRotate = 1.0f;
    public float activeYRotate = 1.0f;

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

            //if (touch.phase == TouchPhase.Began)
            //{
            //    initialTouchPosition = touch.position;
            //}
            //else if (touch.phase == TouchPhase.Moved)
            //{
            //    // 손가락의 이동량에 따라 카메라 회전
            //    float deltaX = touch.position.x - initialTouchPosition.x;
            //    float deltaY = touch.position.y - initialTouchPosition.y;

            //    //Debug.Log(string.Format("s_pos: {0:F2}, deltaX: {1:F2} deltaY {2:F2}", stage.transform.position, deltaX, deltaY));

            //    transform.RotateAround(stage.transform.position, Vector3.up, deltaX * rotateSpeed);
            //    transform.RotateAround(stage.transform.position, Vector3.left, deltaY * rotateSpeed);
            //    transform.LookAt(stage.transform.position);
            //}

            if (touch.phase == TouchPhase.Began)
            {
                previous_pos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // 손가락의 이동량에 따라 카메라 회전
                float deltaX = (touch.position.x - previous_pos.x) * activeXRotate;
                float deltaY = (touch.position.y - previous_pos.y) * activeYRotate;

                //Debug.Log(string.Format("s_pos: {0:F2}, deltaX: {1:F2} deltaY {2:F2}", stage.transform.position, deltaX, deltaY));

                transform.RotateAround(stage.transform.position, Vector2.up, deltaX * rotateSpeed);
                transform.RotateAround(stage.transform.position, Vector2.left, deltaY * rotateSpeed);
                transform.LookAt(stage.transform.position);

                previous_pos = touch.position;
            }
        }
    }

    private void Start()
    {
        mainCamera = GetComponent<Camera>();

        // 카메라 초기 위치 및 회전 값 저장 
        cameraTransform = GetComponent<Transform>();
        initialPosition = cameraTransform.position;
        initialRotation = cameraTransform.rotation;
    }

    void Zoom(float delta)
    {      
        Vector3 camera_pos = mainCamera.transform.position + Vector3.forward * delta * zoomSpeed;
        Vector3 stage_pos = stage.transform.position; 

        float distance = Vector3.Distance(camera_pos, stage_pos);

        Debug.Log(string.Format("camera: {0:F2}, stage: {1:F2}, distanceL {2:F2}", camera_pos, stage_pos, distance));
        // 필요한 경우 최소/최대 제한을 추가할 수 있습니다.

        if (minFOV < distance && distance < maxFOV)
        {
            mainCamera.transform.Translate(0, 0, delta * zoomSpeed);
        }

    }
    //void RotateCamera(float x, float y)
    //{
    //    // 카메라 회전 로직을 구현하세요.
    //    transform.Rotate(Vector3.up * x * 0.01f);
    //    //transform.Rotate(Vector3.left * y * 0.01f);
    //}

    public void ResetRotation()
    {
        // 회전값을 처음 위치로 변경
        cameraTransform.position = initialPosition;
        cameraTransform.rotation = initialRotation;
    }
}
