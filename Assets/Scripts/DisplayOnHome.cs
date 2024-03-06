using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayOnHome : MonoBehaviour
{
    public Image expRankImage;
    public string[] rankNames;
    public Sprite[] rankImages;

    public GameObject statsProfile; 
    public GameObject menuEnergyBar;
    public GameObject menuLifeBar;

    public GameObject feedHungerBar;
    public GameObject feedEnergyBar;
    public GameObject feedFatsBar;
    public GameObject feedCoinsBar;
    public GameObject dialogueBubble;
    public GameObject character;

    private LifeSystem _lifeSystem;
    private CharacterSystem _characterSystem;
    private CoinSystem _coinSystem;

    private void Awake()
    {
        // Reference the scripts from game objects
        _lifeSystem = GameObject.FindGameObjectWithTag("LifeSystem").GetComponent<LifeSystem>();
        _characterSystem = GameObject.FindGameObjectWithTag("CharacterSystem").GetComponent<CharacterSystem>();
        _coinSystem = GameObject.FindGameObjectWithTag("CoinSystem").GetComponent<CoinSystem>();
    }
    
    public void DisplayRank()
    {
        int _globalRank = PlayerPrefs.GetInt("GlobalRank", 1);
        expRankImage.sprite = rankImages[_globalRank - 1];
    }

    public void DisplayPlayerStats()
    {
        float _c_energy = PlayerPrefs.GetFloat("C-Energy", 50);
        int _globalLives = PlayerPrefs.GetInt("GlobalLives", _lifeSystem.maximumLife);
        float _onLifeCooldown = PlayerPrefs.GetFloat("OnLifeCooldown", _lifeSystem.maximumLifeCooldown);
        
        menuEnergyBar.transform.GetComponent<Slider>().value = _c_energy / 100.0f;
        menuEnergyBar.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:0}%", _c_energy);

        menuLifeBar.transform.GetComponent<Slider>().value = (_globalLives * 20.0f) / 100.0f;
        if (_globalLives < 5) 
            { menuLifeBar.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(_onLifeCooldown / 60), Mathf.FloorToInt(_onLifeCooldown % 60)); }
        else if (_globalLives == 5)
            { menuLifeBar.transform.GetChild(3).GetComponent<Text>().text = "Full"; }
    }

    public void DisplayHealthStats()
    {
        float _c_hunger = PlayerPrefs.GetFloat("C-Hunger", 50);
        float _c_energy = PlayerPrefs.GetFloat("C-Energy", 50);
        float _c_fats = PlayerPrefs.GetFloat("C-Fats", 0);
        
        feedHungerBar.transform.GetComponent<Slider>().value = _c_hunger / 100.0f;
        feedHungerBar.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:0}%", _c_hunger);

        feedEnergyBar.transform.GetComponent<Slider>().value = _c_energy / 100.0f;
        feedEnergyBar.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:0}%", _c_energy);

        feedFatsBar.transform.GetComponent<Slider>().value = _c_fats / 100.0f;
        feedFatsBar.transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:0}%", _c_fats);
    }

    public void DisplayDialogue()
    {
        string _dialogue = PlayerPrefs.GetString("C-Dialogue", "null");
        dialogueBubble.transform.GetChild(0).GetComponent<Text>().text = _dialogue;

        Sprite _emotion = _characterSystem.emotions[PlayerPrefs.GetInt("C-EmoteId", 0)];
        character.transform.GetComponent<Image>().sprite = _characterSystem.emotions[PlayerPrefs.GetInt("C-EmoteId", 0)];
    }

    public void DisplayCoins()
    {
        float _globalCoins = PlayerPrefs.GetFloat("GlobalCoins", _coinSystem.initialCoins);
        feedCoinsBar.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0:0.00}", _globalCoins);
    }

    public void DisplayProfileStats()
    {
        int _numberCorrect = PlayerPrefs.GetInt("NumberCorrect", 0);
        int _numberIncorrect = PlayerPrefs.GetInt("NumberIncorrect", 0);
        int _globalRank = PlayerPrefs.GetInt("GlobalRank", 1);
        float _globalExperience = PlayerPrefs.GetFloat("GlobalExperience", 0);
        float _expAmountNeeded = PlayerPrefs.GetFloat("ExpAmountNeeded", 0);

        statsProfile.transform.GetChild(0).GetComponent<Text>().text = (_numberCorrect == 0 && _numberIncorrect == 0)
            ? "Accuracy: 100.00%"
            : string.Format("Accuracy: {0:0.00}", (float)_numberCorrect / (float)_numberIncorrect);
        statsProfile.transform.GetChild(1).GetComponent<Text>().text = "Rank: " + rankNames[_globalRank - 1];
        statsProfile.transform.GetChild(2).GetComponent<Text>().text = string.Format("Exp: {0:0.0} / {1:0} XP", _globalExperience, _expAmountNeeded);
    }
}
