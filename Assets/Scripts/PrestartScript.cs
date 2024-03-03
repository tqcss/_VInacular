using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrestartScript : MonoBehaviour
{
    public string baseLanguage;
    public string targetLanguage;
    private string[] baseLanguages = {"english", "filipino"};
    private string[] targetLanguages = {"akeanon", "capiznon", "hiligaynon", "kinaraya"};
    
    public GameObject introPanel;
    public GameObject langSelectionPanel;
    public GameObject enjoyPanel;
    public Animator langSelectionAnimator;
    public Animator enjoyAnimator;

    public Image langText;
    public Sprite[] langTextSprites;

    public Font montserratMedium;
    public Font montserratBold;
    public Color defaultTextColor = new Color(186, 214, 234, 250);
    public Color selectedTextColor = new Color(60, 197, 255, 250);

    private const int MAX_DEFAULT_LANG = 2;
    private const int MAX_NATIVE_LANG = 4;
    private int submitTurn = 0;
    
    
    private void Awake()
    {
        baseLanguage = PlayerPrefs.GetString("BaseLanguage", "english");
        targetLanguage = PlayerPrefs.GetString("TargetLanguage", "akeanon");
        
        //baseLanguageId = PlayerPrefs.GetInt("DefaultLang", 0);
        //targetLangId = PlayerPrefs.GetInt("NativeLang", 0);
        
        introPanel.SetActive(true);
        langSelectionPanel.SetActive(false);
        enjoyPanel.SetActive(false);

        int _firstTimePlaying = PlayerPrefs.GetInt("FirstTimePlaying", 1);
    }
    
    public void DisplayLangSelection()
    {
        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            introPanel.SetActive(false);
            langSelectionPanel.SetActive(true);
            enjoyPanel.SetActive(false);
            
            langSelectionPanel.transform.GetChild(3).GetComponent<Button>().interactable = false;                   // Submit button
            
            switch (submitTurn)
            {
                case 0:
                    langSelectionAnimator.SetTrigger("DefaultOpening");
                    for (int i = 0; i < MAX_DEFAULT_LANG; i++)
                        langSelectionPanel.transform.GetChild(1).GetChild(i + 1).GetComponent<Button>().interactable = true;    // Buttons on the default languages
                    break;
                case 1:
                    langSelectionAnimator.SetTrigger("NativeOpening");
                    for (int i = 0; i < MAX_NATIVE_LANG; i++)
                        langSelectionPanel.transform.GetChild(2).GetChild(i + 1).GetComponent<Button>().interactable = true;    // Buttons on the native languages
                    break;
            }
        }
        else if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 0)
        {
            StartLearning();
        }
    }

    public void DefaultLangSelect(int baseLanguageId)
    {
        for (int i = 0; i < MAX_DEFAULT_LANG; i++)
        {
            langSelectionPanel.transform.GetChild(1).GetChild(i + 1).GetChild(0).GetComponent<Text>().font = montserratMedium;
            langSelectionPanel.transform.GetChild(1).GetChild(i + 1).GetChild(0).GetComponent<Text>().color = defaultTextColor;
            langSelectionPanel.transform.GetChild(1).GetChild(i + 1).GetChild(1).gameObject.SetActive(false);
        }

        langSelectionPanel.transform.GetChild(1).GetChild(baseLanguageId + 1).GetChild(0).GetComponent<Text>().font = montserratBold;
        langSelectionPanel.transform.GetChild(1).GetChild(baseLanguageId + 1).GetChild(0).GetComponent<Text>().color = selectedTextColor;
        langSelectionPanel.transform.GetChild(1).GetChild(baseLanguageId + 1).GetChild(1).gameObject.SetActive(true);

        langSelectionPanel.transform.GetChild(3).GetComponent<Button>().interactable = true;                    // Submit button
        PlayerPrefs.SetString("BaseLanguage", baseLanguages[baseLanguageId]);
    }

    public void NativeLangSelect(int targetLangId)
    {
        for (int i = 0; i < MAX_NATIVE_LANG; i++)
        {
            langSelectionPanel.transform.GetChild(2).GetChild(i + 1).GetChild(0).GetComponent<Text>().font = montserratMedium;
            langSelectionPanel.transform.GetChild(2).GetChild(i + 1).GetChild(0).GetComponent<Text>().color = defaultTextColor;
            langSelectionPanel.transform.GetChild(2).GetChild(i + 1).GetChild(1).gameObject.SetActive(false);
        }

        langSelectionPanel.transform.GetChild(2).GetChild(targetLangId + 1).GetChild(0).GetComponent<Text>().font = montserratBold;
        langSelectionPanel.transform.GetChild(2).GetChild(targetLangId + 1).GetChild(0).GetComponent<Text>().color = selectedTextColor;
        langSelectionPanel.transform.GetChild(2).GetChild(targetLangId + 1).GetChild(1).gameObject.SetActive(true);

        langSelectionPanel.transform.GetChild(3).GetComponent<Button>().interactable = true;                    // Submit button
        PlayerPrefs.SetString("TargetLanguage", targetLanguages[targetLangId]);
    }

    public void SubmitFunction()
    {
        switch (submitTurn)
        {
            case 0:
                for (int i = 0; i < MAX_DEFAULT_LANG; i++)
                    langSelectionPanel.transform.GetChild(1).GetChild(i + 1).GetComponent<Button>().interactable = false;   // Buttons on the default languages
                
                submitTurn = 1;
                DisplayLangSelection();
                break;
            case 1:
                for (int i = 0; i < MAX_NATIVE_LANG; i++)
                    langSelectionPanel.transform.GetChild(2).GetChild(i + 1).GetComponent<Button>().interactable = false;   // Buttons on the default languages
                
                introPanel.SetActive(false);
                langSelectionPanel.SetActive(false);
                enjoyPanel.SetActive(true);

                langText.sprite = langTextSprites[Array.IndexOf(targetLanguages, PlayerPrefs.GetString("TargetLanguage", "akeanon"))];
                enjoyAnimator.SetTrigger("EnjoyOpening");
                break;
        }
    }

    public void StartLearning()
    {
        PlayerPrefs.SetInt("FirstTimePlaying", 0);
        SceneManager.LoadScene(1);
    }
}
