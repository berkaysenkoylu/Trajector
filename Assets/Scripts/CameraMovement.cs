using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothDampTime = 0.3f;
    public float positionOffset = 0.15f;

    Vector3 velocity;

    void Start()
    {
        velocity = Vector3.zero;
        MoveCamera();
    }

    void FixedUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        float distance = Mathf.Sqrt(Mathf.Pow((targetPosition.x - currentPosition.x), 2) + Mathf.Pow((targetPosition.y - currentPosition.y), 2));

        if (distance >= positionOffset)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothDampTime);
        }
    }
}