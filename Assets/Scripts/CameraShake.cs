using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float cameraShakeDuration = 0.15f;
    public float cameraShakeMagnitude = 0.3f;

    private void Start()
    {
        Entity.OnEntityDestroyed += Shake;
    }

    private void OnDestroy()
    {
        Entity.OnEntityDestroyed -= Shake;
    }

    void Shake(Material mat, string tag, Vector3 pos, Vector3 dir)
    {
        // Arguments are coming from the event: 'onEntityDestroyed'

        StartCoroutine(ShakeProcess());
    }

    public IEnumerator ShakeProcess()
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsedTime = 0f;

        while(elapsedTime < cameraShakeDuration)
        {
            float xOffset = Random.Range(-1f, 1f) * cameraShakeMagnitude;
            float yOffset = Random.Range(-1f, 1f) * cameraShakeMagnitude;

            transform.localPosition = new Vector3(xOffset, yOffset, transform.localPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
