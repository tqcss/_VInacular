using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*   LECTURE TYPE LEGEND
------------------------------
0 = Basic Lecture
1 = Translate: Multiple Choice
2 = Translate: Constructing
3 = Spell-checking
4 = Grammar-checking
5 = Pronunciation-checking
*/

[CreateAssetMenu(fileName = "New Lecture", menuName = "Lecture")]
public class Lecture : ScriptableObject
{
    public string title;
    public int lectureType;
    public string englishDialog;
    public string filipinoDialog;
    public string hiligaynonDialog;
    public string kinarayaDialog;
    public string akeanonDialog;
    public string capiznonDialog;

    public Dictionary<string, string> language = new Dictionary<string, string>();

    public void loadLanguages() {
        language.Add("english", englishDialog);
        language.Add("filipino", filipinoDialog);
        language.Add("hiligaynon", hiligaynonDialog);
        language.Add("kinaraya", kinarayaDialog);
        language.Add("akeanon", akeanonDialog);
        language.Add("capiznon", capiznonDialog);
    }
}
