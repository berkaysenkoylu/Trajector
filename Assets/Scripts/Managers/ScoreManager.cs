using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    List<Material> hitMatList = new List<Material>();
    int hitIndex = 0;
    int score = 0;
    int iterationNumber = 0;
    int combo = 1;

    public delegate void ScoreChanged(int newScore, int comboVal);
    public static event ScoreChanged OnScoreChanged;

    public delegate void MaterialListChanged(List<Material> matList, int hitIndex);
    public static event MaterialListChanged OnMaterialListChanged;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Entity.OnIncrementScore += CalculateScoreValue;
    }

    private void OnDestroy()
    {
        Entity.OnIncrementScore -= CalculateScoreValue;
    }

    void CalculateScoreValue(Material mat, string tag, int value)
    {
        bool reset = false;
        // Add the material of the block to the list
        // if the material doesn't exist in the list already
        if (hitMatList.Count == 0)
        {
            hitMatList.Add(mat);

            hitIndex++;
        }
        else
        {
            if (hitMatList.Count != 5)
            {
                // Check the other materials first
                for (int i = 0; i < hitMatList.Count; i++)
                {
                    if (hitMatList[i].color == mat.color)
                    {
                        //Debug.Log("Reset the list");
                        // Reset the pattern list
                        hitMatList.Clear();

                        hitIndex = 0;

                        combo = 1;

                        iterationNumber = 0;

                        reset = true;

                        break;
                    }
                }
            }
            else
            {
                if (hitMatList[hitIndex % hitMatList.Count].color != mat.color)
                {
                    hitMatList.Clear();

                    hitIndex = 0;

                    combo = 1;

                    iterationNumber = 0;

                    reset = true;
                }
            }

            if (!reset)
            {
                if (hitMatList.Count < 5)
                {
                    hitMatList.Add(mat);

                    hitIndex++;
                    if (hitIndex > 4)
                    {
                        iterationNumber++;
                        hitIndex = 0;
                    }
                }
                else
                {
                    hitIndex++;
                    if(hitIndex > 4)
                    {
                        iterationNumber++;
                        hitIndex = 0;
                    }
                }

                //if (hitIndex == 4)
                //{
                //    iterationNumber++;
                //}
            }
        }

        //Debug.Log("HitIndex: " + hitIndex);
        //Debug.Log("IterationNumber: " + iterationNumber);

        OnMaterialListChanged?.Invoke(hitMatList, hitIndex);

        if (hitMatList.Count == 5)
        {
            combo = iterationNumber + 1;
            combo = Mathf.Clamp(combo, 1, 99);
        }

        score += value * combo;

        OnScoreChanged(score, combo);
    }

    void PatternListDebug()
    {
        foreach(Material patternMat in hitMatList)
        {
            Debug.Log(patternMat.color);
        }
    }

    public int GetScore()
    {
        return score;
    }
}
