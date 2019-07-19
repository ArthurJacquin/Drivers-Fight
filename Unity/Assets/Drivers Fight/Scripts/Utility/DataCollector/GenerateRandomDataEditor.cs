using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DataCollector))]
public class GenerateRandomDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DataCollector myScript = (DataCollector)target;
        if (GUILayout.Button("Generate data"))
        {
            myScript.GenerateData();
        }
    }
}
