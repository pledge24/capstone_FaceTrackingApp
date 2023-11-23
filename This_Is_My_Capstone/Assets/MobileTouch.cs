using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTouch : MonoBehaviour
{
    Material mat;
    bool touchFlag;
    List<Vector3> touchPosList;

    int numberOfTriangle = 10;
    float radius = 0.02f;
    float zoomSpeed = 1.0f;

    void makeCircle(Vector3 touchPos, Color color)
    {
        GL.Color(color);

        List<Vector3> pos = new List<Vector3>();

        for (int i = 0; i < numberOfTriangle; i++)
        {
            float angle = i * (Mathf.PI * 2.0f) / numberOfTriangle;

            Vector3 dot = (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0)) * radius;

            float width = (float)Screen.width;
            float height = (float)Screen.height;

            if (width > height)
            {
                dot.x = dot.x * height / width;
            }
            else
            {
                dot.y = dot.y * width / height;
            }

            pos.Add(dot + touchPos);
        }

        for (int i = 0; i < numberOfTriangle - 1; i++)
        {
            GL.Vertex3(touchPos.x, touchPos.y, 0);
            GL.Vertex3(pos[i + 1].x, pos[i + 1].y, 0);
            GL.Vertex3(pos[i].x, pos[i].y, 0);
        }

        GL.Vertex3(touchPos.x, touchPos.y, 0);
        GL.Vertex3(pos[0].x, pos[0].y, 0);
        GL.Vertex3(pos[numberOfTriangle - 1].x, pos[numberOfTriangle - 1].y, 0);
    }

    void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }

        if (touchFlag)
        {
            GL.PushMatrix();
            mat.SetPass(0);

            GL.LoadOrtho();

            GL.Begin(GL.TRIANGLES);

            int touchCount = touchPosList.Count;

            Color[] colorSet = new Color[] { Color.red, Color.green, Color.blue };

            for (int i = 0; i < touchCount; i++)
                makeCircle(touchPosList[i], colorSet[i % 3]);

            GL.End();
            GL.PopMatrix();
        }
    }

    void Start()
    {
       // mat = new Material(Shader.Find("Draw/Quads"));
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Vector2 prevPos0 = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
            Vector2 prevPos1 = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;

            float prevDistance = (prevPos0 - prevPos1).magnitude;
            float currDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;

            float diff = currDistance - prevDistance;

            Vector3 cameraDirection = this.transform.localRotation * Vector3.forward;

            this.transform.position += cameraDirection * Time.deltaTime * diff * zoomSpeed;
        }

        if (Input.GetMouseButton(0))
        {
            touchPosList = new List<Vector3>();

            int touchCount = Input.touchCount;
            for (int i = 0; i < touchCount; i++)
            {
                Vector3 inputPos = Input.GetTouch(i).position;

                Vector3 mousePos
                    = new Vector3(inputPos.x, inputPos.y, -Camera.main.transform.position.z);

                Vector3 touchPos = Camera.main.ScreenToViewportPoint(mousePos);

                touchPosList.Add(touchPos);
            }

            touchFlag = true;
        }
        else
        {
            touchFlag = false;
        }
    }
}