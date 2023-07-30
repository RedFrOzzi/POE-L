using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Database;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ItemSpecificMods))]
public class ItemSpecificMods_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(30);

        EditorGUILayout.LabelField("Prefixes");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        var prefNames = serializedObject.FindProperty("prefixesModNames");

        for (int i = 0; i < prefNames.arraySize; i++)
        {
            EditorGUILayout.TextField(prefNames.GetArrayElementAtIndex(i).stringValue, GUILayout.Width(200));
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();

        var prefWeights = serializedObject.FindProperty("prefixesModWeights");
        for (int i = 0; i < prefWeights.arraySize; i++)
        {
            EditorGUILayout.FloatField(prefWeights.GetArrayElementAtIndex(i).floatValue, GUILayout.Width(100));
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("Suffixes");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        var sufNames = serializedObject.FindProperty("suffixesModNames");

        for (int i = 0; i < sufNames.arraySize; i++)
        {
            EditorGUILayout.TextField(sufNames.GetArrayElementAtIndex(i).stringValue, GUILayout.Width(200));
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();

        var sufWeights = serializedObject.FindProperty("suffixesModWeights");
        for (int i = 0; i < sufWeights.arraySize; i++)
        {
            EditorGUILayout.FloatField(sufWeights.GetArrayElementAtIndex(i).floatValue, GUILayout.Width(100));
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
