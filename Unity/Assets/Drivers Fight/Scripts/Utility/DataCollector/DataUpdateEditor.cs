using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataDisplayer))]
public class DataUpdateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DataDisplayer myScript = (DataDisplayer)target;
        if (GUILayout.Button("Update data display"))
        {
            myScript.UpdateData();
        }

        if (GUILayout.Button("Clear data display"))
        {
            myScript.ClearData();
        }
    }
}
