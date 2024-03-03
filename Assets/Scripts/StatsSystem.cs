using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsSystem : MonoBehaviour
{
    public int numberCorrect;
    public int numberIncorrect;

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
        numberCorrect = PlayerPrefs.GetInt("NumberCorrect", 0);
        numberIncorrect = PlayerPrefs.GetInt("NumberIncorrect", 0);
    }

    private void Update()
    {
        numberCorrect = PlayerPrefs.GetInt("NumberCorrect", 0);
        numberIncorrect = PlayerPrefs.GetInt("NumberIncorrect", 0);
    }
}
