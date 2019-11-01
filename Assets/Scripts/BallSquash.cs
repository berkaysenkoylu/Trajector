using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSquash : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector3 initialScale;
    Vector2 acceleration;
    Vector2 lastVelocity;

    // Exp
    float timing = 0f;

    void Start()
    {
        rb2d = GetComponentInParent<Rigidbody2D>();

        initialScale = transform.localScale;
    }

    void Update()
    {
        SquashBall();

        timing += Time.deltaTime;

        // Exp
        //SquashProgress(timing);
    }


    void SquashBall()
    {
        transform.right = rb2d.velocity.normalized;

        if(rb2d.velocity.magnitude > 0.5f)
        {
            transform.localScale = new Vector3(1.35f, 0.85f, transform.localScale.z);
        }
        else
        {
            transform.localScale = initialScale;
        }
    }

    void MeasureAcceleration()
    {
        acceleration = (rb2d.velocity - lastVelocity) / Time.fixedDeltaTime;
        lastVelocity = rb2d.velocity;
    }

    public void OnBallBounced(Vector2 collisionNormal)
    {
        // Find the angle between transform.right and normal
        float angle = Vector2.Angle(transform.right, collisionNormal);

        // Rotation matrix around z
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);

        Vector4 scaleRep = new Vector4(7f, 5f, 1f, 1f);
        Vector4 newScaleAfterRotation = new Vector4(0f, 0f, 0f, 0f);
        newScaleAfterRotation.x = (rotMatrix.GetRow(0).x * scaleRep.x) + 
                                  (rotMatrix.GetRow(0).y * scaleRep.y) + 
                                  (rotMatrix.GetRow(0).z * scaleRep.z) + 
                                  (rotMatrix.GetRow(0).w * scaleRep.w);

        newScaleAfterRotation.y = (rotMatrix.GetRow(1).x * scaleRep.x) +
                                  (rotMatrix.GetRow(1).y * scaleRep.y) +
                                  (rotMatrix.GetRow(1).z * scaleRep.z) +
                                  (rotMatrix.GetRow(1).w * scaleRep.w);

        newScaleAfterRotation.z = (rotMatrix.GetRow(2).x * scaleRep.x) +
                                  (rotMatrix.GetRow(2).y * scaleRep.y) +
                                  (rotMatrix.GetRow(2).z * scaleRep.z) +
                                  (rotMatrix.GetRow(2).w * scaleRep.w);

        newScaleAfterRotation.w = (rotMatrix.GetRow(3).x * scaleRep.x) +
                                  (rotMatrix.GetRow(3).y * scaleRep.y) +
                                  (rotMatrix.GetRow(3).z * scaleRep.z) +
                                  (rotMatrix.GetRow(3).w * scaleRep.w);

        // TODO: KEEP IT FOR NOW
        StartCoroutine(SquashBall(new Vector3(newScaleAfterRotation.x, newScaleAfterRotation.y, transform.localScale.z)));
    }

    IEnumerator SquashBall(Vector3 targetScale)
    {
        transform.localScale = targetScale;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = initialScale;
    }

    void SquashProgress(float time)
    {
        // 6.85f, 5.15f
        float xScale = 6.85f;
        float yScale = 5.15f;

        //transform.localScale = new Vector3(Mathf.Lerp(initialScale.x, xScale, 0.1f * Time.deltaTime), 
        //                                   Mathf.Lerp(initialScale.y, yScale, 0.1f * Time.deltaTime), 
        //                                   transform.localScale.z);

        Debug.Log(new Vector3(Mathf.Lerp(initialScale.x, xScale, time),
                                           Mathf.Lerp(initialScale.y, yScale, time),
                                           transform.localScale.z));
    }
}
