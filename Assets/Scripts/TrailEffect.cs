using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    PlayerTrailEffect playerTrailEffect;

    private void Start()
    {
        playerTrailEffect = GetComponentInParent<PlayerTrailEffect>();
    }

    public void AnimationEnded()
    {
        playerTrailEffect.GoBackToQueue(gameObject);
    }
}
