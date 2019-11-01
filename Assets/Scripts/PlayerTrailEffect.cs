using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailEffect : MonoBehaviour
{
    public Transform player;
    public float effectDelay = 0.75f;

    Queue<GameObject> effectQueue = new Queue<GameObject>();
    Rigidbody2D plyerRb;
    float lastEffectTime;

    void Start()
    {
        plyerRb = player.GetComponent<Rigidbody2D>();

        for (int i = 0; i < transform.childCount; i++)
        {
            effectQueue.Enqueue(transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        if(plyerRb.velocity.magnitude > 0f)
        {
            if (Time.time - lastEffectTime > effectDelay)
            {
                PlayEffect();
            }
        }
    }

    void PlayEffect()
    {
        lastEffectTime = Time.time;

        GameObject effectPart = effectQueue.Dequeue();

        effectPart.transform.position = player.position;

        effectPart.SetActive(true);
    }

    public void GoBackToQueue(GameObject effect)
    {
        effect.SetActive(false);

        effectQueue.Enqueue(effect);
    }
}
