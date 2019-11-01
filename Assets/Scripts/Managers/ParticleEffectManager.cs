using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
    // One for rectangular shape, one for circular shape
    public Material[] spriteMaterials;

    ObjectPooler objectPooler;

    private void Start()
    {
        Entity.OnEntityDestroyed += PlayEffect;

        objectPooler = ObjectPooler.instance;
    }

    private void OnDestroy()
    {
        Entity.OnEntityDestroyed -= PlayEffect;
    }

    void PlayEffect(Material mat, string tag, Vector3 position, Vector3 direction)
    {
        StartCoroutine(PlayEffectAtPosition(mat, tag, position, direction));
    }

    IEnumerator PlayEffectAtPosition(Material mat, string tag, Vector3 position, Vector3 direction)
    {
        GameObject effect = objectPooler.SpawnFromPool("EntityDestroyed", position, Quaternion.identity);

        switch(tag)
        {
            case "Rectangle":
                effect.GetComponent<ParticleSystem>().GetComponent<Renderer>().material = spriteMaterials[0];
                break;
            case "Circle":
                effect.GetComponent<ParticleSystem>().GetComponent<Renderer>().material = spriteMaterials[1];
                break;
            default:
                Debug.Log("Check your entities tags");
                break;
        }
        
        effect.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.color = mat.color;

        effect.transform.forward = direction;

        yield return new WaitForSeconds(1.5f);

        objectPooler.ReturnToPool("EntityDestroyed", effect);
    }
}
