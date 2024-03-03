using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject qualitySettings;
    public GameObject audioSettings;
    public GameObject displaySettings;
    public GameObject darkmodeSettings;

    public Color offHandleColor = new Color(111, 147, 189, 255);
    public Color onHandleColor = new Color(255, 255, 255, 255);
    
    private void Awake()
    {
        PrepareSettings(0);
        PrepareSettings(1);
    }

    private void PrepareSettings(int prepareId)
    {
        switch (prepareId)
        {
            case 0:
                int setHighQuality = PlayerPrefs.GetInt("SetHighQuality", 0);
                int setBatterySaver = PlayerPrefs.GetInt("SetBatterySaver", 0);
                int setMusic = PlayerPrefs.GetInt("SetMusic", 1);
                int setSoundFX = PlayerPrefs.GetInt("SetSoundFX", 1);
                int setVibration = PlayerPrefs.GetInt("SetVibration", 0);
                int setNotification = PlayerPrefs.GetInt("SetNotification", 0);
                int setDarkMode = PlayerPrefs.GetInt("SetDarkMode", 0);
                break;

            case 1:
                SwitchHighQuality();
                SwitchBatterySaver();
                SwitchMusic();
                SwitchSoundFX();
                SwitchVibration();
                SwitchNotification();
                SwitchDarkMode();
                break;
        }
    }

    public void SwitchHighQuality()
    {
        GameObject toggleHighQuality = qualitySettings.transform.GetChild(1).GetChild(0).gameObject;
        GameObject handleHighQuality = toggleHighQuality.transform.GetChild(0).gameObject;

        if (toggleHighQuality.GetComponent<Toggle>().isOn)
        {
            toggleHighQuality.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("SetHighQuality", (PlayerPrefs.GetInt("SetHighQuality", 0) == 1) ? 0 : 1);
        }

        handleHighQuality.GetComponent<RectTransform>().anchoredPosition = (PlayerPrefs.GetInt("SetHighQuality", 0) == 1) 
            ? handleHighQuality.GetComponent<RectTransform>().anchoredPosition * -1f 
            : handleHighQuality.GetComponent<RectTransform>().anchoredPosition;
        handleHighQuality.transform.GetComponent<Image>().color = (PlayerPrefs.GetInt("SetHighQuality", 0) == 1) ? onHandleColor : offHandleColor;
    }

    public void SwitchBatterySaver()
    {
        GameObject toggleBatterySaver = qualitySettings.transform.GetChild(2).GetChild(0).gameObject;
        GameObject handleBatterySaver = toggleBatterySaver.transform.GetChild(0).gameObject;

        if (toggleBatterySaver.GetComponent<Toggle>().isOn)
        {
            toggleBatterySaver.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("SetBatterySaver", (PlayerPrefs.GetInt("SetBatterySaver", 0) == 1) ? 0 : 1);
        }

        handleBatterySaver.GetComponent<RectTransform>().anchoredPosition = (PlayerPrefs.GetInt("SetBatterySaver", 0) == 1) 
            ? handleBatterySaver.GetComponent<RectTransform>().anchoredPosition * -1f 
            : handleBatterySaver.GetComponent<RectTransform>().anchoredPosition;
        handleBatterySaver.transform.GetComponent<Image>().color = (PlayerPrefs.GetInt("SetBatterySaver", 0) == 1) ? onHandleColor : offHandleColor;
    }

    public void SwitchMusic()
    {
        GameObject toggleMusic = audioSettings.transform.GetChild(1).GetChild(0).gameObject;
        GameObject handleMusic = toggleMusic.transform.GetChild(0).gameObject;

        if (toggleMusic.GetComponent<Toggle>().isOn)
        {
            toggleMusic.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("SetMusic", (PlayerPrefs.GetInt("SetMusic", 1) == 1) ? 0 : 1);
        }

        handleMusic.GetComponent<RectTransform>().anchoredPosition = (PlayerPrefs.GetInt("SetMusic", 1) == 1) 
            ? handleMusic.GetComponent<RectTransform>().anchoredPosition * -1f 
            : handleMusic.GetComponent<RectTransform>().anchoredPosition;
        handleMusic.transform.GetComponent<Image>().color = (PlayerPrefs.GetInt("SetMusic", 1) == 1) ? onHandleColor : offHandleColor;
    }

    public void SwitchSoundFX()
    {
        GameObject toggleSoundFX = audioSettings.transform.GetChild(2).GetChild(0).gameObject;
        GameObject handleSoundFX = toggleSoundFX.transform.GetChild(0).gameObject;

        if (toggleSoundFX.GetComponent<Toggle>().isOn)
        {
            toggleSoundFX.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("SetSoundFX", (PlayerPrefs.GetInt("SetSoundFX", 1) == 1) ? 0 : 1);
        }

        handleSoundFX.GetComponent<RectTransform>().anchoredPosition = (PlayerPrefs.GetInt("SetSoundFX", 1) == 1) 
            ? handleSoundFX.GetComponent<RectTransform>().anchoredPosition * -1f 
            : handleSoundFX.GetComponent<RectTransform>().anchoredPosition;
        handleSoundFX.transform.GetComponent<Image>().color = (PlayerPrefs.GetInt("SetSoundFX", 1) == 1) ? onHandleColor : offHandleColor;
    }

    public void SwitchVibration()
    {
        GameObject toggleVibration = audioSettings.transform.GetChild(3).GetChild(0).gameObject;
        GameObject handleVibration = toggleVibration.transform.GetChild(0).gameObject;

        if (toggleVibration.GetComponent<Toggle>().isOn)
        {
            toggleVibration.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("SetVibration", (PlayerPrefs.GetInt("SetVibration", 0) == 1) ? 0 : 1);
        }

        handleVibration.GetComponent<RectTransform>().anchoredPosition = (PlayerPrefs.GetInt("SetVibration", 0) == 1) 
            ? handleVibration.GetComponent<RectTransform>().anchoredPosition * -1f 
            : handleVibration.GetComponent<RectTransform>().anchoredPosition;
        handleVibration.transform.GetComponent<Image>().color = (PlayerPrefs.GetInt("SetVibration", 0) == 1) ? onHandleColor : offHandleColor;
    }

    public void SwitchNotification()
    {
        GameObject toggleNotification = displaySettings.transform.GetChild(1).GetChild(0).gameObject;
        GameObject handleNotification = toggleNotification.transform.GetChild(0).gameObject;

        if (toggleNotification.GetComponent<Toggle>().isOn)
        {
            toggleNotification.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("SetNotification", (PlayerPrefs.GetInt("SetNotification", 0) == 1) ? 0 : 1);
        }

        handleNotification.GetComponent<RectTransform>().anchoredPosition = (PlayerPrefs.GetInt("SetNotification", 0) == 1) 
            ? handleNotification.GetComponent<RectTransform>().anchoredPosition * -1f 
            : handleNotification.GetComponent<RectTransform>().anchoredPosition;
        handleNotification.transform.GetComponent<Image>().color = (PlayerPrefs.GetInt("SetNotification", 0) == 1) ? onHandleColor : offHandleColor;
    }

    public void SwitchDarkMode()
    {
        GameObject toggleDarkMode = darkmodeSettings.transform.GetChild(1).gameObject;
        GameObject handleDarkMode = toggleDarkMode.transform.GetChild(0).gameObject;

        if (toggleDarkMode.GetComponent<Toggle>().isOn)
        {
            toggleDarkMode.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("SetDarkMode", (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? 0 : 1);
        }

        handleDarkMode.GetComponent<RectTransform>().anchoredPosition = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) 
            ? handleDarkMode.GetComponent<RectTransform>().anchoredPosition * -1f 
            : handleDarkMode.GetComponent<RectTransform>().anchoredPosition;
        handleDarkMode.transform.GetComponent<Image>().color = (PlayerPrefs.GetInt("SetDarkMode", 0) == 1) ? onHandleColor : offHandleColor;
    }
    
    public void DeletePlayerData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
