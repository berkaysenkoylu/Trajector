using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    public Text energyText;
    public Text scoreText;
    public Text comboText;
    public Slider energySlider;
    public Slider hazardSlider;
    public GameObject patternContainer;

    [Header("Material")]
    public Material defaultMat;

    int lastComboValue = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Get the initial score. I do this in this way because when 
        // I fire the event in the start menu, it is null. 
        // (TODO: REVISIT THIS LATER)
        UpdateScore(ScoreManager.instance.GetScore(), 1);

        ScoreManager.OnScoreChanged += UpdateScore;
        ScoreManager.OnMaterialListChanged += UpdatePatternUI;
    }

    private void OnDestroy()
    {
        ScoreManager.OnScoreChanged -= UpdateScore;
        ScoreManager.OnMaterialListChanged -= UpdatePatternUI;
    }

    public void UpdateEnergyBar(float energyAmount)
    {
        energySlider.value = energyAmount;
        energyText.text = "Energy: " + ((int)energyAmount).ToString();
    }

    public void UpdateHazardSlider(float amount)
    {
        hazardSlider.value = Mathf.Clamp(amount, -100f, 0f);
    }

    void UpdateScore(int newScore, int comboVal)
    {
        scoreText.text = "Score: " + newScore.ToString();
        comboText.text = "Combo: x " + comboVal.ToString();

        if (lastComboValue - comboVal > 0)
        {
            comboText.gameObject.GetComponent<Animator>().SetTrigger("isReset");
        }

        if (lastComboValue - comboVal < 0)
        {
            comboText.gameObject.GetComponent<Animator>().SetTrigger("isIncremented");
        }

        lastComboValue = comboVal;
    }

    void UpdatePatternUI(List<Material> matList, int hitIndex)
    {
        
        for (int i = 0; i < patternContainer.transform.childCount; i++)
        {
            if (i < matList.Count)
            {
                patternContainer.transform.GetChild(i).GetComponent<Image>().material = matList[i];
            }
            else
            {
                patternContainer.transform.GetChild(i).GetComponent<Image>().material = defaultMat;
            }
            patternContainer.transform.GetChild(i).localScale = new Vector3(1f, 1f, 1f);
        }

        // Magnify 
        //if (hitIndex != 0)
        //{
        //    patternContainer.transform.GetChild(hitIndex - 1).localScale = new Vector3(1.2f, 1.2f, 1f);
        //}
        patternContainer.transform.GetChild(hitIndex).localScale = new Vector3(1.2f, 1.2f, 1f);
    }
}
