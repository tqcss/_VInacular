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

    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

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

    public void LoadUi() {
        lectureInfo.loadLanguages();

        switch (lectureInfo.lectureType) {
            case 0: // LECTURE
                text1.text = lectureInfo.language[PlayerPrefs.GetString("BaseLanguage")];
                text2.text = lectureInfo.language[PlayerPrefs.GetString("TargetLanguage")];
                break;
            case 1: // MULTIPLE CHOICE
                for (int i = 0; i < 4; i++) {
                    choiceButtons[i].GetComponent<Text>().text = choiceAnswers[i];
                }
                foreach (GameObject button in choiceButtons) {
                    button.GetComponent<Button>().onClick.AddListener(choiceIncorrect);
                }

                ShuffleList(choiceSequence);
                text1.text = choicePrompt[choiceSequence[page]];
                choiceButtons[choiceSequence[page]].GetComponent<Button>().onClick.AddListener(choiceCorrect);
                break;
            case 2:
                // skip sentence construction for now
                break;
            case 3: // SPELL CHECKING

                break;
        }
    }
}
