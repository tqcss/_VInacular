using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScript : MonoBehaviour
{
    private string[] baseLanguages = {"english", "filipino"};
    private string[] targetLanguages = {"akeanon", "capiznon", "hiligaynon", "kinaraya"};
    private List<string> choiceLanguages = new List<string>();
    public string[] languageDescription;
    
    public GameObject mainBackground;
    public GameObject petfeedPanel;
    public GameObject dictionaryPanel;
    public GameObject menuPanel;
    public GameObject profilePanel;
    public GameObject settingsPanel;
    public GameObject langPickerPanel;

    public Color lightBgColor = new Color(255, 255, 255, 255);
    public Color darkBgColor = new Color(15, 15, 20, 255);
    public Sprite lightBarBorder;
    public Sprite darkBarBorder;
    public Sprite lightBarBorderLong;
    public Sprite darkBarBorderLong;
    public Sprite[] languageTags;

    public int akeanonChapters, capiznonChapters, hiligaynonChapters, kinarayaChapters;
    private const int MAX_CHAPTER = 5;
    private const int MAX_PANEL = 5;
    private const int MAX_NATIVE_LANG = 4;

    private DisplayOnHome _displayOnHome;
    private ExperienceSystem _experienceSystem;
    private LevelSelectManager _levelSelectManager;
    
    private void Start()
    {
        // Reference the scripts from game objects
        _displayOnHome = GameObject.FindGameObjectWithTag("MainScript").GetComponent<DisplayOnHome>();
        _experienceSystem = GameObject.FindGameObjectWithTag("ExperienceSystem").GetComponent<ExperienceSystem>();

        akeanonChapters = PlayerPrefs.GetInt("AkeanonChapters", 1);
        capiznonChapters = PlayerPrefs.GetInt("CapiznonChapters", 1);
        hiligaynonChapters = PlayerPrefs.GetInt("HiligaynonChapters", 1);
        kinarayaChapters = PlayerPrefs.GetInt("KinarayaChapters", 1);
        
        mainBackground.SetActive(true);
        menuPanel.SetActive(true);
        langPickerPanel.SetActive(true);
        langPickerPanel.transform.GetChild(1).gameObject.SetActive(false);
        
        petfeedPanel.SetActive(false);
        dictionaryPanel.SetActive(false);
        profilePanel.SetActive(false);
        settingsPanel.SetActive(false);
        DisplayTag();
    }

    public void Update()
    {
        akeanonChapters = PlayerPrefs.GetInt("AkeanonChapters", 1);
        capiznonChapters = PlayerPrefs.GetInt("CapiznonChapters", 1);
        hiligaynonChapters = PlayerPrefs.GetInt("HiligaynonChapters", 1);
        kinarayaChapters = PlayerPrefs.GetInt("KinarayaChapters", 1);
        
        _displayOnHome.DisplayRank();
        _displayOnHome.DisplayPlayerStats();
        _displayOnHome.DisplayHealthStats();
        _displayOnHome.DisplayDialogue();
        _displayOnHome.DisplayCoins();
        _displayOnHome.DisplayProfileStats();

        mainBackground.transform.GetChild(0).GetComponent<Image>().color = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkBgColor : lightBgColor;
        menuPanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Image>().sprite = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkBarBorder : lightBarBorder;
        menuPanel.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Image>().sprite = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkBarBorder : lightBarBorder;
        petfeedPanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>().sprite = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkBarBorder : lightBarBorder;
        petfeedPanel.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<Image>().sprite = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkBarBorder : lightBarBorder;
        petfeedPanel.transform.GetChild(1).GetChild(3).GetChild(1).GetComponent<Image>().sprite = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkBarBorderLong : lightBarBorderLong;
    }

    public void PanelDisplay(int panelId)
    {
        petfeedPanel.SetActive(false);
        dictionaryPanel.SetActive(false);
        menuPanel.SetActive(false);
        profilePanel.SetActive(false);
        settingsPanel.SetActive(false);
        langPickerPanel.SetActive(false);
        langPickerPanel.transform.GetChild(1).gameObject.SetActive(false);
        mainBackground.transform.GetChild(1).gameObject.SetActive(true);

        switch (panelId)
        {
            case 0: petfeedPanel.SetActive(true); break;
            case 1: dictionaryPanel.SetActive(true); break;
            case 2: menuPanel.SetActive(true); langPickerPanel.SetActive(true); break;
            case 3: profilePanel.SetActive(true); break;
            case 4: settingsPanel.SetActive(true); break;
            case 5: menuPanel.SetActive(true); langPickerPanel.SetActive(true); langPickerPanel.transform.GetChild(1).gameObject.SetActive(true); DisplayLangSelector(); break;
            case 6: SceneManager.LoadScene(2); break;
        }
    }

    public void PanelSelect(int panelId)
    {
        for (int i = 0; i < MAX_PANEL; i++)
        {
            mainBackground.transform.GetChild(1).GetChild(i).localScale = new Vector2(1.0f, 1.0f);
        }
        mainBackground.transform.GetChild(1).GetChild(panelId).localScale = new Vector2(1.3f, 1.3f);
        PanelDisplay(panelId);
    }

    public void ChapterSelect(int chapter)
    {
        PlayerPrefs.SetInt("SelectedChapter", chapter);
        PanelDisplay(6);
    }

    public void DisplayTag()
    {
        int _targetLangId = Array.IndexOf(targetLanguages, PlayerPrefs.GetString("TargetLanguage", "akeanon"));
        langPickerPanel.transform.GetChild(0).GetComponent<Image>().sprite = languageTags[_targetLangId];
    }

    public void DisplayLangSelector()
    {
        int _targetLangId = Array.IndexOf(targetLanguages, PlayerPrefs.GetString("TargetLanguage", "akeanon"));
        int _globalRank = PlayerPrefs.GetInt(_experienceSystem.globalLangRanks[_targetLangId], 1);
        float _globalExperience = PlayerPrefs.GetFloat(_experienceSystem.globalLangExperience[_targetLangId], 0);

        langPickerPanel.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text = targetLanguages[_targetLangId].ToUpper();
        langPickerPanel.transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<Text>().text = languageDescription[_targetLangId];
        langPickerPanel.transform.GetChild(1).GetChild(0).GetChild(3).GetChild(2).GetComponent<Text>().text = "Rank: " + _displayOnHome.rankNames[_globalRank];
        langPickerPanel.transform.GetChild(1).GetChild(0).GetChild(3).GetChild(3).GetComponent<Text>().text = string.Format("Exp: {0:0.0}", _globalExperience);

        for (int i = 0; i < MAX_NATIVE_LANG - 1; i++)
        {
            langPickerPanel.transform.GetChild(1).GetChild(i + 2).GetComponent<Button>().onClick.RemoveAllListeners();
        }

        int optionCount = 0;
        for (int i = 0; i < MAX_NATIVE_LANG; i++)
        {
            if (i == _targetLangId)
            {
                continue;
            }

            int _i = i;
            langPickerPanel.transform.GetChild(1).GetChild(optionCount + 2).GetChild(0).GetComponent<Text>().text = targetLanguages[i].ToUpper();
            langPickerPanel.transform.GetChild(1).GetChild(optionCount + 2).GetComponent<Button>().onClick.AddListener(() => { OptionClicked(_i); });
            optionCount++;
        }
    }

    public void OptionClicked(int targetId)
    {   
        PlayerPrefs.SetString("TargetLanguage", targetLanguages[targetId]);
        PanelDisplay(2);
        DisplayTag();
    }
}
