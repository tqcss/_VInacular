using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    private string[] baseLanguages = {"english", "filipino"};
    private string[] targetLanguages = {"akeanon", "capiznon", "hiligaynon", "kinaraya"};
    private string[] langChaptersPrefs = {"AkeanonChapters", "CapiznonChapters", "HiligaynonChapters", "KinarayaChapters"};

    private Chapter[] chapters = new Chapter[10];
    private Chapter currentChapter;
    
    private GameObject levelButton;
    public GameObject levelContent;
    public GameObject startButton;
    public Scrollbar scrollbar;
    public Sprite[] levelButtonSprites;

    public int levelSelected;
    public int unlockedLevels;
    private StageManager _stageManager;

    private void Awake()
    {
        _stageManager = GameObject.FindGameObjectWithTag("MainScript").GetComponent<StageManager>();
        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        string _targetLanguage = PlayerPrefs.GetString("TargetLanguage");
        unlockedLevels = PlayerPrefs.GetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", 1);
    }

    private void Update()
    {
        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        string _targetLanguage = PlayerPrefs.GetString("TargetLanguage");
        unlockedLevels = PlayerPrefs.GetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", 1);
    }

    public void AddLevel()
    {
        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        string _targetLanguage = PlayerPrefs.GetString("TargetLanguage");
        PlayerPrefs.SetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", PlayerPrefs.GetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", 1) + 1);
        RemoveLevelButtons();
        DisplayLevelSelection();
    }

    public void DisplayLevelSelection()
    {
        chapters = Resources.LoadAll<Chapter>("Chapters");
        startButton.transform.GetComponent<Button>().interactable = false;
        levelButton = Resources.Load("Prefabs/levelButton", typeof(GameObject)) as GameObject;

        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        string _targetLanguage = PlayerPrefs.GetString("TargetLanguage");
        int _unlockedLevels = PlayerPrefs.GetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", 1);
        currentChapter = chapters[_selectedChapter - 1];

        Debug.Log(currentChapter);

        for (int i = 1; i <= 1; i++)
        {
            GameObject newHiddenButton = Instantiate(levelButton, levelContent.transform);
            newHiddenButton.transform.GetComponent<Image>().sprite = null;
            newHiddenButton.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        
        for (int i = 0; i < currentChapter.lectures.Length; i++)
        {
            GameObject newLevelButton = Instantiate(levelButton, levelContent.transform);
            
            if (_unlockedLevels < i + 1)
            { 
                newLevelButton.transform.GetComponent<Button>().interactable = false;
                newLevelButton.transform.GetComponent<Image>().sprite = levelButtonSprites[0];
            }
            else if (_unlockedLevels == i + 1)
            { 
                int _levelSelected = i + 1;
                newLevelButton.transform.GetComponent<Button>().interactable = true;
                newLevelButton.transform.GetComponent<Image>().sprite = levelButtonSprites[1];
                newLevelButton.transform.GetChild(0).GetComponent<Text>().text = currentChapter.lectures[i].title;
                newLevelButton.transform.GetComponent<LectureReference>().lectureInfo = currentChapter.lectures[i];
                  
                EventTrigger clickTrigger = newLevelButton.transform.GetComponent<EventTrigger>();
                EventTrigger.Entry clickEvent = new EventTrigger.Entry()
                {
                    eventID = EventTriggerType.PointerDown
                };

                clickEvent.callback.AddListener((data) => { SelectLevel(_levelSelected); });
                clickTrigger.triggers.Add(clickEvent);
            }
            else if (_unlockedLevels > i + 1)
            {
                newLevelButton.transform.GetComponent<Button>().interactable = false;
                newLevelButton.transform.GetComponent<Image>().sprite = levelButtonSprites[2];
                newLevelButton.transform.GetChild(0).GetComponent<Text>().text = currentChapter.lectures[i].title;
            }
        }

        for (int i = 1; i <= 2; i++)
        {
            GameObject newHiddenButton = Instantiate(levelButton, levelContent.transform);
            newHiddenButton.transform.GetComponent<Image>().sprite = null;
            newHiddenButton.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        StartCoroutine(UpdateDisplay(currentChapter.lectures.Length));
    }

    private IEnumerator UpdateDisplay(int totalLevels)
    {
        levelContent.GetComponent<RectTransform>().sizeDelta = new Vector2
            (levelContent.GetComponent<RectTransform>().sizeDelta.x, 
            (levelContent.GetComponent<GridLayoutGroup>().cellSize.y + levelContent.GetComponent<GridLayoutGroup>().spacing.y) * Mathf.CeilToInt(totalLevels));
        yield return new WaitForSeconds(0.01f);
        scrollbar.value = 0f;
    }

    public void RemoveLevelButtons()
    {
        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        currentChapter = chapters[_selectedChapter - 1];
        
        for (int i = 0; i < currentChapter.lectures.Length + 3; i++)
        {
            Destroy(levelContent.transform.GetChild(i).gameObject);
        }
    }

    public void SelectLevel(int level)
    {
        levelSelected = level;
        startButton.transform.GetComponent<Button>().interactable = true;

        
    }

    public void GoBackToHome()
    {
        RemoveLevelButtons();
        SceneManager.LoadScene(1);
    }
}
