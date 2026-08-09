using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveConfig))]
public class WaveConfigEditor : Editor
{
    private SerializedProperty waveName;
    private SerializedProperty waveType;
    private SerializedProperty enemies;

    private SerializedProperty timeBeforeWave;
    private SerializedProperty timeAfterWave;

    private SerializedProperty eliteHealthMultiplier;

    private SerializedProperty bossHealthMultiplier;
    private SerializedProperty bossSpeedMultiplier;

    private void OnEnable()
    {
        waveName = serializedObject.FindProperty("waveName");
        waveType = serializedObject.FindProperty("waveType");
        enemies = serializedObject.FindProperty("enemies");

        timeBeforeWave = serializedObject.FindProperty("timeBeforeWave");
        timeAfterWave = serializedObject.FindProperty("timeAfterWave");

        eliteHealthMultiplier =
            serializedObject.FindProperty("eliteHealthMultiplier");

        bossHealthMultiplier =
            serializedObject.FindProperty("bossHealthMultiplier");

        bossSpeedMultiplier =
            serializedObject.FindProperty("bossSpeedMultiplier");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(
            "Configuración de Oleada",
            EditorStyles.boldLabel
        );

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(
            waveName,
            new GUIContent("Nombre de la Oleada")
        );

        WaveType currentType =
            (WaveType)waveType.enumValueIndex;

        currentType = (WaveType)EditorGUILayout.EnumPopup(
            "Tipo de Oleada",
            currentType
        );

        waveType.enumValueIndex = (int)currentType;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(
            "Configuración de Tiempo",
            EditorStyles.boldLabel
        );

        EditorGUILayout.PropertyField(
            timeBeforeWave,
            new GUIContent("Tiempo Antes")
        );

        EditorGUILayout.PropertyField(
            timeAfterWave,
            new GUIContent("Tiempo Después")
        );

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(
            "Enemigos",
            EditorStyles.boldLabel
        );

        EditorGUILayout.PropertyField(
            enemies,
            new GUIContent("Enemigos de la Oleada"),
            true
        );

        EditorGUILayout.Space();

    
        if (currentType == WaveType.Elite)
        {
            EditorGUILayout.LabelField(
                "Configuración Elite",
                EditorStyles.boldLabel
            );

            EditorGUILayout.PropertyField(
                eliteHealthMultiplier,
                new GUIContent("Multiplicador de Vida")
            );
        }

        if (currentType == WaveType.Boss)
        {
            EditorGUILayout.LabelField(
                "Configuración Boss",
                EditorStyles.boldLabel
            );

            EditorGUILayout.PropertyField(
                bossHealthMultiplier,
                new GUIContent("Multiplicador de Vida")
            );

            EditorGUILayout.PropertyField(
                bossSpeedMultiplier,
                new GUIContent("Multiplicador de Velocidad")
            );
        }

        serializedObject.ApplyModifiedProperties();
    }
}
