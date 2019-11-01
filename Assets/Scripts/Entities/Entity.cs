using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Attributes")]
    public float energyWorth = 5f;
    public float despawnThreshold = 50f;
    public int scoreValue;

    public Animator animController;

    [SerializeField]
    private Vector2 gridCellInfo;
    [SerializeField]
    private bool isVisible = false;
    private Transform cameraTransform;
    private Material mat;

    public delegate void CollidedWithPlayer(Vector2 hit, float energyBonus);
    public static event CollidedWithPlayer OnCollisionWithPlayer;

    public delegate void EntityDestroyed(Material mat, string tag, Vector3 pos, Vector3 dir);
    public static event EntityDestroyed OnEntityDestroyed;

    public delegate void IncrementScore(Material mat, string tag, int scoreValue);
    public static event IncrementScore OnIncrementScore;

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;

        cameraTransform = Camera.main.transform;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {

            OnCollisionWithPlayer(collision.GetContact(0).normal, energyWorth);
            //collision.collider.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 200f);

            // Simulate object dying
            DestroyObject();

            OnEntityDestroyed(mat, tag, transform.position, collision.GetContact(0).normal);

            OnIncrementScore(mat, tag, scoreValue);
        }
    }

    private void Update()
    {
        // If the entity becomes invisible, calculate the distance
        // between the entity and the player. If player is too far away
        // from the entity, then despawn the entity
        if(!isVisible)
        {
            if(CalculateDistanceToPlayer() >= despawnThreshold)
            {
                DestroyObject();
            }
        } 
    }

    public void SetEntityMaterial(Material newMaterial)
    {
        GetComponent<SpriteRenderer>().material = newMaterial;
    }

    public void OnSpawn()
    {
        animController.SetTrigger("isSpawned");
    }

    public void SetGridCellInfo(Vector2 gridCell)
    {
        gridCellInfo = gridCell;
    }

    private void OnBecameInvisible()
    {
        // Deactivate the object once it becomes invisible
        //DestroyObject();
        isVisible = false;
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private float CalculateDistanceToPlayer()
    {
        Transform cameraPivot = cameraTransform.parent;
        float distance = Mathf.Sqrt(Mathf.Pow((transform.position.x - cameraPivot.position.x), 2) +
                                    Mathf.Pow((transform.position.y - cameraPivot.position.y), 2));

        return distance;
    }

    private void DestroyObject()
    {
        ObjectPooler.instance.ReturnToPool(transform.tag, gameObject);

        animController.SetTrigger("isDespawned");
    }
}
