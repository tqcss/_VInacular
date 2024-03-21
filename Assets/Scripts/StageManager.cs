using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Reflection;

public class StageManager : MonoBehaviour
{
    private Chapter[] chapters = new Chapter[10];
    private Chapter currentChapter;

    public GameObject mainBackground;
    public GameObject levelSelectUi;
    public GameObject lectureUi;
    public GameObject levsecEnergyBar;
    public GameObject levsecLifeBar;
    public GameObject validateUi;
    public GameObject sessionClearedUi;

    public Color lightBgColor = new Color(255, 255, 255, 255);
    public Color darkBgColor = new Color(15, 15, 20, 255);
    public Sprite lightBarBorder;
    public Sprite darkBarBorder;
    public Sprite lightRectBlur;
    public Sprite darkRectBlur;

    private LifeSystem _lifeSystem;
    private CharacterSystem _characterSystem;
    private CoinSystem _coinSystem;
    private LevelSelectManager _levelSelectManager;

    private void Awake()
    {
        // Reference the scripts from game objects
        _lifeSystem = GameObject.FindGameObjectWithTag("LifeSystem").GetComponent<LifeSystem>();
        _characterSystem = GameObject.FindGameObjectWithTag("CharacterSystem").GetComponent<CharacterSystem>();
        _coinSystem = GameObject.FindGameObjectWithTag("CoinSystem").GetComponent<CoinSystem>();
        _levelSelectManager = GameObject.FindGameObjectWithTag("LevelSelectManager").GetComponent<LevelSelectManager>();

        // Initialize private members
        chapters = Resources.LoadAll<Chapter>("Chapters");

        _levelSelectManager.DisplayLevelSelection();
        levelSelectUi.SetActive(true);
        lectureUi.SetActive(false);
        validateUi.SetActive(false);
    }

    private void Update()
    {
        DisplayPlayerStats();
    }

    private void DisplayPlayerStats()
    {
        float _c_energy = PlayerPrefs.GetFloat("C-Energy", 50);
        int _globalLives = PlayerPrefs.GetInt("GlobalLives", _lifeSystem.maximumLife);
        float _onLifeCooldown = PlayerPrefs.GetFloat("OnLifeCooldown", _lifeSystem.maximumLifeCooldown);
        
        mainBackground.transform.GetChild(0).GetComponent<Image>().color = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkBgColor : lightBgColor;
        levelSelectUi.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>().sprite = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkBarBorder : lightBarBorder;
        levelSelectUi.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Image>().sprite = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkBarBorder : lightBarBorder;
        levelSelectUi.transform.GetChild(1).GetChild(3).GetComponent<Image>().sprite = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? darkRectBlur : lightRectBlur;

        levsecEnergyBar.transform.GetComponent<Slider>().value = _c_energy / 100.0f;
        levsecEnergyBar.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:0}%", _c_energy);

        levsecLifeBar.transform.GetComponent<Slider>().value = (_globalLives * 20.0f) / 100.0f;
        if (_globalLives < 5) 
            { levsecLifeBar.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(_onLifeCooldown / 60), Mathf.FloorToInt(_onLifeCooldown % 60)); }
        else if (_globalLives == 5)
            { levsecLifeBar.transform.GetChild(3).GetComponent<Text>().text = "Full"; }
    }

    public void RefreshLectureUi(int nextLecture)
    {
        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        currentChapter = chapters[_selectedChapter - 1];

        GameObject nextButton = GameObject.FindGameObjectWithTag("nextButton");
        nextButton.transform.GetComponent<LectureReference>().lectureInfo = currentChapter.lectures[nextLecture - 1];
        
        for (int i = 0; i < lectureUi.transform.childCount - 1; i++)
        {
            lectureUi.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        validateUi.SetActive(false);
        validateUi.transform.GetChild(0).gameObject.SetActive(false);
        validateUi.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void DisplayValidate(int mode)
    {
        validateUi.SetActive(true);
        validateUi.transform.GetChild(mode).gameObject.SetActive(true);
    }

    public void DisplaySessionCleared()
    {
        sessionClearedUi.SetActive(true);
    
        // Temporary Display
        sessionClearedUi.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "Coins Earned: ";
        sessionClearedUi.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "XP Earned: ";
    }

    public void GoBackToMain()
    {
        for (int i = 0; i < lectureUi.transform.childCount - 1; i++)
        {
            lectureUi.transform.GetChild(i).gameObject.SetActive(false);
        }

        lectureUi.SetActive(false);
        levelSelectUi.SetActive(true);
        _levelSelectManager.DisplayLevelSelection();
        _levelSelectManager.RemoveLevelButtons();
    }
}
