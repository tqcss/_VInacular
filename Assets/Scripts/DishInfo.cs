using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDish", menuName = "Dishes/New Dish")]
public class DishInfo : ScriptableObject
{
    // All ingredients for a particular dish must be referenced in the inspector
    
    // Attributes for object related
    public Sprite sprite;
    public float scaleX;
    public float scaleY;
    public float colliderSizeX;
    public float colliderSizeY;
    public Color particleColorA;
    public Color particleColorB;

    // Attributes for hunger and health-related stats
    public float hungerRestoration;
    public float energy;
    public float fats;

    // Attributes for buy point and description
    public float buyPoint;
    public string description;
}
