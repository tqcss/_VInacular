using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    public int globalLives;
    public int failsBeforeSuccess;
    public int maximumLife;
    public int maximumLifeCooldown;
    public float onLifeCooldown;
    public bool isInCooldown = false;
    
    private static GameObject s_instance {set; get;}
    
    private void Awake()
    {
        // Will not destroy the script when on the next loaded scene
        if (s_instance != null) 
            Destroy(s_instance);
        s_instance = gameObject;
        DontDestroyOnLoad(s_instance);

        // Reference the scripts from game objects


        // Set initial values to the variables 
        globalLives = PlayerPrefs.GetInt("GlobalLives", maximumLife);
        failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);
        onLifeCooldown = PlayerPrefs.GetFloat("OnLifeCooldown", maximumLifeCooldown);
    }

    private void Start()
    {
        OfflineCooldown(PlayerPrefs.GetInt("CheckOfflineLife", 1));
    }

    private void OfflineCooldown(int offline)
    {
        // Check if the user was offline
        if (offline == 1)
        {
            PlayerPrefs.SetInt("CheckOfflineLife", 0);
            // Get the current time
            DateTime timeCurrent = DateTime.Now;
            if (PlayerPrefs.HasKey("SavedTime"))
            {
                // Get the time saved after the user quitted from the previous session
                DateTime timeSaved = DateTime.Parse(PlayerPrefs.GetString("SavedTime"));
                // Compute the amount of time the user is offline
                TimeSpan timePassed = timeCurrent - timeSaved;
                float timeLeftFromOffline = (float)timePassed.TotalSeconds;
                
                // Decrease life cooldown or increase lives based on the amount of time the user is offline
                while (timeLeftFromOffline > 0)
                {
                    int _globalLives = PlayerPrefs.GetInt("GlobalLives", maximumLife);
                    float _onLifeCooldown = PlayerPrefs.GetFloat("OnLifeCooldown", maximumLifeCooldown);

                    // Set time left from offline to 0 if the player global life at maximum 
                    if (_globalLives >= maximumLife)
                        timeLeftFromOffline = 0;

                    // Decrease the time left from offline by the current life cooldown
                    // and increment player global life by one if there are more time left from offline than life cooldown
                    if (timeLeftFromOffline > _onLifeCooldown)
                    {
                        timeLeftFromOffline -= _onLifeCooldown;
                        PlayerPrefs.SetInt("GlobalLives", _globalLives + 1);
                        PlayerPrefs.SetFloat("OnLifeCooldown", maximumLifeCooldown);
                        onLifeCooldown = 0;
                    }
                    // Decrease the current life cooldown by the time left from offline
                    // if there are more life cooldown than time left from offline
                    else
                    {
                        PlayerPrefs.SetFloat("OnLifeCooldown", _onLifeCooldown - timeLeftFromOffline);
                        onLifeCooldown = _onLifeCooldown - timeLeftFromOffline;
                        timeLeftFromOffline = 0;
                    }
                }
            }
        }
    }

    public void RewardLife(int discount, bool isLatestRound)
    {
        // Check if the user has fails before success and it is the latest round
        if (discount > 0 && isLatestRound)
        {
            // Compute the amount of time for decreasing the life cooldown based on the discount
            float timeLeftFromDiscount = maximumLifeCooldown / (discount * 2);
            PlayerPrefs.SetInt("FailsBeforeSuccess", 0);

            // Decrease life cooldown based on fails before success
            while (timeLeftFromDiscount > 0)
            {
                int _globalLives = PlayerPrefs.GetInt("GlobalLives", maximumLife);
                float _onLifeCooldown = PlayerPrefs.GetFloat("OnLifeCooldown", maximumLifeCooldown);
                
                // Set time left from discount to 0 if the player global life at maximum 
                if (_globalLives >= maximumLife)
                    timeLeftFromDiscount = 0;

                // Decrease the time left from discount by the current life cooldown
                // and increment player global life by one if there are more time left from discount than life cooldown
                if (timeLeftFromDiscount > _onLifeCooldown)
                {
                    timeLeftFromDiscount -= _onLifeCooldown;
                    PlayerPrefs.SetInt("GlobalLives", _globalLives + 1);
                    PlayerPrefs.SetFloat("OnLifeCooldown", maximumLifeCooldown);
                    onLifeCooldown = 0;
                }
                // Decrease the current life cooldown by the time left from discount
                // if there are more life cooldown than time left from discount
                else
                {
                    PlayerPrefs.SetFloat("OnLifeCooldown", _onLifeCooldown - timeLeftFromDiscount);
                    onLifeCooldown = _onLifeCooldown - timeLeftFromDiscount;
                    timeLeftFromDiscount = 0;
                }
            }
        }
    }

    private void Update()
    {
        // Automatically update the player global life
        globalLives = PlayerPrefs.GetInt("GlobalLives", maximumLife);
        failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);

        // Check if the player has less life than the maximum
        if (globalLives < maximumLife)
        {
            if (!isInCooldown)
            {
                // Activate the cooldown if it is inactive
                isInCooldown = true;
            }
            else
            {
                // Decrease the life cooldown if it is more than 0
                if (onLifeCooldown > 0)
                {
                    onLifeCooldown -= Time.deltaTime;
                }
                // Increment player global life by one and reset the life cooldown if it reaches 0
                else if (onLifeCooldown <= 0)
                {
                    PlayerPrefs.SetInt("GlobalLives", globalLives + 1);
                    PlayerPrefs.SetFloat("OnLifeCooldown", maximumLifeCooldown);
                    onLifeCooldown = maximumLifeCooldown;
                    isInCooldown = false;
                }

                // Set the life cooldown to its floor
                if (Mathf.FloorToInt(onLifeCooldown % 1) == 0)
                {
                    PlayerPrefs.SetFloat("OnLifeCooldown", Mathf.FloorToInt(onLifeCooldown));
                }
            }
        }
        else
        {
            PlayerPrefs.SetFloat("OnLifeCooldown", maximumLifeCooldown);
            onLifeCooldown = maximumLifeCooldown;
            isInCooldown = false;
        }
    }

    private void OnApplicationQuit()
    {
        // Save the current time, check if offline, and reset fails before success upon quitting
        PlayerPrefs.SetString("SavedTime", DateTime.Now.ToString());
        PlayerPrefs.SetInt("CheckOfflineLife", 1);
        PlayerPrefs.SetInt("FailsBeforeSuccess", 0);
        PlayerPrefs.Save();
    }
}
