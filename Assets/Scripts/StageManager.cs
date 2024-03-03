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
    public GameObject chapterUi;
    public GameObject lectureUi;
    public GameObject chapterContainer;
    public GameObject lectureContainer;
    public GameObject lectureButtonPrefab;

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

    void Awake()
    {
        InitialDebug();

        // Initialize private members
        chapters = Resources.LoadAll<Chapter>("Chapters");

        // Make unlocked chapter buttons interactable
        int chaptersUnlocked = PlayerPrefs.GetInt("ChaptersUnlocked");
        for (int i = 0; i < chaptersUnlocked; i++)
        {
            chapterContainer.GetComponentsInChildren<Button>()[i].interactable = true;
        }
    }

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
}
