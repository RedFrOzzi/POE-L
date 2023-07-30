using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Database;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Reflection;

[CustomEditor(typeof(BuffsDatabase))]
public class BuffsDatabase_Inspector : Editor
{    
    private BuffsDatabase databaseSO;
    private List<MemberInfo> exposedMembers = new();

    private void OnEnable()
    {
        databaseSO = (BuffsDatabase)target;

        Type type = typeof(BuffsDatabase);
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        MemberInfo[] members = type.GetMembers(flags);

        foreach (var member in members)
        {
            ExposedField exposedField = member.GetCustomAttribute<ExposedField>();

            if (exposedField != null)
                exposedMembers.Add(member);
        }
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Show buffs names"))
            databaseSO.Show();
        
        GUILayout.Label("BUFFS", EditorStyles.boldLabel);
        GUILayout.Space(10);

        foreach (MemberInfo member in exposedMembers)
        {
            var property = serializedObject.FindProperty($"{member.Name}");
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Add all buffs to DB"))
            databaseSO.AddAllBuffsToDatabase();
    }
}



    //public override VisualElement CreateInspectorGUI()
    //{
    //    VisualElement DB_Inspector = new();
    //    TreeAsset.CloneTree(DB_Inspector);
    //    Default Inspector
    //    VisualElement inspectorFoldout = DB_Inspector.Q("Default_Inspector");
    //    InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);
    //    return DB_Inspector;
    //}