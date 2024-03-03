using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LectureReference : MonoBehaviour
{
    public Lecture lectureInfo;
    private StageManager stageManager;

    public void Start() {
        stageManager = GameObject.FindGameObjectWithTag("scripts").GetComponent<StageManager>();
        gameObject.GetComponent<Button>().onClick.AddListener(OnButtonPress);
    }

    public void OnButtonPress() {
        stageManager.chapterUi.SetActive(false);
        stageManager.lectureUi.SetActive(true);

        GameObject currentLecture = stageManager.lectureUi.transform.GetChild(lectureInfo.lectureType).gameObject;
        currentLecture.SetActive(true);

        LectureManager lectureManager = currentLecture.GetComponent<LectureManager>();
        lectureManager.lectureInfo = lectureInfo;
        lectureManager.LoadUi();
    }
}
