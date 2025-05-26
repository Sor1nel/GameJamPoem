using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractionType))]
public class InteractionTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InteractionType script = (InteractionType)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("interactionType"));

        EditorGUILayout.Space();

        switch (script.interactionType)
        {
            case InteractionType.Interaction.MoveThere:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moveTarget"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moveSpeed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bobFrequency"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bobAmplitude"));
                break;

            case InteractionType.Interaction.CloseWindow:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("windowObject"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("windowLowerAmount"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("windowCloseSpeed"));

               
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isWindowOpen"));
                break;

            case InteractionType.Interaction.ChooseRadio:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("audioSource"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("channel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("radioClips"), true);
                break;

            case InteractionType.Interaction.ToggleObject:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("toggleTarget"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
