using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*   

LECTURE TYPE LEGEND
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
    public enum LectureType { 
                            Lecture = 0, 
                            TranslateMultipleChoice = 1, 
                            TranslateConstructing = 2, 
                            SpellChecking = 3, 
                            GrammarChecking = 4, 
                            PronunciationChecking = 5, 
                            TranslateMatchingType = 6, 
                            ConversateMultipleChoice = 7, 
                            ConversateConstructing = 8, 
                            ImageRecognizing = 9 
                            };
    
    public string lectureNumber;
    public LectureType lectureType;

    [TextArea(4, 64)]
    public string englishTitle;
    [TextArea(4, 64)]
    public string filipinoTitle;

    public string[] englishWords;
    public string[] filipinoWords;
    public string[] akeanonWords;
    public string[] capiznonWords;
    public string[] hiligaynonWords;
    public string[] kinarayaWords;

    public string englishQuestion;
    public string filipinoQuestion;

    public string[] akeanonChoices;
    public string[] capiznonChoices;
    public string[] hiligaynonChoices;
    public string[] kinarayaChoices;

    public int correctChoice;
    public Image[] images; // for image multi choice

    public Dictionary<string, string> titles = new Dictionary<string, string>();
    public Dictionary<string, string[]> wordEntries = new Dictionary<string, string[]>();
    public Dictionary<string, string> questions = new Dictionary<string, string>();
    public Dictionary<string, string[]> choiceEntries = new Dictionary<string, string[]>();

    public void LoadLanguages() 
    {
        titles.Add("english", englishTitle);
        titles.Add("filipino", filipinoTitle);

        wordEntries.Add("english", englishWords);
        wordEntries.Add("filipino", filipinoWords);
        wordEntries.Add("akeanon", akeanonWords);
        wordEntries.Add("capiznon", capiznonWords);
        wordEntries.Add("hiligaynon", hiligaynonWords);
        wordEntries.Add("kinaraya", kinarayaWords);

        questions.Add("english", englishQuestion);
        questions.Add("filipino", filipinoQuestion);

        choiceEntries.Add("akeanon", akeanonChoices);
        choiceEntries.Add("capiznon", capiznonChoices);
        choiceEntries.Add("hiligaynon", hiligaynonChoices);
        choiceEntries.Add("kinaraya", kinarayaChoices);
    }
}
