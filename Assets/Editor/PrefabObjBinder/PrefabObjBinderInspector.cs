using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabObjBinder), true)]
public class PrefabObjBinderInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Edit"))
        {
            PrefabObjBinderEditor.ShowWindow();
        }
        base.OnInspectorGUI();
    }
}
