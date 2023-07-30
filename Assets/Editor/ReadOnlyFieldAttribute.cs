//using UnityEngine;
//using JetBrains.Annotations;
//using UnityEditor;

//[System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
//public class ReadOnlyFieldAttribute : PropertyAttribute { }


//[UsedImplicitly, CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
//public class ReadOnlyFieldAttributeDrawer : PropertyDrawer
//{
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    => EditorGUI.GetPropertyHeight(property, label, true);

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        GUI.enabled = false;
//        EditorGUI.PropertyField(position, property, label, true);
//        GUI.enabled = true;

//    }
//}