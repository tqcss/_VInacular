using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSystem : MonoBehaviour
{
    public float c_hunger;
    public float c_energy;
    public float c_fats;
    public float minCooldown;
    public float maxCooldown;
    public float onHungerDepleting;
    public float onEnergyDepleting;
    public float onFatsDepleting;
    public string dialogue;
    public int emoteId;
    public Sprite[] emotions;

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
        c_hunger = PlayerPrefs.GetFloat("C-Hunger", 50);
        c_energy = PlayerPrefs.GetFloat("C-Energy", 50);
        c_fats = PlayerPrefs.GetFloat("C-Fats", 0);

        onHungerDepleting = PlayerPrefs.GetFloat("HungerDeplete", 0);
        onEnergyDepleting = PlayerPrefs.GetFloat("EnergyDeplete", 0);
        onFatsDepleting = PlayerPrefs.GetFloat("FatsDeplete", 0);

        DialogueChat(0);
    }

    private void Update()
    {
        // Automatically update the character's health-related attributes
        c_hunger = PlayerPrefs.GetFloat("C-Hunger", 50);
        c_energy = PlayerPrefs.GetFloat("C-Energy", 50);
        c_fats = PlayerPrefs.GetFloat("C-Fats", 0);
        PlayerPrefs.SetString("C-Dialogue", dialogue);
        PlayerPrefs.SetInt("C-EmoteId", emoteId);

        DepleteHunger();
        DepleteEnergy();
        DepleteFats();
    }

    private float RandomCooldown()
    {
        // Return a random maximum cooldown for depleting health-related stats from min to max
        return Random.Range(minCooldown, maxCooldown);
    }

    public void IncreaseHunger(float increase)
    {
        // Increase hunger by the amount prompted
        float _c_hunger = PlayerPrefs.GetFloat("C-Hunger", 50);
        if (_c_hunger + increase < 100)
            PlayerPrefs.SetFloat("C-Hunger", _c_hunger + increase);
        else
            PlayerPrefs.SetFloat("C-Hunger", 100);
    }

    public void IncreaseEnergy(float increase)
    {
        // Increase energy by the amount prompted
        float _c_energy = PlayerPrefs.GetFloat("C-Energy", 0);
        if (_c_energy + increase < 100)
            PlayerPrefs.SetFloat("C-Energy", _c_energy + increase);
        else
            PlayerPrefs.SetFloat("C-Energy", 100);
    }

    public void IncreaseFats(float increase)
    {
        // Increase fats by the amount prompted
        float _c_fats = PlayerPrefs.GetFloat("C-Fats", 0);
        if (_c_fats + increase < 100)
            PlayerPrefs.SetFloat("C-Fats", _c_fats + increase);
        else
            PlayerPrefs.SetFloat("C-Fats", 100);
    }

    private void DepleteHunger()
    {
        // Check if the character has more than zero hunger
        // Deplete the hunger value by 0.1 within random cooldown
        if (c_hunger > 0)
        {
            if (onHungerDepleting > 0)
            {
                onHungerDepleting -= Time.deltaTime;
            }
            else if (onHungerDepleting <= 0)
            {
                PlayerPrefs.SetFloat("C-Hunger", c_hunger - 0.1f);
                float randomCd = RandomCooldown();
                PlayerPrefs.SetFloat("HungerDeplete", randomCd);
                onHungerDepleting = randomCd;
            }
            if (Mathf.FloorToInt(onHungerDepleting % 1) == 0)
            {
                PlayerPrefs.SetFloat("HungerDeplete", Mathf.FloorToInt(onHungerDepleting));
            }
        }
    }

    private void DepleteEnergy()
    {
        // Check if the character has more than zero energy
        // Deplete the energy value by 0.1 within random cooldown * 1.75
        if (c_energy > 0)
        {
            if (onEnergyDepleting > 0)
            {
                onEnergyDepleting -= Time.deltaTime;
            }
            else if (onEnergyDepleting <= 0)
            {
                PlayerPrefs.SetFloat("C-Energy", c_energy - 0.1f);
                float randomCd = RandomCooldown() * 1.75f;
                PlayerPrefs.SetFloat("EnergyDeplete", randomCd);
                onEnergyDepleting = randomCd;
                DialogueChat(1);
            }
            if (Mathf.FloorToInt(onEnergyDepleting % 1) == 0)
            {
                PlayerPrefs.SetFloat("EnergyDeplete", Mathf.FloorToInt(onEnergyDepleting));
            }
        }
    }

    private void DepleteFats()
    {
        // Check if the character has more than zero fats
        // Deplete the fats value by 0.1 within random cooldown * 1.5
        if (c_fats > 0)
        {
            if (onFatsDepleting > 0)
            {
                onFatsDepleting -= Time.deltaTime;
            }
            else if (onFatsDepleting <= 0)
            {
                PlayerPrefs.SetFloat("C-Fats", c_fats - 0.1f);
                float randomCd = RandomCooldown() * 1.5f;
                PlayerPrefs.SetFloat("FatsDeplete", randomCd);
                onFatsDepleting = randomCd;
            }
            if (Mathf.FloorToInt(onFatsDepleting % 1) == 0)
            {
                PlayerPrefs.SetFloat("FatsDeplete", Mathf.FloorToInt(onFatsDepleting));
            }
        }
    }

    public void DialogueChat(int chatmodeId)
    {
        switch (chatmodeId)
        {
            case 0:
                if (c_hunger <= 10)
                    { dialogue = "I am hungry!"; emoteId = 4; }
                else if (c_hunger >= 95)
                    { dialogue = "I am full!"; emoteId = 3; }
                else
                    { dialogue = "I want food!"; emoteId = 0; }
                break;

            case 1:
                if (c_energy <= 5)
                    { dialogue = "I am tired!"; emoteId = 4; }
                break;
            
            case 2:
                dialogue = "I don't want it!"; emoteId = 7;
                break;

            case 3:
                dialogue = "Delicious!"; emoteId = 2;
                break;

            case 4:
                dialogue = "Not enough coins!"; emoteId = 7;
                break;

            case 5:
                emoteId = 1;
                break;
        }
    }
}
