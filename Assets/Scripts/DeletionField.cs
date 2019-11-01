using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionField : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 1f;

    private Vector3 referenceVelocity;

    void Start()
    {
        
    }

    void Update()
    {
        FollowTarget();

        CalculateVerticalDistanceToTarget();
    }

    void FollowTarget()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y + 1f, 0f);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref referenceVelocity, smoothTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            target.GetComponent<GameIsOver>().PlayerIsDead();
        }
    }

    void CalculateVerticalDistanceToTarget()
    {
        float distance = (transform.position.y + transform.localScale.y / 2) - target.position.y;

        UIManager.instance.UpdateHazardSlider(distance);
    }
}
