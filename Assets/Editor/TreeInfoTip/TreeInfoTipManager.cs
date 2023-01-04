using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace TreeInfoTip
{
    public class TreeInfoTipManager
    {
        private static TreeInfoTipManager _instance;

        public static TreeInfoTipManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TreeInfoTipManager();
                return _instance;
            }
        }
        
        public bool IsOpen = true;  //是否开启TreeInfoTip
        
        private Dictionary<string, string> _guid2Title;
        public Dictionary<string, string> Guid2Title
        {
            get
            {
                if (_guid2Title == null) 
                    CreateGuid2Title();
                return _guid2Title;
            }
        }

        private readonly string GUID = "guid";
        private readonly string TITLE = "title";

        public bool AddToGuid2Title(string guid, string title)
        {
            if (_guid2Title.ContainsKey(guid))
            {
                _guid2Title[guid] = title;
                UpdateDirectoryV2(guid, title);
            }
            else
            {
                _guid2Title.Add(guid, title);
                AddDirectoryV2(guid, title);
            }

            return true;
        }

        public string GetTitleByGuid(string guid)
        {
            if (Guid2Title.TryGetValue(guid, out string title))
            {
                return title;
            }
            return String.Empty;
        }

        private bool AddDirectoryV2(string guid, string title)
        {
            string path = Application.dataPath + "/Editor/TreeInfoTip/DirectoryV2.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            var root = xmlDoc.DocumentElement;
            
            var element = xmlDoc.CreateElement("tree");
            element.SetAttribute(GUID, guid);
            element.SetAttribute(TITLE, title);
            root.AppendChild(element);
            // TODO:将指定的节点紧接着插入指定的引用节点之后
            // root.InsertAfter(element, element);
            
            xmlDoc.Save(path);
            
            AssetDatabase.Refresh();
            return true;
        }
        
        //替换
        private bool UpdateDirectoryV2(string guid, string title)
        {
            string path = Application.dataPath + "/Editor/TreeInfoTip/DirectoryV2.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            
            var treeNodes = xmlDoc.SelectNodes("trees/tree");
            if (treeNodes != null)
            {
                foreach (XmlNode node in treeNodes)
                {
                    var element = node as XmlElement;
                    if (element == null)
                        continue;

                    if (element.GetAttribute(GUID) == guid)
                    {
                        element.SetAttribute(TITLE, title);
                        break;
                    }
                }
            }
            xmlDoc.Save(path);
            AssetDatabase.Refresh();
            return true;
        }
        
        //创建
        private void CreateGuid2Title()
        {
            Debug.Log("CreateGuid2Title()");
            string path = Application.dataPath + "/Editor/TreeInfoTip/DirectoryV2.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            
            var treeNodes = xmlDoc.SelectNodes("trees/tree");
            if (treeNodes != null)
            {
                _guid2Title = new Dictionary<string, string>(treeNodes.Count);
                foreach (XmlNode node in treeNodes)
                {
                    var element = node as XmlElement;
                    if (element == null)
                        continue;

                    string guid = element.GetAttribute(GUID);
                    string title = element.GetAttribute(TITLE);
                    _guid2Title.Add(guid, title);
                }
            }
        }
    }
}

