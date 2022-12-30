﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace XSJGame.Finder.Editor
{
    class FindText : FindAssetWindowBase<Font, GameObject>
    {

        protected override void DoFind()
        {
            m_Items.Clear();
            var texts = new List<Text>();

            Finder.ForeachPrefabs((go, path)=> {
                texts.Clear();
                go.GetComponentsInChildren<Text>(true, texts);
                if (texts.Count > 0)
                    m_Items.Add(go);
            }, true, GetSearchInFolders());
            FillMatNames(m_Items);
        }

        protected override bool InGameObjectAndChildren(GameObject prefab)
        {
            throw new System.NotImplementedException();
        }

        private void FillMatNames(List<GameObject> mats)
        {
            m_ItemNames.Clear();
            for (var i = 0; i < mats.Count; i++)
            {
                m_ItemNames.Add(AssetDatabase.GetAssetPath(mats[i]));
            }
        }
    }
}

