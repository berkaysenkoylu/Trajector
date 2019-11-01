using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform visualArrowOrigin;
    public float ballRbForceConstant = 250f;
    public float energyDepletionRate = 5f;
    public float energyRegenerationRage = 3f;
    public float maxEnergy = 100f;

    [SerializeField]
    private float timeSlowedAmount;
    private float currentEnergy;

    Vector3 lastDirection;
    Rigidbody2D rb2d;
    bool clickedOn = false; // TODO: change this later
    Vector3 directionVector;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        Entity.OnCollisionWithPlayer += BallReflection;

        currentEnergy = maxEnergy;

        UIManager.instance.UpdateEnergyBar(currentEnergy);
    }

    private void OnDisable()
    {
        Entity.OnCollisionWithPlayer -= BallReflection;
    }

    private void OnDestroy()
    {
        Entity.OnCollisionWithPlayer -= BallReflection;
    }

    void BallReflection(Vector2 hitNormal, float energyIncrementAmount)
    {
        Vector3 newDirection = Vector3.Reflect(directionVector, new Vector3(hitNormal.x, hitNormal.y, 0));

        directionVector = newDirection;
        if (directionVector.normalized != Vector3.zero)
        {
            lastDirection = directionVector.normalized;
            ApplyForce(directionVector.normalized);
        }
        else
        {
            ApplyForce(lastDirection);
        }
        //ApplyForce(directionVector.normalized);

        UpdateEnergy(energyIncrementAmount);

        // Send normal info to GFX to handle a bouncy squash effect
        // TODO: KEEP IT FOR NOW
        GetComponentInChildren<BallSquash>().OnBallBounced(hitNormal);
    }

    void Update()
    {
        if (clickedOn)
        {
            // 0:Left mouse button, 1: Right mouse button, 2: Middle mouse button
            if (Input.GetMouseButton(0))
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 worldPos = new Vector3(worldPoint.x, worldPoint.y, 0f);
                float forceMagnitudeMultiplier = 0f;

                if (worldPos.y <= transform.position.y)
                {
                    directionVector = (worldPos - transform.position);
                } 
                else
                {
                    directionVector = new Vector3(directionVector.x, 0f, directionVector.z);
                }

                Vector3 arrowScale = new Vector3(1f, 1f, 0f);
                forceMagnitudeMultiplier = Vector3.Magnitude(directionVector);

                arrowScale.z = forceMagnitudeMultiplier;
                arrowScale.z = Mathf.Clamp(arrowScale.z, 0f, 6f);

                visualArrowOrigin.localScale = arrowScale;

                visualArrowOrigin.GetChild(0).gameObject.SetActive(true);

                if(directionVector != Vector3.zero)
                    visualArrowOrigin.forward = directionVector;
                //Debug.Log(directionVector);

                Time.timeScale = 0.1f;

                timeSlowedAmount += Time.deltaTime * 10;

                Debug.DrawRay(transform.position, directionVector, Color.white);
            }

            UpdateEnergy(-timeSlowedAmount * energyDepletionRate * 0.1f);

            if (Input.GetMouseButtonUp(0) || currentEnergy <= 5f)
            {
                timeSlowedAmount = 0;

                if(directionVector.normalized != Vector3.zero)
                {
                    lastDirection = directionVector.normalized;
                    ApplyForce(directionVector.normalized);
                }
                else
                {
                    ApplyForce(lastDirection);
                }

                

                Time.timeScale = 1f;
                visualArrowOrigin.GetChild(0).gameObject.SetActive(false);
                clickedOn = false;
            }
        }

        if(!clickedOn && currentEnergy < maxEnergy)
        {
            UpdateEnergy(energyRegenerationRage * 0.01f);
        }
    }

    void ApplyForce(Vector3 direction)
    {
        // First, reset the velocity on the rigidbody
        rb2d.velocity = Vector2.zero;

        // Apply the new force
        rb2d.AddForce(direction * ballRbForceConstant);
    }

    void UpdateEnergy(float energyChangeAmount)
    {
        currentEnergy += energyChangeAmount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        UIManager.instance.UpdateEnergyBar(currentEnergy);
    }

    private void OnMouseDown()
    {
        directionVector = Vector3.zero;
        clickedOn = true;
    }
}
