using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LectureManager : MonoBehaviour
{
    private int page = 0;
    private List<int> choiceSequence = new List<int>{0, 1, 2, 3};

    private Chapter[] chapters = new Chapter[10];
    private Chapter currentChapter;

    public Lecture lectureInfo;
    public Text text1; // text that displays base langauage or question/prompt
    public Text text2; // text used for target language
    public GameObject[] choiceButtons;
    public GameObject submitButton;
    public int chosenAnswer;

    private CoinSystem _coinSystem;
    private ExperienceSystem _experienceSystem;
    private LifeSystem _lifeSystem;
    private StageManager _stageManager;

    private void Awake()
    {
        chapters = Resources.LoadAll<Chapter>("Chapters");
        _coinSystem = GameObject.FindGameObjectWithTag("CoinSystem").GetComponent<CoinSystem>();
        _experienceSystem = GameObject.FindGameObjectWithTag("ExperienceSystem").GetComponent<ExperienceSystem>();
        _lifeSystem = GameObject.FindGameObjectWithTag("LifeSystem").GetComponent<LifeSystem>();
        _stageManager = GameObject.FindGameObjectWithTag("MainScript").GetComponent<StageManager>();
    }

    public void ChoiceIncorrect() 
    {
        int _globalLives = PlayerPrefs.GetInt("GlobalLives", _lifeSystem.maximumLife);
        int _failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);

        PlayerPrefs.SetInt("GlobalLives", _globalLives - 1);
        PlayerPrefs.SetInt("FailsBeforeSuccess", _failsBeforeSuccess + 1);

        _stageManager.DisplayValidate(1);
        submitButton.GetComponent<Button>().interactable = false;

        Debug.Log("Lives: " + PlayerPrefs.GetInt("GlobalLives", _lifeSystem.maximumLife));
        Debug.Log("Fails: " + PlayerPrefs.GetInt("FailsBeforeSuccess", 0));
    }

    public void TryAgainLecture()
    {
        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        string _targetLanguage = PlayerPrefs.GetString("TargetLanguage");
        
        _stageManager.RefreshLectureUi(PlayerPrefs.GetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", 1));
    }

    public void ChoiceCorrect() 
    {
        const float INITIAL_EXP = 1.6f;
        const float INITIAL_COIN = 8;

        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        int _failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);

        float experienceGrant = INITIAL_EXP * (((_selectedChapter - 1) * 0.2f) + 1);
        _experienceSystem.IncreaseExperience(experienceGrant);

        float coinsGrant = INITIAL_COIN * (((_selectedChapter - 1) * 0.5f) + 1);
        _coinSystem.IncreaseCoins(coinsGrant);

        _lifeSystem.RewardLife(_failsBeforeSuccess, true);
        PlayerPrefs.SetInt("FailsBeforeSuccess", 0);

        _stageManager.DisplayValidate(0);
        submitButton.GetComponent<Button>().interactable = false;

        FinishActivity();
        Debug.Log("Granted Experience: " + experienceGrant);
    }

    public void FinishActivity()
    {
        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        string _targetLanguage = PlayerPrefs.GetString("TargetLanguage");
        PlayerPrefs.SetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", PlayerPrefs.GetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", 1) + 1);
    }

    public void FinishLecture()
    {
        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        string _targetLanguage = PlayerPrefs.GetString("TargetLanguage");
        PlayerPrefs.SetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", PlayerPrefs.GetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", 1) + 1);

        GoToNextLecture();
    }

    public void GoToNextLecture()
    {
        Debug.Log("Go to next lecture");

        int _selectedChapter = PlayerPrefs.GetInt("SelectedChapter", 1);
        string _targetLanguage = PlayerPrefs.GetString("TargetLanguage");
        currentChapter = chapters[_selectedChapter - 1];

        if (PlayerPrefs.GetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", 1) > currentChapter.lectures.Length)
        {
            Debug.Log("Session finished");
            _stageManager.DisplaySessionCleared();
        }
        else
        {
            Debug.Log("Next lecture");
            _stageManager.RefreshLectureUi(PlayerPrefs.GetInt($"{_targetLanguage}Chapter{_selectedChapter}Unlocked", 1));
        }
    }

    public void ChooseAnswer(int answerId)
    {
        submitButton.GetComponent<Button>().interactable = true;
        chosenAnswer = answerId;
    }

    public void SubmitAnswer()
    {
        if (chosenAnswer == lectureInfo.correctChoice)
            ChoiceCorrect();
        else
            ChoiceIncorrect();
        _stageManager.lectureUi.transform.GetChild(12).gameObject.SetActive(true);
    }

    public void LoadUi()
    {
        lectureInfo.LoadLanguages();
        string baseLang = PlayerPrefs.GetString("BaseLanguage", "english");
        string targetLang = PlayerPrefs.GetString("TargetLanguage", "akeanon");

        text1.text = null;
        text2.text = null;
        _stageManager.lectureUi.transform.GetChild(12).gameObject.SetActive(false);


        switch ((int)lectureInfo.lectureType) {
            case 0: // LECTURE
                text1.text = $"{lectureInfo.titles[baseLang]}";
                _stageManager.lectureUi.transform.GetChild(12).gameObject.SetActive(true);

                for (int i = 0; i < lectureInfo.wordEntries[baseLang].Length; i++)
                {
                    text2.text += $"{lectureInfo.wordEntries[baseLang][i]} = {lectureInfo.wordEntries[targetLang][i]}\n";
                }
                break;

            case 1: // TRANSLATE: MULTIPLE CHOICE
                text1.text = $"{lectureInfo.questions[baseLang]}";
                text2.text = $"{baseLang.ToUpper()}";
                submitButton.GetComponent<Button>().interactable = false;

                for (int i = 0; i < lectureInfo.choiceEntries[targetLang].Length; i++)
                {
                    choiceButtons[i].transform.GetChild(0).GetComponent<Text>().text = $"{lectureInfo.choiceEntries[targetLang][i]}";
                }
                break;

            case 2:
                
                break;
            case 3: // SPELL CHECKING
                text1.text = $"{lectureInfo.questions[baseLang]}";
                text2.text = $"{baseLang.ToUpper()}";
                submitButton.GetComponent<Button>().interactable = false;

                for (int i = 0; i < lectureInfo.choiceEntries[targetLang].Length; i++)
                {
                    choiceButtons[i].transform.GetChild(0).GetComponent<Text>().text = $"{lectureInfo.choiceEntries[targetLang][i]}";
                }
                
                break;
            case 4: // GRAMMAR-CHECKING
                text1.text = $"{lectureInfo.questions[baseLang]}";
                text2.text = $"{baseLang.ToUpper()}";
                submitButton.GetComponent<Button>().interactable = false;

                for (int i = 0; i < lectureInfo.choiceEntries[targetLang].Length; i++)
                {
                    choiceButtons[i].transform.GetChild(0).GetComponent<Text>().text = $"{lectureInfo.choiceEntries[targetLang][i]}";
                }
                
                break;
            case 5:

                break;
            case 6:

                break;
            case 7: // CONVERSATE: MULTIPLE CHOICE
                text1.text = $"{lectureInfo.questions[baseLang]}";
                submitButton.GetComponent<Button>().interactable = false;

                for (int i = 0; i < lectureInfo.choiceEntries[targetLang].Length; i++)
                {
                    choiceButtons[i].transform.GetChild(0).GetComponent<Text>().text = $"{lectureInfo.choiceEntries[targetLang][i]}";
                }

                break;
            case 8:

                break;
            case 9:
            
                break;
        }
    }
}
