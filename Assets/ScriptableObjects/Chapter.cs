using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chapter", menuName = "Chapter")]
public class Chapter : ScriptableObject
{
    public string title;
    public Lecture[] lectures;
}
