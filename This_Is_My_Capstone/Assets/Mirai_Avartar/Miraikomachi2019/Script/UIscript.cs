using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIscript : MonoBehaviour
{
    public Transform targetPosition;
    public float moveDuration = 1.0f;

    private bool isMoving = false;
    private float startTime;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);

            // Apply Lerp to move the object smoothly
            transform.position = Vector3.Lerp(startPosition, targetPosition.position, t);

            if (t >= 1.0f)
            {
                isMoving = false;
            }
        }
    }

    public void OnButtonClick1()
    {
        if (!isMoving)
        {
            // Start moving when the button is clicked
            isMoving = true;
            startTime = Time.time;
            startPosition = transform.position;
        }
    }
}
