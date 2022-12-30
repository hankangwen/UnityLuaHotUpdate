using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;


namespace XSJGame.Finder.Editor
{
    using static UnityUtil;
    class FindMissingOnAllAssets : FindWindowBase<UnityObject>
    {
        protected override bool InGameObjectAndChildren(GameObject prefab)
        {
            return AnyOneComponentAndChildren<Component>(comp =>
            {
                return comp == null;
            }, prefab.transform);
        }
    }
}

