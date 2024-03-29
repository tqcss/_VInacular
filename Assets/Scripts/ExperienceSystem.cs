using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExperienceSystem : MonoBehaviour
{
    public int globalRank;
    public float globalExperience;
    public float expAmountNeeded;
    public float baseExpAmount;
    public float scaleExpAmount;
    public int maximumRank;

    public string[] globalLangRanks = {"AkeanonRank", "CapiznonRank", "HiligaynonRank", "KinarayaRank"};
    public string[] globalLangExperience = {"AkeanonExperience", "CapiznonExperience", "HiligaynonExperience", "KinarayaExperience"};

    private DisplayOnHome _displayOnHome;
    private static GameObject s_instance {set; get;}

    private void Awake()
    {
        // Will not destroy the script when on the next loaded scene
        if (s_instance != null) 
            Destroy(s_instance);
        s_instance = gameObject;
        DontDestroyOnLoad(s_instance);

        // Reference the scripts from game objects
        try { _displayOnHome = GameObject.FindGameObjectWithTag("MainScript").GetComponent<DisplayOnHome>(); }
        catch (UnityException) {};

        // Set initial values to the variables
        int _nativeLangId = PlayerPrefs.GetInt("NativeLang", 0);
        globalRank = PlayerPrefs.GetInt(globalLangRanks[_nativeLangId], 1);
        globalExperience = PlayerPrefs.GetFloat(globalLangExperience[_nativeLangId], 0);
        ComputeExpAmountNeeded();
    }

    private void Update()
    {
        // Automatically update the player global coins
        int _nativeLangId = PlayerPrefs.GetInt("NativeLang", 0);
        
        globalRank = PlayerPrefs.GetInt(globalLangRanks[_nativeLangId], 1);
        globalExperience = PlayerPrefs.GetFloat(globalLangExperience[_nativeLangId], 0);
        PlayerPrefs.SetFloat("ExpAmountNeeded", expAmountNeeded);
    }

    public float ComputeExpAmountNeeded()
    {
        int _nativeLangId = PlayerPrefs.GetInt("NativeLang", 0);
        
        int _globalRank = PlayerPrefs.GetInt(globalLangRanks[_nativeLangId], 1);
        float _expAmountNeeded = 0;

        // Get the summation of the formula from 0 to the current player global rank
        for (int i = 0; i < _globalRank; i++)
            _expAmountNeeded += (float)Math.Floor(Mathf.Pow(Mathf.Pow(i, 2.5f), scaleExpAmount) + baseExpAmount);

        expAmountNeeded = _expAmountNeeded;
        return _expAmountNeeded;
    }

    public void IncreaseExperience(float increase)
    {
        int _nativeLangId = PlayerPrefs.GetInt("NativeLang", 0);
        
        int _globalRank = PlayerPrefs.GetInt(globalLangRanks[_nativeLangId], 1);
        float _globalExperience = 0;
        float _expAmountNeeded = ComputeExpAmountNeeded();

        // Increase experience by the amount prompted
        _globalExperience = PlayerPrefs.GetFloat(globalLangExperience[_nativeLangId], 0);
        PlayerPrefs.SetFloat(globalLangExperience[_nativeLangId], _globalExperience + increase);
        
        // Check if the player global experience points is greater than or equal to the experience amount needed
        _globalExperience = PlayerPrefs.GetFloat(globalLangExperience[_nativeLangId], 0);
        if (_globalExperience >= _expAmountNeeded) { RankUp(_globalExperience - _expAmountNeeded); }
    }

    public void DecreaseExperience(float decrease)
    {
        int _nativeLangId = PlayerPrefs.GetInt("NativeLang", 0);
        
        int _globalRank = PlayerPrefs.GetInt(globalLangRanks[_nativeLangId], 1);
        float _globalExperience = 0;

        // Decrease experience by the amount prompted
        _globalExperience = PlayerPrefs.GetFloat(globalLangExperience[_nativeLangId], 0);
        bool isBeyondMinimum = (_globalRank > 1) || (_globalExperience >= decrease);
        PlayerPrefs.SetFloat(globalLangExperience[_nativeLangId], (isBeyondMinimum) ? _globalExperience - decrease : 0);

        // Check if the player global experience points reaches negative after deducting with the prompted amount
        _globalExperience = PlayerPrefs.GetFloat(globalLangExperience[_nativeLangId], 0);
        if (_globalExperience < 0) { RankDown(Mathf.Abs(_globalExperience)); }
    }

    public void RankUp(float remainder)
    {
        int _nativeLangId = PlayerPrefs.GetInt("NativeLang", 0);
        
        int _globalRank = PlayerPrefs.GetInt(globalLangRanks[_nativeLangId], 1);
        float _expAmountNeeded = ComputeExpAmountNeeded();
        bool isMaximumReached = _globalRank >= maximumRank;

        // Rank up the user by one and reset the experience points
        PlayerPrefs.SetInt(globalLangRanks[_nativeLangId], (isMaximumReached) ? _globalRank : _globalRank + 1);
        PlayerPrefs.SetFloat(globalLangExperience[_nativeLangId], (isMaximumReached) ? _expAmountNeeded : remainder);
        ComputeExpAmountNeeded();
    }

    public void RankDown(float remainder)
    {
        int _nativeLangId = PlayerPrefs.GetInt("NativeLang", 0);
        
        // Rank down the user by one
        int _globalRank = PlayerPrefs.GetInt(globalLangRanks[_nativeLangId], 1);
        PlayerPrefs.SetInt(globalLangRanks[_nativeLangId], _globalRank - 1);

        // Reduce the maximum with the remaining experience points after ranking down
        float _expAmountNeeded = ComputeExpAmountNeeded();
        PlayerPrefs.SetFloat(globalLangExperience[_nativeLangId], _expAmountNeeded - remainder);
    }
}
