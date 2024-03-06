using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LectureManager : MonoBehaviour
{
    private int page = 0;
    private List<int> choiceSequence = new List<int>{0, 1, 2, 3};

    public Lecture lectureInfo;
    public Text text1; // text that displays base langauage or question/prompt
    public Text text2; // text used for target language
    public GameObject[] choiceButtons;
    public string[] choicePrompt;
    public string[] choiceAnswers;

    // void ShuffleList<T>(List<T> list)
    // {
    //     int n = list.Count;
    //     while (n > 1)
    //     {
    //         n--;
    //         int k = Random.Range(0, n + 1);
    //         T value = list[k];
    //         list[k] = list[n];
    //         list[n] = value;
    //     }
    // }

    public void choiceIncorrect() {
        // enter functionality when player makes a wring choice in a mulitple choice uqestion
    }

    public void choiceCorrect() {
        if (page >= 4) {
            foreach (GameObject button in choiceButtons) {
                button.GetComponent<Button>().onClick.AddListener(choiceIncorrect);
            }

            page++;
            text1.text = choicePrompt[choiceSequence[page]];
            choiceButtons[choiceSequence[page]].GetComponent<Button>().onClick.AddListener(choiceCorrect);
        } else {
            // enter functinality here for after finished answering all multiple choice questions
        }
    }

    public void LoadUi()
    {
        lectureInfo.loadLanguages();
        string baseLang = PlayerPrefs.GetString("BaseLanguage", "english");
        string targetLang = PlayerPrefs.GetString("TargetLang", "akeanon");

        switch (lectureInfo.lectureType) {
            case 0: // LECTURE
                if (PlayerPrefs.GetString("BaseLanguage", "english") == "english")
                {
                    text1.text = $"Learn some new {targetLang.ToUpper()} words:";
                }
                else
                {
                    text1.text = $"Matuto ng bagong {targetLang.ToUpper()} mga salita:";
                }

                for (int i = 0; i < lectureInfo.language[baseLang].Length; i++)
                {
                    text2.text += $"{lectureInfo.language[baseLang][i]}: {lectureInfo.language[targetLang][i]}\n";
                }

                
                break;
            case 1: // MULTIPLE CHOICE
                
                break;
            case 2:
                
                break;
            case 3: // SPELL CHECKING

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:
            
                break;
        }
    }
}
