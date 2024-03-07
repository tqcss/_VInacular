using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*   LECTURE TYPE LEGEND
------------------------------
0 = Basic Lecture
1 = Translate: Multiple Choice
2 = Translate: Constructing
3 = Spell-checking
4 = Grammar-checking
5 = Pronunciation-checking
6 = Translate: Matching Type
7 = Conversate: Multiple Choice
8 = Conversate: Constructing
9 = Image-recognizing
*/

[CreateAssetMenu(fileName = "New Lecture", menuName = "Lecture")]
public class Lecture : ScriptableObject
{
    public string title;
    public int lectureType;
    public string[] englishTitle;
    public string[] filipinoTitle;
    public string[] englishWord;
    public string[] filipinoWord;
    public string[] akeanonWord;
    public string[] capiznonWord;
    public string[] hiligaynonWord;
    public string[] kinarayaWord;

    public string[] englishQuestion;
    public string[] filipinoQuestion;
    public string[] akeanonChoices;
    public string[] capiznonChoices;
    public string[] hiligaynonChoices;
    public string[] kinarayaChoices;
    public int correctChoice;
    public Image[] images; // for image multi choice

    public Dictionary<string, string[]> titles = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> wordEntries = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> questions = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> choiceEntries = new Dictionary<string, string[]>();

    public void loadLanguages() {
        titles.Add("english", englishTitle);
        titles.Add("filipino", filipinoTitle);

        wordEntries.Add("english", englishWord);
        wordEntries.Add("filipino", filipinoWord);
        wordEntries.Add("akeanon", akeanonWord);
        wordEntries.Add("capiznon", capiznonWord);
        wordEntries.Add("hiligaynon", hiligaynonWord);
        wordEntries.Add("kinaraya", kinarayaWord);

        questions.Add("english", englishQuestion);
        questions.Add("filipino", filipinoQuestion);

        choiceEntries.Add("akeanon", akeanonChoices);
        choiceEntries.Add("capiznon", capiznonChoices);
        choiceEntries.Add("hiligaynon", hiligaynonChoices);
        choiceEntries.Add("kinaraya", kinarayaChoices);
    }
}
