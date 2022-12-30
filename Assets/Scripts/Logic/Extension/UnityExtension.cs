
using UnityEngine;

/// <summary>
/// UnityÕÿ’π
/// </summary>
static public class UnityExtension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (null == t)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }
}
