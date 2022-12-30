using UnityEditor;
using UnityEngine;

namespace XSJGame.Finder.Editor
{
    class FindPrefabByTextue : FindAssetWindowBase<Texture, UnityEngine.Object>
    {
        protected override void ConfigValues()
        {
            m_DisableFind = m_Asset == null;
            m_EnabledFindInScene = m_Asset != null;
        }

        protected override bool InGameObjectAndChildren(GameObject prefab)
        {
            return UnityUtil.AnyOneComponentAndChildren<Component>((comp) =>
            {
                if (FindUtil.IgnoreType(comp)) return false;
                return UnityUtil.AnyOneProperty((prop) =>
                {
                    return prop.propertyType == SerializedPropertyType.ObjectReference &&
                    prop.objectReferenceValue == m_Asset;

                }, comp);
            }, prefab.transform);
        }

    }
}

