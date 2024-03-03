using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinSystem : MonoBehaviour
{
    public float globalCoins;
    public float initialCoins;

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
        globalCoins = PlayerPrefs.GetFloat("GlobalCoins", initialCoins);
    }

    private void Update()
    {
        // Automatically update the player global coins
        globalCoins = PlayerPrefs.GetFloat("GlobalCoins", initialCoins);
    }

    public void IncreaseCoins(float increase)
    {
        // Increase coins by the amount prompted
        float _globalCoins_ = PlayerPrefs.GetFloat("GlobalCoins", initialCoins);
        PlayerPrefs.SetFloat("GlobalCoins", _globalCoins_ + increase);
    }

    public void DecreaseCoins(float decrease)
    {
        // Decrease coins by the amount prompted
        float _globalCoins = PlayerPrefs.GetFloat("GlobalCoins", initialCoins);
        PlayerPrefs.SetFloat("GlobalCoins", (_globalCoins >= decrease) ? _globalCoins - decrease : 0);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
