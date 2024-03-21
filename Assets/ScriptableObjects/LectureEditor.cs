using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(Lecture))]
public class LectureEditor : Editor
{
    SerializedProperty lectureNumberProp;
    SerializedProperty lectureTypeProp;
    SerializedProperty englishTitleProp;
    SerializedProperty filipinoTitleProp;
    SerializedProperty englishWordsProp;
    SerializedProperty filipinoWordsProp;
    SerializedProperty akeanonWordsProp;
    SerializedProperty capiznonWordsProp;
    SerializedProperty hiligaynonWordsProp;
    SerializedProperty kinarayaWordsProp;
    SerializedProperty englishQuestionProp;
    SerializedProperty filipinoQuestionProp;
    SerializedProperty akeanonChoicesProp;
    SerializedProperty capiznonChoicesProp;
    SerializedProperty hiligaynonChoicesProp;
    SerializedProperty kinarayaChoicesProp;
    SerializedProperty correctChoiceProp;
    SerializedProperty imagesProp;

    private void OnEnable()
    {
        lectureNumberProp = serializedObject.FindProperty("lectureNumber");
        lectureTypeProp = serializedObject.FindProperty("lectureType");
        englishTitleProp = serializedObject.FindProperty("englishTitle");
        filipinoTitleProp = serializedObject.FindProperty("filipinoTitle");
        englishWordsProp = serializedObject.FindProperty("englishWords");
        filipinoWordsProp = serializedObject.FindProperty("filipinoWords");
        akeanonWordsProp = serializedObject.FindProperty("akeanonWords");
        capiznonWordsProp = serializedObject.FindProperty("capiznonWords");
        hiligaynonWordsProp = serializedObject.FindProperty("hiligaynonWords");
        kinarayaWordsProp = serializedObject.FindProperty("kinarayaWords");
        englishQuestionProp = serializedObject.FindProperty("englishQuestion");
        filipinoQuestionProp = serializedObject.FindProperty("filipinoQuestion");
        akeanonChoicesProp = serializedObject.FindProperty("akeanonChoices");
        capiznonChoicesProp = serializedObject.FindProperty("capiznonChoices");
        hiligaynonChoicesProp = serializedObject.FindProperty("hiligaynonChoices");
        kinarayaChoicesProp = serializedObject.FindProperty("kinarayaChoices");
        correctChoiceProp = serializedObject.FindProperty("correctChoice");
        imagesProp = serializedObject.FindProperty("images");
    }

    override public void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(lectureNumberProp, new GUIContent("Lecture Number"));
        EditorGUILayout.PropertyField(lectureTypeProp, new GUIContent("Lecture Type"));

        Lecture.LectureType currentLectureType = (Lecture.LectureType)lectureTypeProp.enumValueIndex;

        if (currentLectureType == Lecture.LectureType.Lecture)
        {
            EditorGUILayout.PrefixLabel("");
            EditorGUILayout.PropertyField(englishTitleProp, new GUIContent("English Title"));
            EditorGUILayout.PropertyField(filipinoTitleProp, new GUIContent("Filipino Title"));

            EditorGUILayout.PrefixLabel("");
            EditorGUILayout.PrefixLabel("Default Translation");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(englishWordsProp, new GUIContent("English Words"));
            EditorGUILayout.PropertyField(filipinoWordsProp, new GUIContent("Filipino Words"));
            EditorGUI.indentLevel--;

            EditorGUILayout.PrefixLabel("");
            EditorGUILayout.PrefixLabel("Native Translation");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(akeanonWordsProp, new GUIContent("Akeanon Words"));
            EditorGUILayout.PropertyField(capiznonWordsProp, new GUIContent("Capiznon Words"));
            EditorGUILayout.PropertyField(hiligaynonWordsProp, new GUIContent("Hiligaynon Words"));
            EditorGUILayout.PropertyField(kinarayaWordsProp, new GUIContent("Kinaray-a Words"));
            EditorGUI.indentLevel--;
        }
        else if (currentLectureType == Lecture.LectureType.TranslateMultipleChoice ||
                 currentLectureType == Lecture.LectureType.SpellChecking ||
                 currentLectureType == Lecture.LectureType.GrammarChecking ||
                 currentLectureType == Lecture.LectureType.ConversateMultipleChoice)
        {
            EditorGUILayout.PrefixLabel("");
            EditorGUILayout.PrefixLabel("Question");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(englishQuestionProp, new GUIContent("English Question"));
            EditorGUILayout.PropertyField(filipinoQuestionProp, new GUIContent("Filipino Question"));
            EditorGUI.indentLevel--;

            EditorGUILayout.PrefixLabel("");
            EditorGUILayout.PrefixLabel("Validate");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(correctChoiceProp, new GUIContent("Correct Choice"));
            EditorGUI.indentLevel--;

            EditorGUILayout.PrefixLabel("");
            EditorGUILayout.PrefixLabel("Choices");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(akeanonChoicesProp, new GUIContent("Akeanon Choices"));
            EditorGUILayout.PropertyField(capiznonChoicesProp, new GUIContent("Capiznon Choices"));
            EditorGUILayout.PropertyField(hiligaynonChoicesProp, new GUIContent("Hiligaynon Choices"));
            EditorGUILayout.PropertyField(kinarayaChoicesProp, new GUIContent("Kinaray-a Choices"));
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUILayout.PrefixLabel("");
            EditorGUILayout.PrefixLabel("Not available. Work in Progress.");
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif