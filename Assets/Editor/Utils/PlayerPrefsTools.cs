using UnityEngine;
using UnityEditor;

public class PlayerPrefsTools 
{
    [MenuItem("Tools/ClearCache")]
    private static void ClearCache()
    {
        PlayerPrefs.DeleteAll();
    }
}
