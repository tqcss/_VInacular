using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
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
    public GameObject chapterContainer;
    public GameObject lectureContainer;
    public GameObject lectureButtonPrefab;

    public Color lightBgColor = new Color(255, 255, 255, 255);
    public Color darkBgColor = new Color(15, 15, 20, 255);
    public Sprite darkBarBorder;
    public Sprite lightBarBorder;

    private LifeSystem _lifeSystem;
    private CharacterSystem _characterSystem;
    private CoinSystem _coinSystem;
    private LevelSelectManager _levelSelectManager;

    /*
    void InitialDebug()
    {
        // Number of chapters unlocked
        PlayerPrefs.SetInt("ChaptersUnlocked", 5);

        // Number of lectures unlocked in chapter
        PlayerPrefs.SetInt("Chapter1Unlocked", 1);
        PlayerPrefs.SetInt("Chapter2Unlocked", 1);
        PlayerPrefs.SetInt("Chapter3Unlocked", 1);
        PlayerPrefs.SetInt("Chapter4Unlocked", 1);
        PlayerPrefs.SetInt("Chapter5Unlocked", 1);
        PlayerPrefs.SetInt("Chapter6Unlocked", 1);
        PlayerPrefs.SetInt("Chapter7Unlocked", 1);
        PlayerPrefs.SetInt("Chapter8Unlocked", 1);
        PlayerPrefs.SetInt("Chapter9Unlocked", 1); 
        PlayerPrefs.SetInt("Chapter10Unlocked", 1);

        // Lanugages
        // should be all lowercase and no dashes
        PlayerPrefs.SetString("BaseLanguage", "english");
        PlayerPrefs.SetString("TargetLanguage", "akeanon");
    }
    */

    private void Awake()
    {
        // InitialDebug();

        // Reference the scripts from game objects
        _lifeSystem = GameObject.FindGameObjectWithTag("LifeSystem").GetComponent<LifeSystem>();
        _characterSystem = GameObject.FindGameObjectWithTag("CharacterSystem").GetComponent<CharacterSystem>();
        _coinSystem = GameObject.FindGameObjectWithTag("CoinSystem").GetComponent<CoinSystem>();
        _levelSelectManager = GameObject.FindGameObjectWithTag("LevelSelectManager").GetComponent<LevelSelectManager>();

        // Initialize private members
        chapters = Resources.LoadAll<Chapter>("Chapters");

        _levelSelectManager.DisplayLevelSelection();

        /* Make unlocked chapter buttons interactable
        int chaptersUnlocked = PlayerPrefs.GetInt("ChaptersUnlocked");
        for (int i = 0; i < chaptersUnlocked; i++)
        {
            chapterContainer.GetComponentsInChildren<Button>()[i].interactable = true;
        }
        */
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

        levsecEnergyBar.transform.GetComponent<Slider>().value = _c_energy / 100.0f;
        levsecEnergyBar.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:0}%", _c_energy);

        levsecLifeBar.transform.GetComponent<Slider>().value = (_globalLives * 20.0f) / 100.0f;
        if (_globalLives < 5) 
            { levsecLifeBar.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(_onLifeCooldown / 60), Mathf.FloorToInt(_onLifeCooldown % 60)); }
        else if (_globalLives == 5)
            { levsecLifeBar.transform.GetChild(3).GetComponent<Text>().text = "Full"; }
    }

    /*
    public void LoadLectures(int chapterIndex)
    {
        currentChapter = chapters[chapterIndex];

        // Destroy old buttons in container and replace with new, updated buttons
        foreach (Transform child in lectureContainer.transform)
        {
            Destroy(child.gameObject);
            Debug.Log("Destroyed");
        }

        for (int i = 0; i < currentChapter.lectures.Length; i++)
        {
            GameObject lectureButton = Instantiate(lectureButtonPrefab, lectureContainer.transform);
            lectureButton.transform.GetComponentInChildren<Text>().text = currentChapter.lectures[i].title;
            lectureButton.transform.GetComponent<LectureReference>().lectureInfo = currentChapter.lectures[i];
        }

        for (int i = 0; i < PlayerPrefs.GetInt($"Chapter{chapterIndex + 1}Unlocked"); i++)
        {
            lectureContainer.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }
    */
}
